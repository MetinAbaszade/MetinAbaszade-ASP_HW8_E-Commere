using App.Business.Abstract;
using App.Entities.Concrete;
using App.MvcWebUI.Entities;
using App.MvcWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.MvcWebUI.Controllers
{
   
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IHttpContextAccessor _httpContextAccessor;
        private UserManager<CustomIdentityUser> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, IHttpContextAccessor httpContextAccessor, UserManager<CustomIdentityUser> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = new ProductListViewModel
            {
                Products = _productService.GetAll()
            };

            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role == "Editor")
                model.IsAdmin = false;
            else model.IsAdmin = true;

            return View(model);
        }


        public IActionResult AddProduct()
        {
            var model = new AddProductViewModel()
            {
                Product = new Product(),
                Categories = _categoryService.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.Add(product);
                TempData.Add("message", "Product was added successfully");
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult UpdateProduct(int productId)
        {
            var product = _productService.GetById(productId);
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            _productService.Update(product);
            return RedirectToAction("Index");
        }


        public IActionResult DeleteProduct(int productId)
        {
            var product = _productService.GetById(productId);
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProductPost(int productId)
        {
            _productService.Delete(productId);
            return RedirectToAction("index");
        }
    }
}
