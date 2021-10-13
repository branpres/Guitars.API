using Guitars.Application.Interfaces;
using Guitars.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Guitars.Infrastructure.Persistence
{
    public class GuitarsContext : DbContext, IGuitarsContext
    {
        public GuitarsContext() { }

        public GuitarsContext(DbContextOptions<GuitarsContext> options) : base(options) { }

        public virtual DbSet<Guitar> Guitar { get; set; }

        public virtual DbSet<GuitarString> GuitarString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // don't like this, but not sure how to get around it atm
            optionsBuilder.UseMySQL("Server=127.0.0.1;Database=guitars;User Id=testuser;Password=test123;port=3306");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}