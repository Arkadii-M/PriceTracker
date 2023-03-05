using System.Text;
using RabbitMQ.Client;
using GraphQLDto;
using Newtonsoft.Json;
using RozetkaDto;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

const string quenue_name = "PARSER_INPUT_QUEUE";

channel.QueueDeclare(queue: quenue_name,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

const string url_ = "https://rozetka.com.ua/ua/bosch_0092s50050/p59504740/";

RozetkaParseInput product = new RozetkaParseInput(url_);

string message = JsonConvert.SerializeObject(product);

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: quenue_name,
                     basicProperties: null,
                     body: body);

Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

