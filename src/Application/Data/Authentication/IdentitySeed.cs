using Application.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Application.Data.Authentication
{
    public static class IdentitySeed
    {
        public static void Seed(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRoleAsync(roleManager, Constants.Roles.ADMIN).GetAwaiter().GetResult();
            SeedRoleAsync(roleManager, Constants.Roles.READ_ONLY_USER).GetAwaiter().GetResult();

            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.REFRESH_TOKEN).GetAwaiter().GetResult();
            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.CREATE_GUITAR).GetAwaiter().GetResult();
            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.READ_GUITAR).GetAwaiter().GetResult();
            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.UPDATE_GUITAR).GetAwaiter().GetResult();
            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.DELETE_GUITAR).GetAwaiter().GetResult();
            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.STRING_GUITAR).GetAwaiter().GetResult();
            SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.TUNE_GUITAR).GetAwaiter().GetResult();

            SeedClaimAsync(roleManager, Constants.Roles.READ_ONLY_USER, Constants.Claims.READ_GUITAR).GetAwaiter().GetResult();

            SeedDefaultUsersAsync(userManager).GetAwaiter().GetResult();
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task SeedClaimAsync(RoleManager<IdentityRole> roleManager, string roleName, string claimValue)
        {
            var identityRole = roleManager.Roles.FirstOrDefault(x => x.Name == roleName);
            if (identityRole != null)
            {
                var claims = await roleManager.GetClaimsAsync(identityRole);
                var claim = claims.FirstOrDefault(x => x.Value == claimValue);
                if (claim == null)
                {
                    await roleManager.AddClaimAsync(identityRole, new Claim(Constants.ClaimTypes.HAS_PERMISSION, claimValue));
                }
            }
        }

        private static async Task SeedDefaultUsersAsync(UserManager<IdentityUser> userManager)
        {
            var user = userManager.Users.FirstOrDefault(x => x.UserName == "admin");
            if (user == null)
            {
                var identityUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@guitars.com"
                };
                var result = await userManager.CreateAsync(identityUser, "guitarsAdmin1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, Constants.Roles.ADMIN);
                }
            }

            user = userManager.Users.FirstOrDefault(x => x.UserName == "readonlyuser");
            if (user == null)
            {
                var identityUser = new IdentityUser
                {
                    UserName = "readonlyuser",
                    Email = "readonlyuser@guitars.com"
                };
                var result = await userManager.CreateAsync(identityUser, "guitarsReadonlyuser1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityUser, Constants.Roles.READ_ONLY_USER);
                }
            }
        }
    }
}