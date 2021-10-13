using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Guitars.API.Endpoints
{
    internal static class ErrorEndpoints
    {
        internal static void MapErrorEndpoints(this WebApplication app)
        {
            app.Map("/errors", LogError);
        }

        internal static IResult LogError(HttpContext httpContext)
        {
            var context = httpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error is ValidationException validationException)
            {
                return Results.BadRequest(new { errors = validationException.Errors.Select(x => x.ErrorMessage).ToList() });
            }

            // log the error

            return Results.Content($"<div>An error occurred!</div><div>Error Message: {context.Error.Message}</div><div>Stack Trace: {context.Error.StackTrace}</div>", "text/html");
        }
    }
}