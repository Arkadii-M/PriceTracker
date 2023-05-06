namespace Api.Helpers.Services
{
    public interface IRabbitMqClient
    {
        public void SendUrlToParse(string url);
    }
}
