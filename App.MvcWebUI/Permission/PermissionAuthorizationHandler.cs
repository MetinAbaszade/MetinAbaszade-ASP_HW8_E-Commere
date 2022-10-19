using App.MvcWebUI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace App.MvcWebUI.Permission
{

    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly RoleManager<CustomIdentityRole> _roleManager;

        public PermissionAuthorizationHandler(RoleManager<CustomIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
       {
            if (context.User == null)
            {
                return;
            }
            var role = context.User.Claims;
            var userClaims = context.User.Claims;
            bool tf = false;
            foreach (var item in userClaims)
            {
                if (item.Type == "Permission" && item.Value == requirement.Permission)
                {
                    tf = true;
                }
            }
            if (tf)
            {
                context.Succeed(requirement);
                return;
            }
            else
            {
                context.Fail();
                return;
            }
        }
    }
}
