using Microsoft.AspNetCore.Mvc;
using Valtuutus.RealWorld.Api.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Valtuutus.RealWorld.Api.Features.Users;

public static class UsersEndpoints
{
    private static async Task<IResult> CreateUser([FromServices] CreateUserHandler createUserHandler,
        [FromBody] CreateUser req, CancellationToken ct)
    {
        return (await createUserHandler.Handle(req, ct)).ToApiResult();
    }

    private static async Task<IResult> Login([FromServices] LoginHandler loginHandler, [FromBody] Login req,
        CancellationToken ct)
    {
        return (await loginHandler.Handle(req, ct)).ToApiResult();
    }

    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", CreateUser);
        app.MapPost("users/login", Login);
    }
}