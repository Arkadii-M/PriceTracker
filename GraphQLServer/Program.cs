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

//builder.Services.AddTransient<UserService>();
builder.Services
    .AddTransient<IHistoryService,HistoryService>()
    .AddTransient<IProductService,ProductService>()
    .AddTransient<ISellerService,SellerService>()
    .AddTransient<ISubscriptionService,SubscriptionService>()
    .AddTransient<IUpdateService,UpdateService>()
    .AddTransient<IUserService,UserService>();

builder.Services
    .AddDbContextFactory<PriceTrackerContext>()
//    .AddAutoMapper(config => config.CreateMap<User, GraphQLDto.User.UserPayload_QL>())
     .AddAutoMapper(config => config.AddProfiles(profiles))
//    .AddDbContext<PriceTrackerContext>()
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
