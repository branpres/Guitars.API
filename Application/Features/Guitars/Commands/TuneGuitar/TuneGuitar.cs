using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Guitars.Commands.TuneGuitar
{
    public class TuneGuitar : GuitarStringCommand
    {
        public TuneGuitar(int id, List<GuitarStringDto> guitarStrings) : base(id, guitarStrings) { }
    }

    public class TuneGuitarHandler : IRequestHandler<TuneGuitar>
    {
        private readonly IGuitarsContext _guitarContext;

        public TuneGuitarHandler(IGuitarsContext guitarContext)
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
                throw new NotFoundException(nameof(guitar), request.Id);
            }

            request.GuitarStrings.ForEach(x => guitar.Tune(x.Number, x.Tuning));
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}