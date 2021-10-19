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
            app.MapGet("/guitars", ReallGuitarsAsync);
            app.MapPut("/guitars", UpdateGuitarAsync);
            app.MapDelete("/guitars/{id}", DeleteGuitarAsync);
            app.MapPost("/guitars/string", StringGuitarAsync);
            app.MapPut("/guitars/tune", TuneGuitarAsync);
        }

        internal async static Task<IResult> CreateGuitarAsync(ISender mediator, CreateGuitar createGuitar)
        {
            var id = await mediator.Send(createGuitar);
            return Results.CreatedAtRoute($"/guitars/{id}");
        }

        internal async static Task<IResult> ReadGuitarAsync(ISender mediator, int id)
        {
            var guitarDto = await mediator.Send(new ReadGuitar(id));
            return Results.Ok(guitarDto);
        }

        internal async static Task<IResult> ReallGuitarsAsync(ISender mediator, string filter, int? pageIndex = null, int? pageSize = null)
        {
            var guitarsVM = await mediator.Send(new ReadGuitars(filter, pageIndex, pageSize));
            return Results.Ok(guitarsVM);
        }               

        internal async static Task<IResult> UpdateGuitarAsync(ISender mediator, UpdateGuitar updateGuitar)
        {
            await mediator.Send(updateGuitar);
            return Results.NoContent();
        }

        internal async static Task<IResult> DeleteGuitarAsync(ISender mediator, int id)
        {
            await mediator.Send(new DeleteGuitar(id));
            return Results.NoContent();
        }

        internal async static Task<IResult> StringGuitarAsync(ISender mediator, StringGuitar stringGuitar)
        {
            await mediator.Send(stringGuitar);
            return Results.NoContent();
        }

        internal async static Task<IResult> TuneGuitarAsync(ISender mediator, TuneGuitar tuneGuitar)
        {
            await mediator.Send(tuneGuitar);
            return Results.NoContent();
        }
    }
}