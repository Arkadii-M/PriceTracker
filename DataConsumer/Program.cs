using DataConsumer;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        string graphql_server_uri =  Environment
        .GetEnvironmentVariable("GraphQlServerAddress") ?? throw new ArgumentException("Missing env var: GraphQlServerAddress");
        services
        .AddHostedService<ConsumerWorker>()
        .AddPriceTrackerClient()
        .ConfigureHttpClient(client => client.BaseAddress = new Uri(graphql_server_uri));
    })
    .Build();


//var serviceCollection = new ServiceCollection();

//serviceCollection
//    .AddCryptoClient()
//    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://api-crypto-workshop.chillicream.com/graphql"));

//IServiceProvider services = serviceCollection.BuildServiceProvider();


await host.RunAsync();
