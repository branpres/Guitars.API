using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Guitars.Queries.ReadGuitarsFiltered
{
    public class ReadGuitarsFilteredQuery : IRequest<GuitarsVM>
    {
        public ReadGuitarsFilteredQuery(int? guitarType, int? maxNumberOfStrings, string make, string model)
        {
            GuitarType = guitarType;
            MaxNumberOfStrings = maxNumberOfStrings;
            Make = make;
            Model = model;
        }

        public int? GuitarType { get; private set; }

        public int? MaxNumberOfStrings { get; private set; }

        public string Make { get; private set; }

        public string Model { get; private set; }
    }

    public class ReadGuitarsFilteredQueryHandler : IRequestHandler<ReadGuitarsFilteredQuery, GuitarsVM>
    {
        private readonly IGuitarsContext _guitarContext;

        public ReadGuitarsFilteredQueryHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public async Task<GuitarsVM> Handle(ReadGuitarsFilteredQuery request, CancellationToken cancellationToken)
        {
            // just an expression to get us started
            Expression<Func<Guitar, bool>> expression = x => x.Id != 0;

            if (request.GuitarType != null)
            {
                expression = expression.And(x => (int)x.GuitarType == request.GuitarType);
            }

            if (request.MaxNumberOfStrings != null)
            {
                expression = expression.And(x => x.MaxNumberOfStrings == request.MaxNumberOfStrings);
            }

            if (string.IsNullOrEmpty(request.Make))
            {
                expression = expression.And(x => x.Make == request.Make);
            }

            if (string.IsNullOrEmpty(request.Model))
            {
                expression = expression.And(x => x.Model == request.Model);
            }

            return new GuitarsVM
            {
                Guitars = await _guitarContext.Guitar
                    .Where(expression)
                    .Include(x => x.GuitarStrings)
                    .Select(x => x.MapToDto())
                    .ToListAsync(cancellationToken)
            };
        }
    }
}