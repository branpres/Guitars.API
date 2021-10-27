using Application.Features.Authentication.Commands.Login;
using MediatR;

namespace Guitars.API.Endpoints
{
    internal static class AuthenticationEndpoints
    {
        internal static void MapAuthenticationEndpoints(this WebApplication app)
        {
            app.MapPost("/authentication/login", Login);
        }

        internal async static Task<IResult> Login(ISender mediator, string email, string password)
        {
            var jwt = await mediator.Send(new LoginCommand(email, password));
            return Results.Ok(jwt);
        }
    }
}