using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Guitars.Queries.ReadGuitarsFiltered
{
    public class ReadGuitarsFilterQuery : IRequest<GuitarsVM>
    {
        public int GuitarType { get; set; }

        public int MaxNumberOfStrings { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }
    }

    public class ReadGuitarsFilterQueryHandler : IRequestHandler<ReadGuitarsFilterQuery, GuitarsVM>
    {
        private readonly IGuitarsContext _guitarContext;

        public ReadGuitarsFilterQueryHandler(IGuitarsContext guitarContext)
        {
            _guitarContext = guitarContext;
        }

        public Task<GuitarsVM> Handle(ReadGuitarsFilterQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}