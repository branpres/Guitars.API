namespace Guitars.API.Endpoints;

internal static class ErrorEndpoints
{
    internal static void MapErrorEndpoints(this WebApplication app)
    {
        app.Map("/errors", HandleErrors);
    }

    internal static IResult HandleErrors(HttpContext httpContext)
    {
        // TODO: log the error

        var context = httpContext.Features.Get<IExceptionHandlerFeature>();

        if (context.Error is ValidationException validationException)
        {
            return Results.BadRequest(new { errors = validationException.Errors.Select(x => x.ErrorMessage).ToList() });
        }

        if (context.Error is NotFoundException notFoundException)
        {
            return Results.NotFound(notFoundException.Message);
        }

        if (context.Error is InvalidLoginException)
        {
            return Results.BadRequest("Invalid login");
        }

        if (context.Error is UserLockedOutException)
        {
            return Results.BadRequest("User account is locked out");
        }

        if (context.Error is TokenValidationException tokenValidationException)
        {
            return Results.BadRequest(tokenValidationException.Message);
        }

        return Results.Content($"<div>An error occurred!</div><div>Error Message: {context.Error.Message}</div><div>Stack Trace: {context.Error.StackTrace}</div>", "text/html");
    }
}