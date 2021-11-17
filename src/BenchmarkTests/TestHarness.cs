using BenchmarkDotNet.Attributes;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BenchmarkTests;

public class TestHarness
{
    [Params(100, 200)]
    public int Count;

    [Benchmark]
    public async Task ControllerTestAsync()
    {
        await GetAsync("https://localhost:7219");
    }
    [Benchmark]
    public async Task MinimalApiAsync()
    {
        await GetAsync("https://localhost:7190");
    }    

    private async Task GetAsync(string baseAddress)
    {
        var jwt = await GetJwtAsync();

        for (int i = 0; i < Count; i++)
        {
            var client = new HttpClient() { BaseAddress = new Uri(baseAddress) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            await client.GetAsync("/guitars", new CancellationToken());
        }
    }

    private static async Task<string> GetJwtAsync()
    {
        var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7190") };
        var response = client.PostAsJsonAsync("/authentication/login", new { UserName = "admin", Password = "guitarsAdmin1!" }).GetAwaiter().GetResult();
        return await response.Content.ReadFromJsonAsync<string>();
    }
}