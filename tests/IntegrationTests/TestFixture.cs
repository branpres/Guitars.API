namespace IntegrationTests;

[SetUpFixture]
public class TestFixture
{
    private static IConfiguration _configuration;
    private static Checkpoint _checkpoint;

    [OneTimeSetUp]
    public void RunBeforeTests()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false).Build();

        _checkpoint = new MySqlCheckpoint
        {
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = new[] { "__EFMigrationsHistory" },
            SchemasToInclude = new[] { "guitarsTest" }
        };

        // connect to test database
        var services = new ServiceCollection();
        services.AddData(_configuration);
        services.BuildServiceProvider().ApplyMigrations();
    }

    public static async Task ResetCheckpoint()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("Guitars"));
    }
}

/// <summary>
/// Overrides the Reset method as otherwise we get a "Keyword not supported: 'port'" error when trying to create the checkpoint.
/// </summary>
public class MySqlCheckpoint : Checkpoint
{
    public override async Task Reset(string connectionString)
    {
        using var mySqlConnection = new MySqlConnection(connectionString);
        await mySqlConnection.OpenAsync();
        await base.Reset(mySqlConnection);
    }
}