using Application.Authentication;
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.RefreshToken;
using MediatR;

namespace Guitars.API.Endpoints
{
    internal static class AuthenticationEndpoints
    {
        internal static void MapAuthenticationEndpoints(this WebApplication app)
        {
            app.MapPost("/authentication/login", Login).AllowAnonymous();
            app.MapPost("/authentication/refreshtoken", RefreshToken).RequireAuthorization(Constants.Policies.WRITE);
        }

        internal async static Task<IResult> Login(ISender mediator, LoginCommand loginCommand)
        {
            var jwt = await mediator.Send(loginCommand);
            return Results.Ok(jwt);
        }

        internal async static Task<IResult> RefreshToken(ISender mediator)
        {
            var jwt = await mediator.Send(new RefreshTokenCommand());
            return Results.Ok(jwt);
        }
    }
}