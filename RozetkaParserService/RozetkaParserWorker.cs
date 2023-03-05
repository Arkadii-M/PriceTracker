using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using AngleSharp.Text;
using System.Text.RegularExpressions;
using RozetkaDto;
using Newtonsoft.Json;
using GraphQLDto;

namespace RozetkaParserService
{
    public class RozetkaParserWorker : BackgroundService
    {
        private readonly ILogger<RozetkaParserWorker> _logger;
        //RabbitMQ
        private readonly string in_queue_name = "parse_service_input";
        private readonly string out_queue_name = "parse_service_output";
        private readonly string host_name = "parse_service_output";

        //private const string tracker_in_name = "to_parse_url";
        //private const string tracker_out_name = "to_add_to_database";
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private EventingBasicConsumer consumer;

        public RozetkaParserWorker(ILogger<RozetkaParserWorker> logger)
        {
            _logger = logger;
            // parse envrimoment variables
            host_name = Environment.GetEnvironmentVariable("RabbitMqHost") ?? throw new ArgumentException("Missing env var: RabbitMqHost");
            in_queue_name = Environment.GetEnvironmentVariable("InputQueueName") ?? throw new ArgumentException("Missing env var: InputQueueName");
            out_queue_name = Environment.GetEnvironmentVariable("OutputQueueName") ?? throw new ArgumentException("Missing env var: OutputQueueName");


            factory = new ConnectionFactory { HostName = host_name };
            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.QueueDeclare(queue: in_queue_name,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.QueueDeclare(queue: out_queue_name,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
           catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
        //private void ParseUrl(string url,out RozetkaPageResult info)
        //{
        //    info = new RozetkaPageResult();
        //    _logger.LogInformation("Recived url: {0}", url);

        //    try
        //    {
        //        var options = new ChromeOptions();
        //        options.AddArgument("--headless");
        //        options.AddArgument("--no-sandbox");
        //        options.AddArgument("--disable-dev-shm-usage");

        //        using var driver = new ChromeDriver(@"/chromedriver", options);

        //       driver.Navigate().GoToUrl(url);
        //        try
        //        {
        //            var price_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.CssSelector("p.product-price__big")));
        //            var title_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.ClassName("product__title")));
        //            var in_stock_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.ClassName("status-label")));
        //            //TODO: check if all elemetns read correctly

        //            // fill the object
        //            string price_str = price_element.Text;
        //            price_str = price_str.Replace(" ", "");
        //            price_str = price_str.Remove(price_str.Length - 1);// remove currency symbol

        //            info.datetime = DateTime.Now;
        //            info.product_title = title_element.Text;
        //            info.price = decimal.Parse(price_str, System.Globalization.CultureInfo.InvariantCulture);
        //            info.in_stock = in_stock_element.Text == "Є в наявності" ? true : false;



        //            _logger.LogInformation("Title: {0}, price: {1}, datetime {2}, in stock: {3}",
        //                info.product_title,
        //                info.price,
        //                info.datetime,
        //                info.in_stock);
        //        }
        //        catch (Exception exception)
        //        {
        //            _logger.LogError(exception.Message);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError(exception.Message);
        //    }
        //}
        private void ParseProduct(RozetkaParseInput product, out RozetkaPageResult info)
        {
            _logger.LogInformation("Recived url: {url}", product.url);

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            using var driver = new ChromeDriver(@"/chromedriver", options);
            var rozetka_parser = new RozetkaParser(driver);
            var procuct_page = rozetka_parser.GetProductPageByUrl(product.url);
            info = procuct_page.ParsePage();
            _logger.LogInformation("Title: {title}, price: {price}, datetime {datetime}, in stock: {in_stock}",
                info.product_title,
                info.price,
                info.datetime,
                info.in_stock);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        RozetkaPageResult out_info;
                        var product_input = JsonConvert.DeserializeObject<RozetkaParseInput>(Encoding.UTF8.GetString(e.Body.ToArray()));

                        ParseProduct(product_input, out out_info);
                        channel.BasicPublish(
                            exchange: string.Empty,
                            routingKey: out_queue_name,
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(
                                JsonConvert.SerializeObject(out_info)
                                ));
                    };
                    channel.BasicConsume(in_queue_name, true, consumer);
                });
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}