using Application.Common.Exceptions;
using Application.Data;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Guitars.Commands.TuneGuitar
{
    public class TuneGuitar : IRequest
    {
        public TuneGuitar(int id, List<TuningDto> tunings)
        {
            Id = id;
            Tunings = tunings;
        }

        public int Id { get; private set; }

        public List<TuningDto> Tunings { get; private set; }
    }

    public class TuneGuitarHandler : IRequestHandler<TuneGuitar>
    {
        private readonly GuitarsContext _guitarContext;

        public TuneGuitarHandler(GuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<Unit> Handle(TuneGuitar request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(Guitar), request.Id);
            }

            request.Tunings.ForEach(x => guitar.Tune(x.Number, x.Tuning));
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}