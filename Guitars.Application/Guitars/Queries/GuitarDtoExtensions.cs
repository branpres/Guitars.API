using Guitars.Domain.Models;

namespace Guitars.Application.Guitars.Queries
{
    public static class GuitarDtoExtensions
    {
        public static GuitarDto MapToDto(this Guitar guitar)
        {
            return new GuitarDto
            {
                Id = guitar.Id,
                GuitarType = (int)guitar.GuitarType,
                MaxNumberOfStrings = guitar.MaxNumberOfStrings,
                Make = guitar.Make,
                Model = guitar.Model,
                Created = guitar.Created,
                Updated = guitar.Updated,
                GuitarStrings = guitar.GuitarStrings
                    .Select(x => new GuitarStringDto
                    {
                        Id = x.Id,
                        Number = x.Number,
                        Gauge = x.Gauge,
                        Tuning = x.Tuning,
                        Created = x.Created,
                        Updated = x.Updated
                    }).ToList()
            };
        }
    }
}