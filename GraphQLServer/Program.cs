using GraphQLServer;
using GraphQLServer.GraphQL;
using GraphQLServer.DbModels;
using GraphQLServer.Services;
using System.Reflection;
using GraphQLServer.MapperProfiles;

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
    .RegisterDbContext<PriceTrackerContext>(DbContextKind.Synchronized)
    .RegisterService<IHistoryService>()
    .RegisterService<IProductService>()
    .RegisterService<ISellerService>()
    .RegisterService<ISubscriptionService>()
    .RegisterService<IUpdateService>()
    .RegisterService<IUserService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering();

var app = builder.Build();


app.MapGet("/", () => "GraphQl is avaliable at /graphql");


app.MapGraphQL();


app.Run();
