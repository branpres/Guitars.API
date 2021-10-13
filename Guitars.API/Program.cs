using Guitars.API.Endpoints;
using Guitars.Application;
using Guitars.Infrastructure;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Guitars.API", Version = "v1" });
});

// configure dependency injection for projects
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guitars.API v1"));
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/errors");

app.MapErrorEndpoints();

app.Run();
