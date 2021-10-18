using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guitars.Commands.DeleteGuitar
{
    public class DeleteGuitarCommand : IRequest<int>
    {
        public DeleteGuitarCommand(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }

    public class DeleteGuitarCommandHandler : IRequestHandler<DeleteGuitarCommand, int>
    {
        private readonly IGuitarsContext _guitarContext;

        public DeleteGuitarCommandHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<int> Handle(DeleteGuitarCommand request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (guitar != null)
            {
                _guitarContext.Guitar.Remove(guitar);
                return await _guitarContext.SaveChangesAsync(cancellationToken);
            }

            return 0;
        }
    }
}