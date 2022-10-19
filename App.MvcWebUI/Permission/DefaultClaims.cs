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
                     Type = "Permissions",
                     Value = "Permissions.Admin.AddProduct",
                },
                new RoleClaimsViewModel()
                {
                    Type = "Permissions",
                    Value = "Permissions.Admin.DeleteProduct",
                },
                new RoleClaimsViewModel()
                {
                    Type = "Permissions",
                    Value = "Permissions.Admin.UpdateProduct",
                },
                new RoleClaimsViewModel()
                {
                   Type = "Permissions",
                   Value = "Permissions.Account.RegisterAccount",
                },
                 new RoleClaimsViewModel()
                {
                   Type = "Controller",
                   Value = ":Admin:Index",
                },
                
            };
    }
}
