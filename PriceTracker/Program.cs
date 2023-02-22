using System.Text;
using RabbitMQ.Client;


var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string quenue_name = "to_parse_url";

channel.QueueDeclare(queue: quenue_name,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

const string message = "https://rozetka.com.ua/ua/364193610/p364193610/";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: quenue_name,
                     basicProperties: null,
                     body: body);

Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();