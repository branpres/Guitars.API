﻿using Guitars.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guitars.Application.Guitars.Queries.ReadGuitar
{
    public class ReadGuitarQuery : IRequest<GuitarDto?>
    {
        public ReadGuitarQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }

    public class ReadGuitarQueryHandler : IRequestHandler<ReadGuitarQuery, GuitarDto?>
    {
        private readonly IGuitarsContext _guitarContext;

        public ReadGuitarQueryHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<GuitarDto?> Handle(ReadGuitarQuery request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            return guitar?.MapToDto();
        }
    }
}