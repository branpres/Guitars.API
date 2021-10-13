using Guitars.Application.Guitars.Commands.CreateGuitar;
using MediatR;

namespace Guitars.API.Endpoints
{
    internal static class GuitarsEndpoints
    {
        internal static void MapGuitarEndpoints(this WebApplication app)
        {
            app.MapPost("/guitars", CreateAsync);
        }

        internal async static Task<IResult> CreateAsync(ISender mediator, CreateGuitarCommand createGuitarCommand)
        {
            var id = await mediator.Send(createGuitarCommand);
            return Results.CreatedAtRoute($"/guitars/{id}");
        }
    }
}