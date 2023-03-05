using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace DataConsumer
{
    public class QueueHelper
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public QueueHelper(ConnectionFactory factory)
        {
            _factory = factory;
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void AddQueueDeclaration(string queue_name)
        {
            _channel.QueueDeclare(queue: queue_name,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public EventingBasicConsumer CreateConsumer()
        {
            return new EventingBasicConsumer(_channel);
        }

    }
}
