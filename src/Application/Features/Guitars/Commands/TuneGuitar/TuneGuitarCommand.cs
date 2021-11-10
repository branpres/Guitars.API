namespace Application.Features.Guitars.Commands.TuneGuitar;

public class TuneGuitarCommand : IRequest
{
    public TuneGuitarCommand(int id, List<TuningDto> tunings)
    {
        Id = id;
        Tunings = tunings;
    }

    public int Id { get; private set; }

    public List<TuningDto> Tunings { get; private set; }
}

public class TuneGuitarCommandHandler : IRequestHandler<TuneGuitarCommand>
{
    private readonly GuitarsContext _guitarContext;

    public TuneGuitarCommandHandler(GuitarsContext guitarContext)
    {
        _guitarContext = guitarContext;
    }

    public async Task<Unit> Handle(TuneGuitarCommand request, CancellationToken cancellationToken)
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