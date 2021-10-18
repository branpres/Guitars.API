using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IGuitarsContext
    {
        DbSet<Guitar> Guitar { get; set; }

        DbSet<GuitarString> GuitarString { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}