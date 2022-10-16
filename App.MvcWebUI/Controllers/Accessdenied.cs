using Microsoft.AspNetCore.Mvc;

namespace App.MvcWebUI.Controllers
{
    public class Accessdenied : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
