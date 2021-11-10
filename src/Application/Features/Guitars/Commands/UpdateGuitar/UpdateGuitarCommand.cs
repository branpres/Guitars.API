namespace Application.Features.Guitars.Commands.UpdateGuitar;

public class UpdateGuitarCommand : IRequest
{
    public int Id { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }
}

public class UpdateGuitarCommandHandler : IRequestHandler<UpdateGuitarCommand>
{
    private readonly GuitarsContext _guitarContext;

    public UpdateGuitarCommandHandler(GuitarsContext guitarContext)
    {
        _guitarContext = guitarContext;
    }

    public async Task<Unit> Handle(UpdateGuitarCommand request, CancellationToken cancellationToken)
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