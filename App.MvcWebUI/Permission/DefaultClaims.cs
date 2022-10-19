using App.MvcWebUI.Models;
using Newtonsoft.Json.Linq;

namespace App.MvcWebUI.Permission
{
    public class DefaultClaims
    {
        public static List<RoleClaimsViewModel> DefaultClaimsList =
           new List<RoleClaimsViewModel>()
            {
                new RoleClaimsViewModel()
                {
                     Type = "Permission",
                     Value = "Permissions.Admin.AddProduct",
                },
                new RoleClaimsViewModel()
                {
                    Type = "Permission",
                    Value = "Permissions.Admin.DeleteProduct",
                },
                new RoleClaimsViewModel()
                {
                    Type = "Permission",
                    Value = "Permissions.Admin.UpdateProduct",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Permission",
                   Value = "Permissions.Account.RegisterAccount",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Admin:Index",
                },
             

                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Permission:Index",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Permission:Update",
                },

                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Roles:Index",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Roles:AddRole",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Roles:DeleteRole",
                },

                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Users:Index",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Users:GetUserRoles",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Users:UpdateUserRoles",
                },

            };
    }
}
