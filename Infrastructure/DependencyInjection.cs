using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<GuitarsContext>(options => options.UseInMemoryDatabase("Guitars"));
            }
            else
            {
                services.AddDbContext<GuitarsContext>(options => options.UseMySQL(configuration.GetConnectionString("Guitars")));
            }
                        
            services.AddScoped<IGuitarsContext>(provider => provider.GetService<GuitarsContext>());
        }
    }
}