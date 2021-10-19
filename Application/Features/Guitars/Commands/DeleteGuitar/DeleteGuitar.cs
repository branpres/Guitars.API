using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Guitars.Commands.DeleteGuitar
{
    public class DeleteGuitar : IRequest
    {
        public DeleteGuitar(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }

    public class DeleteGuitarHandler : IRequestHandler<DeleteGuitar>
    {
        private readonly IGuitarsContext _guitarContext;

        public DeleteGuitarHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<Unit> Handle(DeleteGuitar request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(guitar), request.Id);
            }

            guitar.IsDeleted = true;
            guitar.GuitarStrings.ForEach(x => x.IsDeleted = true);
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}