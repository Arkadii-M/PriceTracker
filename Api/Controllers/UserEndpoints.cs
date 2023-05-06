using Api.Helpers;
using DTO.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.IdentityModel.Tokens;
using StrawberryShake;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

namespace Api.Controllers;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        //routes.MapGet("/api/User", async ([FromServices] IPriceTrackerClient client) =>
        //{
        //    var result = await client.GetAllUsers.ExecuteAsync();
        //    result.EnsureNoErrors();
        //    return result.Data.Users;
        //})
        //.WithName("GetAllUsers").RequireAuthorization();

        //routes.MapGet("/api/User/{id}", async (int id, [FromServices] IPriceTrackerClient client) =>
        //{
        //    var user_payload = await client.GetUserById.ExecuteAsync(id);
        //    user_payload.EnsureNoErrors();
        //    var user = user_payload.Data.Users.FirstOrDefault();
        //    if (user == null)
        //        return Results.NotFound();

        //    return Results.Ok(new { UserId = user.UserId, Username = user.Username });
        //})
        //.WithName("GetUserById").RequireAuthorization();

        //routes.MapPut("/api/User/{id}", (int id, User input) =>
        //{
        //    return Results.NoContent();
        //})
        //.WithName("UpdateUser");

        routes.MapPost("/api/User/", async ([FromBody] User model, [FromServices] IPriceTrackerClient client) =>
        {
            var add_user = await client.AddUser.ExecuteAsync(new CreateUserQLInput() { Username = model.Username, Password = model.Password });
            add_user.EnsureNoErrors();
            var new_user = add_user.Data.AddUser;
            return Results.Created($"/Users/{new_user.UserId}", new_user);
        })
        .WithName("CreateUser");

        routes.MapPost("/api/User/login", async ([FromBody] User user, [FromServices] IPriceTrackerClient client) =>
        {
            //TODO: check user and password
            var response = await client.LoginUser.ExecuteAsync(new LoginUserQLInput() { Username = user.Username, Password = user.Password });
            response.EnsureNoErrors();
            if (response.Data.LoginUser.Is_login)
            {
                var claims = new List<Claim> { 
                    new Claim("id",response.Data.LoginUser.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                };
                claims.Add(new Claim(ClaimTypes.Role, "user"));

                var jwt = new JwtSecurityToken(
                    issuer: AuthHelper.Issuer,
                    audience: AuthHelper.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(AuthHelper.TokenLifetime),
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                        AuthHelper.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256
                        )
                    );
                return Results.Ok(new { username = user.Username, token = new JwtSecurityTokenHandler().WriteToken(jwt) });
            }

            return Results.Unauthorized();
            
        }).WithName("LoginUser");
    }
}
