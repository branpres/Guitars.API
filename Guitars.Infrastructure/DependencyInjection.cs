using Guitars.Application.Interfaces;
using Guitars.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guitars.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["RepositoryImplementation"] == "EF")
            {
                services.AddDbContext<GuitarsContext>(options => options.UseMySQL(configuration.GetConnectionString("Guitars")));
                services.AddScoped<DbContext, GuitarsContext>();
                services.AddScoped<IGuitarRepository, EFGuitarRepository>();
            }
            else
            {
                services.AddScoped<IGuitarRepository, InMemoryGuitarRepository>();
            }
        }
    }
}