namespace Application.Features.Guitars.Commands.DeleteGuitar;

public class DeleteGuitarCommand : IRequest
{
    public DeleteGuitarCommand(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }
}

public class DeleteGuitarCommandHandler : IRequestHandler<DeleteGuitarCommand>
{
    private readonly GuitarsContext _guitarContext;

    public DeleteGuitarCommandHandler(GuitarsContext guitarContext)
    {
        _guitarContext = guitarContext;
    }

    public async Task<Unit> Handle(DeleteGuitarCommand request, CancellationToken cancellationToken)
    {
        var guitar = await _guitarContext.Guitar
            .Include(x => x.GuitarStrings)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        if (guitar == null)
        {
            throw new NotFoundException(nameof(Guitar), request.Id);
        }

        guitar.IsDeleted = true;
        guitar.GuitarStrings.ForEach(x => x.IsDeleted = true);
        await _guitarContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}