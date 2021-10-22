using Application.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Respawn;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [SetUpFixture]
    public class TestsSetup
    {
        private static IConfiguration _configuration;
        private static MySqlCheckpoint _checkpoint;

        [OneTimeSetUp]
        public void RunBeforeTests()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false).Build();

            _checkpoint = new MySqlCheckpoint
            {
                DbAdapter = DbAdapter.MySql,
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            var services = new ServiceCollection();
            services.AddData(_configuration);
            services.BuildServiceProvider().ApplyMigrations();
        }

        public static async Task ResetCheckpoint()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("Guitars"));
        }
    }

    public class MySqlCheckpoint : Checkpoint
    {
        public override async Task Reset(string connectionString)
        {
            using var mySqlConnection = new MySqlConnection(connectionString);
            await mySqlConnection.OpenAsync();
            await base.Reset(mySqlConnection);
        }
    }
}