using App.MvcWebUI.Entities;
using App.MvcWebUI.Models;
using App.MvcWebUI.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace App.MvcWebUI.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly CustomIdentityDbContext _dbContext;

        public PermissionController(RoleManager<CustomIdentityRole> roleManager, CustomIdentityDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<ActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();

            var allPermissions = DefaultClaims.DefaultClaimsList;


            // Role - u tapiriq
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;

            var roleClaimValues = await (
               from roleclaims in _dbContext.RoleClaims
               where roleclaims.RoleId == model.RoleId
               select roleclaims).ToListAsync();

            var allRoleClaimsViewModel = new List<RoleClaimsViewModel>();


            foreach (var item in DefaultClaims.DefaultClaimsList)
            {
                var newRoleClaimsViewModel = new RoleClaimsViewModel()
                {
                    Value = item.Value,
                    Type = item.Type,
                    Selected = false
                };

                if (roleClaimValues.Any(a => a.ClaimValue == item.Value))
                {
                    newRoleClaimsViewModel.Selected = true;
                }
                allRoleClaimsViewModel.Add(newRoleClaimsViewModel);
            }
            model.RoleClaims = allRoleClaimsViewModel;
            return View(model);
        }

        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
            }
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}
