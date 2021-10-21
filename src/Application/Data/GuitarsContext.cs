using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Application.Data
{
    public class GuitarsContext : DbContext
    {
        private readonly IConfigurationRoot _configuration;

        public GuitarsContext() { }

        public GuitarsContext(DbContextOptions<GuitarsContext> options, IConfigurationRoot configuration) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Guitar> Guitar { get; set; }

        public virtual DbSet<GuitarString> GuitarString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // don't like this, but not sure how to get around it atm
            //optionsBuilder.UseMySQL("Server=127.0.0.1;Database=guitars;Trusted_Connection=True;port=3306");
            
            optionsBuilder.UseMySQL(_configuration.GetConnectionString("Guitars"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<ModelBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.Updated = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}