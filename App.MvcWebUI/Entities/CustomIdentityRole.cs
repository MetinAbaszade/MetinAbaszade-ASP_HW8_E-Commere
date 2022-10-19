using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.MvcWebUI.Entities
{
    [Keyless]
    public class CustomIdentityRole: IdentityRole
    {
        //custom

        public CustomIdentityRole(string name)
        {
            this.Name = name;
        }
        [NotMapped]
        public List<string>? Access { get; set; } = new List<string>() { new string("Mytest") }; 
    }
}
