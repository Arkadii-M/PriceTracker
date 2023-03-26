﻿using Api;
using Api.Controllers;
using Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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



// Authorization
builder.Services.AddAuthorization();
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


// Add graphQl client

string graphql_server_uri = Environment
    .GetEnvironmentVariable("GraphQlServerAddress") 
    ?? throw new ArgumentException("Missing env var: GraphQlServerAddress");

builder.Services.AddPriceTrackerClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(graphql_server_uri));

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowOrigin");

app.MapControllers();

app.MapUserEndpoints();

app.MapGet("/", () => "Swagger is avaliable at /swagger");

app.MapSubscriptionEndpoints();

app.MapProductEndpoints();

app.Run();
