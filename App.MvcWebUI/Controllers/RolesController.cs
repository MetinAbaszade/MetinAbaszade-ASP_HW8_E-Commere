using App.MvcWebUI.Entities;
using App.MvcWebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace App.MvcWebUI.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly CustomIdentityDbContext _dbContext;
        private readonly IMvcControllerDiscovery _mvcControllerDiscovery;

        public RolesController(RoleManager<CustomIdentityRole> roleManager, CustomIdentityDbContext dbContext, IMvcControllerDiscovery mvcControllerDiscovery)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
            _mvcControllerDiscovery = mvcControllerDiscovery;
        }

        public async Task<IActionResult> Index()
        {
            // var test = _mvcControllerDiscovery.GetControllers();
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new CustomIdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = _dbContext.Roles.Where(r => r.Id == roleId).FirstOrDefault();
            if (!String.IsNullOrEmpty(role.Name))
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
