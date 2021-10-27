using Guitars.API.Endpoints;
using Application;
using Application.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Features.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Guitars.API", Version = "v1" });
    c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
        In = ParameterLocation.Header,
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
});

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

// configure dependency injection
builder.Services.AddAuthentication(configuration);
builder.Services.AddApplication(configuration);
builder.Services.AddData(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guitars.API v1"));
}

app.UseAuthentication();
//app.UseAuthorization();

app.UseHttpsRedirection();

app.UseExceptionHandler("/errors");

app.MapGuitarsEndpoints();
app.MapErrorEndpoints();

app.Services.ApplyMigrations();

app.Run();

// makes this accessible to WebApplicationFactory
public partial class Program { }