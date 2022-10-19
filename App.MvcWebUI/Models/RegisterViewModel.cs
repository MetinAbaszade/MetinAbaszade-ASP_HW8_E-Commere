using App.MvcWebUI.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.MvcWebUI.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }


        public IList<UserRolesViewModel> UserRoles { get; set; }
    }
}
