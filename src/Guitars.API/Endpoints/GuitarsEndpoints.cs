using Application.Features.Guitars.Commands.CreateGuitar;
using Application.Features.Guitars.Commands.DeleteGuitar;
using Application.Features.Guitars.Commands.StringGuitar;
using Application.Features.Guitars.Commands.TuneGuitar;
using Application.Features.Guitars.Commands.UpdateGuitar;
using Application.Features.Guitars.Queries.ReadGuitar;
using Application.Features.Guitars.Queries.ReadGuitars;
using MediatR;

namespace Guitars.API.Endpoints
{
    internal static class GuitarsEndpoints
    {
        internal static void MapGuitarEndpoints(this WebApplication app)
        {
            app.MapPost("/guitars", CreateGuitarAsync);
            app.MapGet("/guitars/{id}", ReadGuitarAsync);
            app.MapGet("/guitars", ReadGuitarsAsync);
            app.MapPut("/guitars", UpdateGuitarAsync);
            app.MapDelete("/guitars/{id}", DeleteGuitarAsync);
            app.MapPost("/guitars/string", StringGuitarAsync);
            app.MapPut("/guitars/tune", TuneGuitarAsync);
        }

        internal async static Task<IResult> CreateGuitarAsync(ISender mediator, CreateGuitarCommand createGuitarCommand)
        {
            var id = await mediator.Send(createGuitarCommand);
            return Results.CreatedAtRoute($"/guitars/{id}");
        }

        internal async static Task<IResult> ReadGuitarAsync(ISender mediator, int id)
        {
            var guitarDto = await mediator.Send(new ReadGuitarQuery(id));
            return Results.Ok(guitarDto);
        }

        internal async static Task<IResult> ReadGuitarsAsync(ISender mediator, string? filter = null, int? pageIndex = null, int? pageSize = null)
        {
            var guitarsVM = await mediator.Send(new ReadGuitarsQuery(filter, pageIndex, pageSize));
            return Results.Ok(guitarsVM);
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

        internal async static Task<IResult> StringGuitarAsync(ISender mediator, StringGuitarCommand stringGuitarCommand)
        {
            await mediator.Send(stringGuitarCommand);
            return Results.NoContent();
        }

        internal async static Task<IResult> TuneGuitarAsync(ISender mediator, TuneGuitarCommand tuneGuitarCommand)
        {
            await mediator.Send(tuneGuitarCommand);
            return Results.NoContent();
        }
    }
}