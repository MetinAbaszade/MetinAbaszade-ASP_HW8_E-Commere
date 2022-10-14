using Microsoft.AspNetCore.Identity;

namespace App.MvcWebUI.Entities
{
    public class CustomIdentityRole: IdentityRole
    {
        //custom
       public CustomIdentityRole(string Name)
        {
            this.Name = Name;
        }
    }
}
