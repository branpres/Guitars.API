using Guitars.API.Endpoints;
using Application;
using Application.Data;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("IntegrationTests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Guitars.API", Version = "v1" });
});

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

// configure dependency injection
builder.Services.AddApplication();
builder.Services.AddData(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guitars.API v1"));
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/errors");

app.MapGuitarEndpoints();
app.MapErrorEndpoints();

app.Services.ApplyMigrations();

app.Run();

public partial class Program { }