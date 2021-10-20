﻿using Application.Common.Exceptions;
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

            return Results.Content($"<div>An error occurred!</div><div>Error Message: {context.Error.Message}</div><div>Stack Trace: {context.Error.StackTrace}</div>", "text/html");
        }
    }
}