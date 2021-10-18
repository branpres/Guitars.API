using Application.Guitars.Commands.CreateGuitar;
using Application.Guitars.Commands.DeleteGuitar;
using Application.Guitars.Commands.UpdateGuitar;
using Application.Guitars.Queries.ReadAllGuitars;
using Application.Guitars.Queries.ReadGuitar;
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
            app.MapPut("guitars", UpdateGuitarAsync);
            app.MapDelete("/guitars/{id}", DeleteGuitarAsync);
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
            return Results.Ok(guitarDto);
        }

        internal async static Task<IResult> UpdateGuitarAsync(ISender mediator, UpdateGuitarCommand updateGuitarCommand)
        {
            await mediator.Send(updateGuitarCommand);
            return Results.NoContent();
        }

        internal async static Task<IResult> DeleteGuitarAsync(ISender mediator, int id)
        {
            await mediator.Send(new DeleteGuitarCommand(id));
            return Results.NoContent();
        }
    }
}