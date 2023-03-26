using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using AngleSharp.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using DTO.Rozetka;

namespace RozetkaParserService
{
    public class RozetkaParserWorker : BackgroundService
    {
        private readonly ILogger<RozetkaParserWorker> _logger;
        //RabbitMq

        //// Envrimoment variables
        private readonly string host_name;
        // Exchange key
        private readonly string parser_exchange_key;
        private readonly string consumer_exchange_key;

        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;

        private EventingBasicConsumer parse_consumer;


        public RozetkaParserWorker(ILogger<RozetkaParserWorker> logger)
        {
            _logger = logger;
            // parse envrimoment variables
            host_name = Environment.GetEnvironmentVariable("RabbitMqHost") ?? throw new ArgumentException("Missing env var: RabbitMqHost");

            parser_exchange_key = Environment.GetEnvironmentVariable("ParserExchangeKey") ?? throw new ArgumentException("Missing env var: ParserExchangeKey");
            consumer_exchange_key = Environment.GetEnvironmentVariable("ConsumerExchangeKey") ?? throw new ArgumentException("Missing env var: ConsumerExchangeKey");

            factory = new ConnectionFactory { HostName = host_name };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Declare two exchanges
            channel.ExchangeDeclare(exchange: parser_exchange_key, type: ExchangeType.Direct);
            channel.ExchangeDeclare(exchange: consumer_exchange_key, type: ExchangeType.Direct);

            // declare a parser-named queue. The name is not crucial, so Rabbitmq will generate one.
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName, exchange: parser_exchange_key, routingKey: String.Empty);// no routing key

            parse_consumer = new EventingBasicConsumer(channel);
            parse_consumer.Received += OnItemRecived;
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: parse_consumer);


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

        private void OnItemRecived(object? sender, BasicDeliverEventArgs e)
        {

            RozetkaPageResult out_info;
            var product_input = JsonConvert.DeserializeObject<RozetkaParseInput>(Encoding.UTF8.GetString(e.Body.ToArray()));


            ParseProduct(product_input, out out_info);

            out_info.id = product_input.id; // save the id
            out_info.url = product_input.url;


            channel.BasicPublish(
                exchange: consumer_exchange_key,
                routingKey: product_input.routing_key,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(out_info)));
        }
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

            _logger.LogInformation("Page parsed with data:\nTitle: {title}, price: {price}, datetime {datetime}, in stock: {in_stock}",
                info.product_title,
                info.price,
                info.datetime,
                info.in_stock);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}