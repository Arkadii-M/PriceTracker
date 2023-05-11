using GraphQLServer;
using GraphQLServer.GraphQL;
using GraphQLServer.DbModels;
using GraphQLServer.Services;
using System.Reflection;
using GraphQLServer.MapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GraphQLServer.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GraphQLServer.Services.Interface;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var profiles = new List<AutoMapper.Profile>()
{
    new HistoryProfile(),
    new ProductProfile(),
    new SellerProfile(),
    new SubscriptionProfile(),
    new UpdateProfile(),
    new UserProfile()
};

// Enable cors
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options =>
    {
        options.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthHelper.Issuer,

            ValidateAudience = true,
            ValidAudience = AuthHelper.Audience,

            ValidateLifetime = false,
            IssuerSigningKey = AuthHelper.GetSymmetricSecurityKey(),

            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();


// 
var host_name = Environment.GetEnvironmentVariable("RabbitMqHost") ?? throw new ArgumentException("Missing env var: RabbitMqHost");
var parser_exchange_key = Environment.GetEnvironmentVariable("ParserExchangeKey") ?? throw new ArgumentException("Missing env var: ParserExchangeKey");
var new_item_routing_key = Environment.GetEnvironmentVariable("NewItemRoutingKey") ?? throw new ArgumentException("Missing env var: NewItemRoutingKey");

builder.Services.AddSingleton<IRabbitMqClient>(impl => new RabbitMqClient(host_name, parser_exchange_key, new_item_routing_key));



var redis_connection_string = Environment.GetEnvironmentVariable("RedisConnectionString") ?? throw new ArgumentException("Missing env var: RedisConnectionString");
//var redis_host = Environment.GetEnvironmentVariable("RedisHost") ?? throw new ArgumentException("Missing env var: RedisHost");
//var redis_port = Environment.GetEnvironmentVariable("RedisPort") ?? throw new ArgumentException("Missing env var: RedisPort");
//var redis_password = Environment.GetEnvironmentVariable("RedisPassword") ?? throw new ArgumentException("Missing env var: RedisPassword");

builder.Services
    .AddTransient<IHistoryService,HistoryService>()
    .AddTransient<IProductService,ProductService>()
    .AddTransient<ISellerService,SellerService>()
    .AddTransient<ISubscriptionService,SubscriptionService>()
    .AddTransient<IUpdateService,UpdateService>()
    .AddTransient<IUserService,UserService>();

builder.Services
    .AddDbContextFactory<PriceTrackerContext>()
    .AddAutoMapper(config => config.AddProfiles(profiles))
    .AddGraphQLServer()
    .AddAuthorization()
    .RegisterDbContext<PriceTrackerContext>(DbContextKind.Synchronized)
    .RegisterService<IRabbitMqClient>()
    .RegisterService<IHistoryService>()
    .RegisterService<IProductService>()
    .RegisterService<ISellerService>()
    .RegisterService<ISubscriptionService>()
    .RegisterService<IUpdateService>()
    .RegisterService<IUserService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<GraphQlSubscription>()
    //.AddInMemorySubscriptions()
    .AddRedisSubscriptions((sp) => 
    ConnectionMultiplexer.Connect(redis_connection_string))
    .AddFiltering();

var app = builder.Build();

app.MapGet("/", () => "GraphQl is avaliable at /graphql");
app.MapGet("/healthcheck", () => "Ok");
//app.MapHealthChecks("/healthcheck");

app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.MapGraphQL();


app.Run();
