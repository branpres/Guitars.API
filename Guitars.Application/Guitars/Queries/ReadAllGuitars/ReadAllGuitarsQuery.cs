using Guitars.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guitars.Application.Guitars.Queries.ReadAllGuitars
{
    public class ReadAllGuitarsQuery : IRequest<GuitarsVM> { }

    public class ReadAllGuitarsQueryHandler : IRequestHandler<ReadAllGuitarsQuery, GuitarsVM>
    {
        private readonly IGuitarsContext _guitarContext;

        public ReadAllGuitarsQueryHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<GuitarsVM> Handle(ReadAllGuitarsQuery request, CancellationToken cancellationToken)
        {
            return new GuitarsVM
            {
                Guitars = await _guitarContext.Guitar
                    .Include(x => x.GuitarStrings)
                    .Select(x => x.MapToDto())
                    .ToListAsync(cancellationToken)
            };
        }
    }
}