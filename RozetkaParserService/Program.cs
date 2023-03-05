using RozetkaParserService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RozetkaParserWorker>();
    })
    .Build();

await host.RunAsync();
