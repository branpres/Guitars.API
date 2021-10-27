using Application.Data.Authentication;
using Domain.Common;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Application.Data
{
    public class GuitarsContext : IdentityDbContext
    {
        public GuitarsContext() { }

        public GuitarsContext(DbContextOptions<GuitarsContext> options) : base(options) { }

        public virtual DbSet<Guitar> Guitar { get; set; }

        public virtual DbSet<GuitarString> GuitarString { get; set; }

        public virtual DbSet<AuthToken> AuthToken { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            optionsBuilder.UseMySQL(configuration.GetConnectionString("Guitars"));
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