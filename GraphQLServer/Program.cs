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


// Add auth

//var signingKey = new SymmetricSecurityKey(
//    Encoding.UTF8.GetBytes("MySuperSecretKey"));
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
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters =
//            new TokenValidationParameters
//            {
//                ValidIssuer = "https://auth.chillicream.com",
//                ValidAudience = "https://graphql.chillicream.com",
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = signingKey,
//                ValidateLifetime = false,
//            };
//    });


//builder.Services.AddHealthChecks();

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
app.MapGet("/healthcheck", () => "Ok");
//app.MapHealthChecks("/healthcheck");

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();


app.Run();
