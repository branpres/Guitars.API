namespace Guitars.API.Endpoints;

internal static class AuthenticationEndpoints
{
    internal static void MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/authentication/login", Login).AllowAnonymous();
        app.MapPost("/authentication/logout", Logout).RequireAuthorization();
        app.MapPost("/authentication/refreshtoken", RefreshToken).RequireAuthorization();            
    }

    internal async static Task<IResult> Login(ISender mediator, LoginCommand loginCommand)
    {
        var jwt = await mediator.Send(loginCommand);
        return Results.Ok(jwt);
    }

    internal async static Task<IResult> Logout(ISender mediator)
    {
        await mediator.Send(new LogoutCommand());
        return Results.NoContent();
    }

    internal async static Task<IResult> RefreshToken(ISender mediator)
    {
        var jwt = await mediator.Send(new RefreshTokenCommand());
        return Results.Ok(jwt);
    }
}