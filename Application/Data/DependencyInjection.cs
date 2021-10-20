using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Data
{
    public static class DependencyInjection
    {
        public static void AddData(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<GuitarsContext>(options => options.UseInMemoryDatabase("Guitars"));
            }
            else
            {
                services.AddDbContext<GuitarsContext>(options => options.UseMySQL(configuration.GetConnectionString("Guitars")));
            }
                        
            services.AddScoped<GuitarsContext>();
        }
    }
}