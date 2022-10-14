using App.MvcWebUI.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace App.MvcWebUI.Permission
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<CustomIdentityUser> userManager,
            RoleManager<CustomIdentityRole> roleManager)
        {
            // Seed Basic User
            var defaultUser = new CustomIdentityUser
            {
                UserName = "User123",
                Email = "defaultuser@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "User_123");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");
                }
            }
        }

        public static async Task SeedSuperAdminUserAsync(UserManager<CustomIdentityUser> userManager,
            RoleManager<CustomIdentityRole> roleManager)
        {
            // Seed SuperAdmin User
            var defaultUser = new CustomIdentityUser
            {
                UserName = "SuperAdmin",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "SuperAdmin_123");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        private static async Task SeedClaimsForSuperAdmin(this RoleManager<CustomIdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");
            await roleManager.AddPermissionClaim(adminRole, "Products");
        }

        public static async Task AddPermissionClaim(this RoleManager<CustomIdentityRole> roleManager, CustomIdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
