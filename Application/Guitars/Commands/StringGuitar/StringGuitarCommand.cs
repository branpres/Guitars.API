using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guitars.Commands.StringGuitar
{
    public class StringGuitarCommand : GuitarStringCommand
    {
        public StringGuitarCommand(int id, List<GuitarStringDto> guitarStrings) : base(id, guitarStrings) { }
    }

    public class StringGuitarCommandHandler : IRequestHandler<StringGuitarCommand>
    {
        private readonly IGuitarsContext _guitarContext;

        public StringGuitarCommandHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<Unit> Handle(StringGuitarCommand request, CancellationToken cancellationToken)
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