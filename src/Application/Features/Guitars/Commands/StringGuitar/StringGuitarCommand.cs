namespace Application.Features.Guitars.Commands.StringGuitar;

public class StringGuitarCommand : IRequest
{
    public StringGuitarCommand(int id, List<StringDto> strings)
    {
        Id = id;
        Strings = strings;
    }

    public int Id { get; private set; }

    public List<StringDto> Strings { get; private set; }
}

public class StringGuitarCommandHandler : IRequestHandler<StringGuitarCommand>
{
    private readonly GuitarsContext _guitarContext;

    public StringGuitarCommandHandler(GuitarsContext guitarContext)
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
            throw new NotFoundException(nameof(Guitar), request.Id);
        }

        request.Strings.ForEach(x => guitar.String(x.Number, x.Gauge, x.Tuning));
        await _guitarContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}