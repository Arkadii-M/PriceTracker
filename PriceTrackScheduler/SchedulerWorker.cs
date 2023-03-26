using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using StrawberryShake;
using System.Text;
using Newtonsoft.Json;
using DTO.Rozetka;

namespace PriceTrackScheduler
{
    public class SchedulerWorker : BackgroundService
    {
        private readonly ILogger<SchedulerWorker> _logger;


        //RabbitMq

        //// Envrimoment variables
        private readonly string host_name;
        // Exchange key
        private readonly string parser_exchange_key;
        private readonly string parser_update_route_key;

        private readonly int WaitMilliseconds;


        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;

        // GraphQl client
        private IPriceTrackerClient _priceTrackerClient;

        public SchedulerWorker(ILogger<SchedulerWorker> logger, IPriceTrackerClient priceTrackerClient)
        {
            _logger = logger;
            _priceTrackerClient = priceTrackerClient;

            host_name = Environment.GetEnvironmentVariable("RabbitMqHost") ?? throw new ArgumentException("Missing env var: RabbitMqHost");

            parser_exchange_key = Environment.GetEnvironmentVariable("ParserExchangeKey") ?? throw new ArgumentException("Missing env var: ParserExchangeKey");
            parser_update_route_key = Environment.GetEnvironmentVariable("ParserUpdateRouteKey") ?? throw new ArgumentException("Missing env var: ParserUpdateRouteKey");
            WaitMilliseconds = Convert.ToInt32(Environment.GetEnvironmentVariable("WaitMilliseconds") ?? throw new ArgumentException("Missing env var: WaitMilliseconds"));

            factory = new ConnectionFactory { HostName = host_name };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Declare parse exchange
            channel.ExchangeDeclare(exchange: parser_exchange_key, type: ExchangeType.Direct);
            _logger.LogInformation("SchedulerWorker is constructed");
        }

        private void CheckProductsToUpdate()
        {
            _logger.LogInformation("CheckProductsToUpdate is called");
            var task = _priceTrackerClient.CheckForUpdates.ExecuteAsync(DateTime.Now);
            task.Wait();
            var products_to_update = task.Result;
            products_to_update.EnsureNoErrors();


            foreach(var element in products_to_update.Data.Updates)
            {
                var product = element.Subscription.Product;
                channel.BasicPublish(
                    exchange: parser_exchange_key,
                    routingKey: String.Empty,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(
                            new RozetkaParseInput(product.Link, product.ProductId, parser_update_route_key)
                            )));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    CheckProductsToUpdate();
                }
                catch(Exception e)
                {
                    _logger.LogError(e.Message);
                }
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(WaitMilliseconds, stoppingToken);
            }
        }
    }
}