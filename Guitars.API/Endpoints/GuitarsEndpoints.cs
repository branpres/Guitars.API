using Guitars.Application.Guitars.Commands.CreateGuitar;
using Guitars.Application.Guitars.Queries.ReadAllGuitars;
using Guitars.Application.Guitars.Queries.ReadGuitar;
using MediatR;

namespace Guitars.API.Endpoints
{
    internal static class GuitarsEndpoints
    {
        internal static void MapGuitarEndpoints(this WebApplication app)
        {
            app.MapPost("/guitars", CreateGuitarAsync);
            app.MapGet("/guitars", ReallAllGuitarsAsync);
            app.MapGet("/guitars/{id}", ReadGuitarAsync);
        }

        internal async static Task<IResult> CreateGuitarAsync(ISender mediator, CreateGuitarCommand createGuitarCommand)
        {
            var id = await mediator.Send(createGuitarCommand);
            return Results.CreatedAtRoute($"/guitars/{id}");
        }

        internal async static Task<IResult> ReallAllGuitarsAsync(ISender mediator)
        {
            var guitarsVM = await mediator.Send(new ReadAllGuitarsQuery());
            return Results.Ok(guitarsVM);
        }

        internal async static Task<IResult> ReadGuitarAsync(ISender mediator, int id)
        {
            var guitarDto = await mediator.Send(new ReadGuitarQuery(id));
            if (guitarDto != null)
            {
                return Results.Ok(guitarDto);
            }

            return Results.NotFound(id);
        }
    }
}