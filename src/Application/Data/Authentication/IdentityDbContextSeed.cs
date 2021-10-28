using Application.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Data.Authentication
{
    public static class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoleAsync(roleManager, Constants.Roles.ADMIN);
            await SeedRoleAsync(roleManager, Constants.Roles.READ_ONLY_USER);

            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.REFRESH_TOKEN);
            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.CREATE_GUITAR);
            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.READ_GUITAR);
            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.UPDATE_GUITAR);
            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.DELETE_GUITAR);
            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.STRING_GUITAR);
            await SeedClaimAsync(roleManager, Constants.Roles.ADMIN, Constants.Claims.TUNE_GUITAR);

            await SeedClaimAsync(roleManager, Constants.Roles.READ_ONLY_USER, Constants.Claims.READ_GUITAR);

            await SeedDefaultUsersAsync(userManager);
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