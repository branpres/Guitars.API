namespace Application.Features.Guitars.Queries.ReadGuitars;

public class ReadGuitarsQuery : IRequest<GuitarsVM>
{
    public ReadGuitarsQuery(string? filter = null, int? pageIndex = null, int? pageSize = null)
    {
        Filter = filter;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public string? Filter { get; private set; }

    public int? PageIndex { get; private set; }

    public int? PageSize { get; private set; }
}

public class ReadGuitarsQueryHandler : IRequestHandler<ReadGuitarsQuery, GuitarsVM>
{
    private readonly GuitarsContext _guitarContext;

    public ReadGuitarsQueryHandler(GuitarsContext guitarContext)
    {
        _guitarContext = guitarContext;
    }

    public async Task<GuitarsVM> Handle(ReadGuitarsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Guitar, bool>> expression = x => !x.IsDeleted;
        if (!string.IsNullOrEmpty(request.Filter))
        {
            expression = expression.And(x =>
                x.GuitarType.ToString().Contains(request.Filter)
                || x.MaxNumberOfStrings.ToString().Contains(request.Filter)
                || x.Make.Contains(request.Filter)
                || x.Model.Contains(request.Filter));
        }

        var queryable = _guitarContext.Guitar.Where(expression);

        if (request.PageIndex != null && request.PageSize != null)
        {
            queryable = queryable.Skip(((int)request.PageIndex - 1) * (int)request.PageSize).Take((int)request.PageSize);
        }

        return new GuitarsVM
        {
            Guitars = await queryable
                .Include(x => x.GuitarStrings)
                .Select(x => x.MapToDto())
                .ToListAsync(cancellationToken)
        };
    }
}