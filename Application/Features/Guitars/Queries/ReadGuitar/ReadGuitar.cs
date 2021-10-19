using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Guitars.Queries.ReadGuitar
{
    public class ReadGuitar : IRequest<GuitarDto>
    {
        public ReadGuitar(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }

    public class ReadGuitarHandler : IRequestHandler<ReadGuitar, GuitarDto>
    {
        private readonly IGuitarsContext _guitarContext;

        public ReadGuitarHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<GuitarDto> Handle(ReadGuitar request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(guitar), request.Id);
            }

            return guitar?.MapToDto();
        }
    }
}