using Application;
using Application.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests
{
    class GuitarsApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

            builder.ConfigureServices(services =>
            {
                services.AddApplication();
                services.AddData(configuration);
            });

            return base.CreateHost(builder);
        }
    }
}