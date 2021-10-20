using Application.Common.Exceptions;
using Application.Data;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Guitars.Commands.StringGuitar
{
    public class StringGuitar : IRequest
    {
        public StringGuitar(int id, List<StringDto> strings)
        {
            Id = id;
            Strings = strings;
        }

        public int Id { get; private set; }

        public List<StringDto> Strings { get; private set; }
    }

    public class StringGuitarHandler : IRequestHandler<StringGuitar>
    {
        private readonly GuitarsContext _guitarContext;

        public StringGuitarHandler(GuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<Unit> Handle(StringGuitar request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(Guitar), request.Id);
            }

            request.Strings.ForEach(x => guitar.String(x.Number, x.Gauge, x.Tuning));
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}