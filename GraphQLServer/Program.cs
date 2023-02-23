using GraphQLServer;
using GraphQLServer.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddDbContext<PriceTrackerContext>()
    .AddSingleton<Repository>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");


app.MapGraphQL();


app.Run();
