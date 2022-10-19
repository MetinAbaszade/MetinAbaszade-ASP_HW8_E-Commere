using App.MvcWebUI.Entities;
using App.MvcWebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using System.Web.Mvc;
using System.Security.Claims;
using RedirectResult = Microsoft.AspNetCore.Mvc.RedirectResult;

namespace App.MvcWebUI.Filters
{
    public class DynamicAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly CustomIdentityDbContext _dbContext;

        public DynamicAuthorizationFilter(CustomIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var actionMethodInfo = controllerActionDescriptor.MethodInfo;
            var actionId = GetActionId(context);
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = context.HttpContext.User.Identity.Name;


            var hasAuthAttribute = actionMethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(actionMethodInfo.GetCustomAttributes(true))
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
            .Any();

            if (!hasAuthAttribute)
                return;


            if (String.IsNullOrEmpty(userName))
            {
                return;
            }


            var roles = await (
                 from user in _dbContext.Users
                 join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                 join role in _dbContext.Roles on userRole.RoleId equals role.Id
                 join roleClaim in _dbContext.RoleClaims on role.Id equals roleClaim.RoleId
                 where user.Id == userId
                 select roleClaim).ToListAsync();


            foreach (var role in roles)
            {
                if (role.ClaimValue == actionId)
                    return;
            }
            context.Result = new RedirectResult("/Account/Accessdenied/");
        }

        private string GetActionId(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;
            var controller = controllerActionDescriptor.ControllerName;
            var action = controllerActionDescriptor.ActionName;

            return $"{area}:{controller}:{action}";
        }
    }
}
