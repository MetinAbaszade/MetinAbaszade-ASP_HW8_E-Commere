using App.MvcWebUI.Entities;
using App.MvcWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace App.MvcWebUI.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;

        public AccountController(UserManager<CustomIdentityUser> userManager,
            RoleManager<CustomIdentityRole> roleManager,
            SignInManager<CustomIdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(this.User))
            {
                return RedirectToAction(actionName: "Index", controllerName: "Admin");
            }
            else return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password, loginViewModel.RememberMe, false).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError("", "Invalid Login");
            }

            return View(loginViewModel);
        }


        public IActionResult LoggOff()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }



        [Authorize(Roles = "Admin")]
        public IActionResult RegisterEditor()
        {
            return View();
        }


        public IActionResult AddUser()
        {
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRole = new UserRolesViewModel
                {
                    RoleName = role.Name,
                    Selected = false
                };
                userRoles.Add(userRole);
            }

            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                UserRoles = userRoles
            };

            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                CustomIdentityUser user = new CustomIdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                };
                // hemen identity(user,role,policy ola biler) yaranmasiyla bagli melumat verir
                IdentityResult result = _userManager.CreateAsync(user, registerViewModel.Password).Result;

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("Admin").Result)
                    {
                        CustomIdentityRole role = new CustomIdentityRole("Admin");

                        IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "We can not add the role!");
                            return View(registerViewModel);
                        }
                    }
                    foreach (var item in registerViewModel.UserRoles)
                    {
                        if (item.Selected)
                        {
                            _userManager.AddToRoleAsync(user, item.RoleName).Wait();
                        }
                    }
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(registerViewModel);
        }


        public IActionResult Accessdenied()
        {
            return View();
        }

    }
}
