using Microsoft.AspNetCore.Mvc;

namespace App.NET.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
