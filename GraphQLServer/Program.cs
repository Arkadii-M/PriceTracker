using GraphQLServer;
using GraphQLServer.GraphQL;
using GraphQLServer.Models;
using GraphQLServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAutoMapper(config => config.CreateMap<User, UserPayLoad>())
    .AddDbContext<PriceTrackerContext>()
    .AddGraphQLServer()
    .RegisterDbContext<PriceTrackerContext>(DbContextKind.Synchronized)
    .AddQueryType<Query>();

//builder.Services
//    .AddDbContext<PriceTrackerContext>()
//    .AddTransient<UsersService>()
//    .AddGraphQLServer()
//    .RegisterService<UsersService>()
//    .AddQueryType<Query>();

//builder.Services
//    .AddScoped<IUserService,UsersService>()
//    .AddDbContext<PriceTrackerContext>()
//    .AddGraphQLServer()
//    .RegisterDbContext<PriceTrackerContext>(DbContextKind.Synchronized)
//    .AddQueryType<Query>()
//    .AddMutationType<Mutation>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");


app.MapGraphQL();


app.Run();
