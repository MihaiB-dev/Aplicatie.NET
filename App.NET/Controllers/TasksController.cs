using Microsoft.AspNetCore.Mvc;

namespace App.NET.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
