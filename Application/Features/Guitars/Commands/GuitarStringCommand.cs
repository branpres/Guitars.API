using MediatR;

namespace Application.Features.Guitars.Commands
{
    public abstract class GuitarStringCommand : IRequest
    {
        public GuitarStringCommand(int id, List<GuitarStringDto> guitarStrings)
        {
            Id = id;
            GuitarStrings = guitarStrings;
        }

        public int Id { get; private set; }

        public List<GuitarStringDto> GuitarStrings { get; private set; }
    }
}