using Guitars.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Guitars.Application.Interfaces
{
    public interface IGuitarsContext
    {
        DbSet<Guitar> Guitar { get; set; }

        DbSet<GuitarString> GuitarString { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}