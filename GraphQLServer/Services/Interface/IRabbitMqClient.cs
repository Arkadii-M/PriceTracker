namespace GraphQLServer.Services.Interface
{
    public interface IRabbitMqClient
    {
        public void SendUrlToParse(string url);
    }
}
