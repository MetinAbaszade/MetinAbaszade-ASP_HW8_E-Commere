using App.MvcWebUI.Entities;
using App.MvcWebUI.Models;
using App.MvcWebUI.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace App.MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PermissionController : Controller
    {
        private readonly RoleManager<CustomIdentityRole> _roleManager;

        public PermissionController(RoleManager<CustomIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        //public static List<string> GetAllActionNames(string controllerName)
        //{
        //    var test = typeof(Program).Assembly.GetName().Name;
        //    var controllerType = Assembly.Load(typeof(Program).Assembly.GetName().Name)
        //        .GetTypes()
        //        .FirstOrDefault(x => typeof(System.Web.Mvc.IController).IsAssignableFrom(x)
        //            && x.Name.Equals(controllerName + "Controller", StringComparison.OrdinalIgnoreCase));

        //    if (controllerType == null)
        //    {
        //        return Enumerable.Empty<string>().ToList();
        //    }
        //    return new System.Web.Mvc.ReflectedControllerDescriptor(controllerType)
        //        .GetCanonicalActions().Select(x => x.ActionName)
        //        .ToList();
        //}



        public async Task<ActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();

            var allPermissions = new List<RoleClaimsViewModel>()
            { 
                new RoleClaimsViewModel()
                {
                    Type = "Permissions",
                    Value = "Permissions.Products.Create",
                },
                new RoleClaimsViewModel()
                {
                    Type = "Permissions",
                    Value = "Permissions.Products.View",
                }
            };

           
            // Role - u tapiriq
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;


            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
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
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}
