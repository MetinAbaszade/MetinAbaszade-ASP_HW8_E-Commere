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
            var userName = context.HttpContext.User.Identity.Name;
            return;

            if (String.IsNullOrEmpty(userName))
            {
                //context.Result = new ChallengeResult("/Account/Login");
                //context.Result = new
                //RedirectResult("/Account/Login");
                return;
            }


            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated; 
            if (IsAuthenticated)
            {
                return;
            }

            var actionId = GetActionId(context); 



            var roles = await (
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                join roleClaim in _dbContext.RoleClaims on role.Id equals roleClaim.RoleId
                where user.UserName == userName
                select roleClaim.ClaimValue).ToListAsync();


            foreach (var role in roles)
            {
                if (role == actionId)
                    return;
            }

            context.Result = new UnauthorizedResult();
            // context.Result = new ForbidResult();
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
