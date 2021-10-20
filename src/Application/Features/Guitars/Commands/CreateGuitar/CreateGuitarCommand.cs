using Application.Data;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Features.Guitars.Commands.CreateGuitar
{
    public class CreateGuitarCommand : IRequest<int>
    {
        public GuitarType GuitarType { get; set; }

        public int MaxNumberOfStrings { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }
    }

    public class CreateGuitarCommandHandler : IRequestHandler<CreateGuitarCommand, int>
    {
        private readonly GuitarsContext _guitarContext;

        public CreateGuitarCommandHandler(GuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<int> Handle(CreateGuitarCommand request, CancellationToken cancellationToken)
        {
            var guitar = new Guitar(request.GuitarType, request.MaxNumberOfStrings, request.Make, request.Model);

            await _guitarContext.Guitar.AddAsync(guitar, cancellationToken);
            await _guitarContext.SaveChangesAsync(cancellationToken);

            return guitar.Id;
        }
    }
}