using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Guitars.Commands.StringGuitar
{
    public class StringGuitar : GuitarStringCommand
    {
        public StringGuitar(int id, List<GuitarStringDto> guitarStrings) : base(id, guitarStrings) { }
    }

    public class StringGuitarHandler : IRequestHandler<StringGuitar>
    {
        private readonly IGuitarsContext _guitarContext;

        public StringGuitarHandler(IGuitarsContext guitarContext)
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
                throw new NotFoundException(nameof(guitar), request.Id);
            }

            request.GuitarStrings.ForEach(x => guitar.String(x.Number, x.Gauge, x.Tuning));
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}