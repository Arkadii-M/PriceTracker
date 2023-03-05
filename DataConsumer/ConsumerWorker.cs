using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RozetkaDto;
using System.Text;
using Newtonsoft.Json;
using StrawberryShake;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Channels;

namespace DataConsumer
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _logger;

        // Envrimoment variables
        private readonly string host_name;
        private readonly string in_schedule_queue;
        private readonly string in_api_queue;
        private readonly string in_parsed_queue;



        //RabbitMq
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private EventingBasicConsumer consumer;

        // GraphQl client
        private IPriceTrackerClient _priceTrackerClient;

        public ConsumerWorker(ILogger<ConsumerWorker> logger, IPriceTrackerClient priceTrackerClient)
        {
            _logger = logger;
            _priceTrackerClient = priceTrackerClient;

            // parse envrimoment variables
            host_name = Environment.GetEnvironmentVariable("RabbitMqHost") ?? throw new ArgumentException("Missing env var: RabbitMqHost");
            in_parsed_queue = Environment.GetEnvironmentVariable("InputQueueName") ?? throw new ArgumentException("Missing env var: InputQueueName");

            factory = new ConnectionFactory { HostName = host_name };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: in_parsed_queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnQueueRecived;
        }

        private void OnQueueRecived(object? sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());
            var result = JsonConvert.DeserializeObject<RozetkaPageResult>(message);
            if (result is not null)
            {
                _logger.LogInformation(
                    "Title: {title}, price: {price}, datetime {datetime}, in stock: {in_stock}",
                    result.product_title,
                    result.price,
                    result.datetime,
                    result.in_stock);

                // Store to database
                var input_history = new HistoryQLInput { ProductId = 1, Datetime = result.datetime, Price = result.price, InStock = result.in_stock };
                var res = _priceTrackerClient.AddHistory.ExecuteAsync(input_history);
                res.Wait();
                var input_payload = res.Result;
                input_payload.EnsureNoErrors();
                if (input_payload.IsSuccessResult())
                    _logger.LogInformation("Add history to DB with id {id}", input_payload.Data.AddHistory.HistoryId);

            }
            else
            {
                _logger.LogWarning("Failde to parse message:\n{msg}", message);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                channel.BasicConsume(in_parsed_queue, true, consumer);
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}