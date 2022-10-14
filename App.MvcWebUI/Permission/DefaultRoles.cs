using App.MvcWebUI.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace App.MvcWebUI.Permission
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<CustomIdentityUser> userManager,
            RoleManager<CustomIdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new CustomIdentityRole("Admin"));
            await roleManager.CreateAsync(new CustomIdentityRole("Manager"));
            await roleManager.CreateAsync(new CustomIdentityRole("Basic"));
        }
    }
}
