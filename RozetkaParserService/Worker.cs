using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using AngleSharp.Text;
using System.Text.RegularExpressions;

namespace RozetkaParserService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //RabbitMQ
        private const string tracker_in_name = "to_parse_url";
        private const string tracker_out_name = "to_add_to_database";
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private EventingBasicConsumer consumer;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            factory = new ConnectionFactory { HostName = "172.18.0.2" };
            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.QueueDeclare(queue: tracker_in_name,
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);


                //consumer = new EventingBasicConsumer(channel);
                //consumer.Received += Consumer_Received;
            }
           catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            //channel.BasicConsume(queue: tracker_in_name,
            //         autoAck: true,
            //         consumer: consumer);
        }
        private void ParseUrl(string url)
        {
            _logger.LogInformation("Recived url: {0}", url);
            try
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                using var driver = new ChromeDriver(@"/chromedriver", options);

                driver.Navigate().GoToUrl(url);
                try
                {
                    var price_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.CssSelector("p.product-price__big")));
                    var title_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.ClassName("product__title")));
                    var title = title_element.Text;
                    string price_str = price_element.Text;
                    price_str = price_str.Replace(" ", "");
                    price_str = price_str.Remove(price_str.Length - 1);// remove currency symbol
                    _logger.LogInformation("Name {name}, price: {price}",
                        title,
                        double.Parse(price_str, System.Globalization.CultureInfo.InvariantCulture));
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
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
                        ParseUrl(Encoding.UTF8.GetString(e.Body.ToArray()));
                    };
                    channel.BasicConsume(tracker_in_name, true, consumer);
                });
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}