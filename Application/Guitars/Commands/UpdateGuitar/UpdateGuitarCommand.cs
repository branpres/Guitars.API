using Application.Interfaces;
using MediatR;

namespace Application.Guitars.Commands.UpdateGuitar
{
    public class UpdateGuitarCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }
    }

    public class UpdateGuitarCommandHandler : IRequestHandler<UpdateGuitarCommand, int>
    {
        private readonly IGuitarsContext _guitarContext;

        public UpdateGuitarCommandHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<int> Handle(UpdateGuitarCommand request, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar.FindAsync(request.Id);
            if (guitar != null)
            {
                guitar.Make = request.Make;
                guitar.Model = request.Model;
                return await _guitarContext.SaveChangesAsync(cancellationToken);
            }

            return 0;
        }
    }
}