using GraphQLServer;
using GraphQLServer.GraphQL;
using GraphQLServer.GraphQL.Queries;
using GraphQLServer.DbModels;
using GraphQLServer.Services;

var builder = WebApplication.CreateBuilder(args);



//builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<IHistoryService,HistoryService>();

builder.Services
    .AddDbContextFactory<PriceTrackerContext>()
//    .AddAutoMapper(config => config.CreateMap<User, GraphQLDto.User.UserPayload_QL>())
     .AddAutoMapper(config => config.CreateMap<History, GraphQLDto.History.History_QL>())
//    .AddDbContext<PriceTrackerContext>()
    .AddGraphQLServer()
    .RegisterDbContext<PriceTrackerContext>(DbContextKind.Synchronized)
//    .RegisterService<UserService>()
    //.RegisterService<UpdateService>()
    //.RegisterService<SubscriptionService>()
    //.RegisterService<SellerService>()
//    .RegisterService<ProductService>()
    .RegisterService<HistoryService>()
//    .AddQueryType<UserQuery>()
    //.AddQueryType<UserQuery>();
    //.AddQueryType<UpdateQuery>()
    //.AddQueryType<SubscriptionQuery>()
    //.AddQueryType<SellerQuery>()
    //.AddQueryType<ProductQuery>();
    .AddQueryType<HistoryQuery>();

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
