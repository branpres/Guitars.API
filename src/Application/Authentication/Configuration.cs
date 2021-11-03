using Application.Authentication.Behaviors;
using Application.Data;
using Application.Data.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Authentication
{
    public static class Configuration
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = configuration.GetSection("JwtConfiguration");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfiguration["Issuer"],
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration["Key"])),
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParameters);
            services.AddScoped<TokenGenerator>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<GuitarsContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.ClaimsIssuer = jwtConfiguration["Issuer"];
                jwt.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.Policies.WRITE, policy =>
                {
                    policy.RequireClaim(
                        Constants.ClaimTypes.HAS_PERMISSION,
                        Constants.Claims.REFRESH_TOKEN,
                        Constants.Claims.CREATE_GUITAR,
                        Constants.Claims.UPDATE_GUITAR,
                        Constants.Claims.DELETE_GUITAR,
                        Constants.Claims.STRING_GUITAR,
                        Constants.Claims.TUNE_GUITAR);
                });
                options.AddPolicy(Constants.Policies.READ, policy =>
                {
                    policy.RequireClaim(Constants.ClaimTypes.HAS_PERMISSION, Constants.Claims.READ_GUITAR);
                });
            });

            services.AddHttpContextAccessor();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehavior<,>));

            var serviceProvider = services.BuildServiceProvider();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityDbContextSeed.SeedAsync(userManager, roleManager).GetAwaiter().GetResult();
        }
    }
}