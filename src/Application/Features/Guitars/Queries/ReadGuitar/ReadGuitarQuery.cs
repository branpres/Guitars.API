using Application.Common.Exceptions;
using Application.Data;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Guitars.Queries.ReadGuitar
{
    public class ReadGuitarQuery : IRequest<GuitarDto>
    {
        public ReadGuitarQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }

    public class ReadGuitarQueryHandler : IRequestHandler<ReadGuitarQuery, GuitarDto>
    {
        private readonly GuitarsContext _guitarContext;

        public ReadGuitarQueryHandler(GuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<GuitarDto> Handle(ReadGuitarQuery request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(Guitar), request.Id);
            }

            return guitar?.MapToDto();
        }
    }
}