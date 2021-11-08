using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using System.Net.Http.Headers;
using System.Net.Http.Json;

// before runing, get the JWT required
var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7190/guitars") };

var response = await client.PostAsJsonAsync("/authentication/login", new { UserName = "admin", Password = "guitarsAdmin1!" });
var jwt = await response.Content.ReadFromJsonAsync<string>();

var httpFactory = HttpClientFactory.Create();

var stepController = CreateStep("controller", httpFactory, "https://localhost:7219/guitars");
var stepMinimalAPI = CreateStep("minimalAPI", httpFactory, "https://localhost:7190/guitars");

var scenarioController = CreateScenario("controller", stepController, 10, TimeSpan.FromSeconds(60));
var scenarioMinimalAPI = CreateScenario("minimalAPI", stepMinimalAPI, 10, TimeSpan.FromSeconds(60));

NBomberRunner
    .RegisterScenarios(scenarioController, scenarioMinimalAPI)
    .Run();

IStep CreateStep(string stepName, IClientFactory<HttpClient> httpClientFactory, string endpoint) =>
    Step.Create(stepName, httpClientFactory, async context =>
    {
        var client = context.Client;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await context.Client.GetAsync(endpoint, context.CancellationToken);

        return response.IsSuccessStatusCode
            ? Response.Ok(statusCode: (int)response.StatusCode)
            : Response.Fail(statusCode: (int)response.StatusCode);
    });

Scenario CreateScenario(string scenarioName, IStep step, int copies, TimeSpan duration) =>
    ScenarioBuilder
        .CreateScenario(scenarioName, step)
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(Simulation.KeepConstant(copies, duration));
