﻿using Application.Features.Guitars.Commands.CreateGuitar;
using Application.Features.Guitars.Commands.DeleteGuitar;
using Application.Features.Guitars.Commands.StringGuitar;
using Application.Features.Guitars.Commands.TuneGuitar;
using Application.Features.Guitars.Commands.UpdateGuitar;
using Application.Features.Guitars.Queries.ReadGuitar;
using Application.Features.Guitars.Queries.ReadGuitars;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Guitars.API.Endpoints
{
    internal static class GuitarsEndpoints
    {
        internal static void MapGuitarsEndpoints(this WebApplication app)
        {
            app.MapPost("/guitars", CreateGuitarAsync).RequireAuthorization();
            app.MapGet("/guitars/{id}", ReadGuitarAsync).RequireAuthorization();
            app.MapGet("/guitars", ReadGuitarsAsync).RequireAuthorization();
            app.MapPut("/guitars", UpdateGuitarAsync).RequireAuthorization();
            app.MapDelete("/guitars/{id}", DeleteGuitarAsync).RequireAuthorization();
            app.MapPost("/guitars/string", StringGuitarAsync).RequireAuthorization();
            app.MapPost("/guitars/tune", TuneGuitarAsync).RequireAuthorization();
        }

        internal async static Task<IResult> CreateGuitarAsync(ISender mediator, CreateGuitarCommand createGuitarCommand)
        {
            var id = await mediator.Send(createGuitarCommand);
            return Results.Created($"/guitars/{id}", id);
        }

        internal async static Task<IResult> ReadGuitarAsync(ISender mediator, int id)
        {
            var guitarDto = await mediator.Send(new ReadGuitarQuery(id));
            return Results.Ok(guitarDto);
        }

        internal async static Task<IResult> ReadGuitarsAsync(ISender mediator, string filter = "", int pageIndex = -1, int pageSize = -1)
        {
            var guitarsVM = await mediator.Send(new ReadGuitarsQuery(filter, pageIndex == -1 ? null : pageIndex, pageSize == -1 ? null : pageSize));
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