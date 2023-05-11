using DTO.Rozetka;
using GraphQLServer.Services.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

public class RabbitMqClient : IRabbitMqClient
{
    private readonly string _hostName;
    private readonly string _parserExchangeKey;
    private readonly string _routingKey;

    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqClient(string hostName, string parserExchangeKey, string routingKey)
    {

        _hostName = hostName;
        _parserExchangeKey = parserExchangeKey;
        _routingKey = routingKey;

        // init
        _factory = new ConnectionFactory { HostName = _hostName };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _parserExchangeKey, type: ExchangeType.Direct);
    }
    public void SendUrlToParse(string url)
    {
        _channel.BasicPublish(
                exchange: _parserExchangeKey,
                routingKey: String.Empty,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new RozetkaParseInput(url, null, _routingKey)
                        ))
                );
    }

}