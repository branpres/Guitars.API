using Guitars.API.Endpoints;
using Application;
using Application.Authentication;
using Application.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Guitars.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {                    
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }
            },
            new List<string>()
        }
    });
});

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

// configure dependency injection
builder.Services.AddApplication();
builder.Services.AddData(configuration);
builder.Services.AddAuthentication(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guitars.API v1"));
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseExceptionHandler("/errors");

app.MapAuthenticationEndpoints();
app.MapGuitarsEndpoints();
app.MapErrorEndpoints();

app.Services.ApplyMigrations();

app.Run();

// makes this accessible to WebApplicationFactory
public partial class Program { }