using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RozetkaDto;
using System.Text;
using Newtonsoft.Json;
using StrawberryShake;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using DataConsumer.State;

namespace DataConsumer
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _logger;

        //RabbitMq

        //// Envrimoment variables
        private readonly string host_name;
        // Exchange key
        private readonly string consumer_exchange_key;
        // Routing keys
        private readonly string new_item_routing_key;
        private readonly string update_item_routing_key;

        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;

        private EventingBasicConsumer new_item_consumer;
        private EventingBasicConsumer update_item_consumer;



        // GraphQl client
        private IPriceTrackerClient _priceTrackerClient;

        public ConsumerWorker(ILogger<ConsumerWorker> logger, IPriceTrackerClient priceTrackerClient)
        {
            _logger = logger;
            _priceTrackerClient = priceTrackerClient;

            // parse envrimoment variables
            host_name = Environment
                .GetEnvironmentVariable("RabbitMqHost") ?? throw new ArgumentException("Missing env var: RabbitMqHost");
            consumer_exchange_key = Environment
                .GetEnvironmentVariable("ConsumerExchangeKey") ?? throw new ArgumentException("Missing env var: ConsumerExchangeKey");

            new_item_routing_key = Environment
                .GetEnvironmentVariable("NewItemRoutingKey") ?? throw new ArgumentException("Missing env var: NewItemRoutingKey");
            update_item_routing_key = Environment
                .GetEnvironmentVariable("UpdateItemRoutingKey") ?? throw new ArgumentException("Missing env var: UpdateItemRoutingKey");


            factory = new ConnectionFactory { HostName = host_name };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: consumer_exchange_key, type: ExchangeType.Direct);

            // declare a consumer-named queue. The name is not crucial, so Rabbitmq will generate one.
            var queueName = channel.QueueDeclare().QueueName;

            // Bind two (new, update) queues
            channel.QueueBind(queue: queueName, exchange: consumer_exchange_key, routingKey: new_item_routing_key);
            channel.QueueBind(queue: queueName, exchange: consumer_exchange_key, routingKey: update_item_routing_key);

            //TODO: make async consumers

            // Add cosumer when adding the new item
            new_item_consumer = new EventingBasicConsumer(channel);
            new_item_consumer.Received += OnNewItemRecived;
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: new_item_consumer);

            // Add consumer to update exist item
            update_item_consumer = new EventingBasicConsumer(channel);
            update_item_consumer.Received += OnUpdateItemRecived;
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: update_item_consumer);


        }

        static IOperationResult<T> TaskWaitAndGetResult<T>(Task<IOperationResult<T>> task) where T :class
        {
            task.Wait();
            var res = task.Result;
            res.EnsureNoErrors();
            return res;
        }

        private void OnNewItemRecived(object? sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());
            _logger.LogInformation("On new item recived message: {msg}", message);

            var page_result = JsonConvert.DeserializeObject<RozetkaPageResult>(message);

            if (page_result is null)
            {
                _logger.LogError("Failed to parse message:\n{msg}", message);
                return;
            }
            _logger.LogInformation(
                "Recived item:\nTitle: {title}, price: {price}, datetime {datetime}, in stock: {in_stock}, seller name: {seller}",
                page_result.product_title,
                page_result.price,
                page_result.datetime,
                page_result.in_stock,
                page_result.seller_name);


            var sellers_payload = TaskWaitAndGetResult(_priceTrackerClient.GetAllSellers.ExecuteAsync());

            long seller_id;
            var seller = sellers_payload.Data.Sellers.ToList().Find(s => s.SellerName == page_result.seller_name);

            if (seller is null) // If this seller is not exist, add to the database
            {
                var add_seller_payload = TaskWaitAndGetResult(_priceTrackerClient.AddSeller.ExecuteAsync(new SellerQLInput { SellerName = page_result.seller_name }));
                seller_id = add_seller_payload.Data.AddSeller.SellerId;
            }
            else
                seller_id = seller.SellerId;

            var add_product_payload = TaskWaitAndGetResult(_priceTrackerClient.AddProduct.ExecuteAsync(new ProductQLInput { SellerId = seller_id, Name = page_result.product_title, Link = page_result.url }));



            var add_history_payload = TaskWaitAndGetResult(_priceTrackerClient.AddHistory.ExecuteAsync(
                new HistoryQLInput
                {
                    ProductId = add_product_payload.Data.AddProduct.ProductId,
                    Datetime = page_result.datetime,
                    Price = page_result.price,
                    InStock = page_result.in_stock
                }));


            if (add_history_payload.IsSuccessResult())
                _logger.LogInformation("Add history to DB with id {id}", add_history_payload.Data.AddHistory.HistoryId);
            else
                _logger.LogError("Failed to add data to DB: {error}", add_history_payload.Errors.ToString());
        }

        private void OnUpdateItemRecived(object? sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());

            //TODO: test

            var page_result = JsonConvert.DeserializeObject<RozetkaPageResult>(message);


            if (page_result is null)
            {
                _logger.LogError("Failed to parse message:\n{msg}", message);
                return;
            }
            _logger.LogInformation(
                "Recived item:\nTitle: {title}, price: {price}, datetime {datetime}, in stock: {in_stock}, seller name: {seller}",
                page_result.product_title,
                page_result.price,
                page_result.datetime,
                page_result.in_stock,
                page_result.seller_name);

            // Store to database
            if (page_result.id is null)
            {
                _logger.LogError("Recived product id is null, but valid id expected.");
                return;
            }

            var add_history_payload = TaskWaitAndGetResult(_priceTrackerClient.AddHistory.ExecuteAsync(
                new HistoryQLInput
                {
                    ProductId = page_result.id ?? default(long),
                    Datetime = page_result.datetime,
                    Price = page_result.price,
                    InStock = page_result.in_stock
                }));

            if (add_history_payload.IsSuccessResult())
                _logger.LogInformation("Add history to DB with id {id}", add_history_payload.Data.AddHistory.HistoryId);
            else
                _logger.LogError("Failde to add data to DB: {error}", add_history_payload.Errors.ToString());
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