using Application.Common.Exceptions;
using Application.Data;
using Domain.Models;
using MediatR;

namespace Application.Features.Guitars.Commands.UpdateGuitar
{
    public class UpdateGuitar : IRequest
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }
    }

    public class UpdateGuitarHandler : IRequestHandler<UpdateGuitar>
    {
        private readonly GuitarsContext _guitarContext;

        public UpdateGuitarHandler(GuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<Unit> Handle(UpdateGuitar request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar.FindAsync(request.Id);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(Guitar), request.Id);
            }

            guitar.Make = request.Make;
            guitar.Model = request.Model;
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}