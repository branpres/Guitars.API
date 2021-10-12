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

            // I am aware this isn't actually logging anything. I was mainly interested in seeing how to use this type of error handling.
            return Results.Json(new { ErrorMessage = context.Error.Message, context.Error.StackTrace });
        }
    }
}