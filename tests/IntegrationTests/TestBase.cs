namespace IntegrationTests;

public abstract class TestBase
{
    [SetUp]
    public async Task ResetCheckpoint()
    {
        await TestFixture.ResetCheckpoint();
    }

    public static async Task<HttpClient> GetHttpClientAsync()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // using admin because it doesn't matter who is logged in as authentication is not what we're testing here
        // although this by nature does, in fact, test logging in as admin
        var loginCommand = new LoginCommand { UserName = "admin", Password = "guitarsAdmin1!" };
        var response = await client.PostAsJsonAsync("/authentication/login", loginCommand);
        var jwt = await response.Content.ReadFromJsonAsync<string>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        return client;
    }

    public static async Task<HttpClient> GetHttpClientForReadOnlyUserAsync()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // using admin because it doesn't matter who is logged in as authentication is not what we're testing here
        // although this by nature does, in fact, test logging in as admin
        var loginCommand = new LoginCommand { UserName = "readonlyuser", Password = "guitarsReadonlyuser1!" };
        var response = await client.PostAsJsonAsync("/authentication/login", loginCommand);
        var jwt = await response.Content.ReadFromJsonAsync<string>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        return client;
    }
}