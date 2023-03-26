using PriceTrackScheduler;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        string graphql_server_uri = Environment
        .GetEnvironmentVariable("GraphQlServerAddress") ?? throw new ArgumentException("Missing env var: GraphQlServerAddress");

        services.AddHostedService<SchedulerWorker>()
        .AddPriceTrackerClient()
        .ConfigureHttpClient(client => client.BaseAddress = new Uri(graphql_server_uri));
    })
    .Build();

await host.RunAsync();
