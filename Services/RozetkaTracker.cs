using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Services
{
    partial class RozetkaTracker : ServiceBase
    {
        private EventLog eventLog;
        //private Timer timer;

        //RabbitMQ
        const string tracker_in_name = "to_parse_url";
        const string tracker_out_name = "to_add_to_database";
        readonly ConnectionFactory factory;
        readonly IConnection connection;
        readonly IModel channel;
        readonly EventingBasicConsumer consumer;
        public RozetkaTracker()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;

            //Initialize logger
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("RozetkaParserLog"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RozetkaParserLog", "RozetkaParserDebug");
            }
            eventLog.Source = "RozetkaParserLog";
            eventLog.Log = "RozetkaParserDebug";
            //Initialize RabbitMQ
            try
            {
                factory = new ConnectionFactory { HostName = "localhost" };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.QueueDeclare(queue: tracker_in_name,
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);


                consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
            }
            catch(Exception e)
            {
                eventLog.WriteEntry(e.Message,EventLogEntryType.Error);
            }
            channel.BasicConsume(queue: tracker_in_name,
                     autoAck: true,
                     consumer: consumer);



            //// setup timer
            //TimerCallback timerDelegate = new TimerCallback(this.ParseRozetkaPrices);
            //timer = new System.Threading.Timer(timerDelegate, null, Timeout.Infinite, 60000); // every 60 seconds, not start in constructor

        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            // get url
            //TODO: read few messages if exists
            var url = Encoding.UTF8.GetString(e.Body.ToArray());
            eventLog.WriteEntry(String.Format("Got url: {0}", url), EventLogEntryType.Information);
            try
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless");
                using (var driver = new ChromeDriver(@"C:\ChromeDriver\chromedriver_win32", options))
                {
                    driver.Navigate().GoToUrl(url);
                    try
                    {
                        var price_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.ClassName("product-prices__big")));
                        var title_element = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(drv => drv.FindElement(By.ClassName("product__title")));
                        eventLog.WriteEntry(
                            String.Format("Name {0},price: {1}",
                            title_element.Text,
                            Convert.ToDouble(price_element.Text.Remove(price_element.Text.Length - 1))), EventLogEntryType.Information);
                    }
                    catch (Exception exception)
                    {
                        eventLog.WriteEntry(exception.Message, EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception exception)
            {
                eventLog.WriteEntry(exception.Message, EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("Rozetka parser start", EventLogEntryType.Information);
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Rozetka parser stop", EventLogEntryType.Information);
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
