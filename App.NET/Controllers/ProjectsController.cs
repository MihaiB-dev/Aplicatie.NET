using App.NET.Data;
using Microsoft.AspNetCore.Mvc;

namespace App.NET.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext db;
        public ProjectsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var projects = db.Projects;
            return View();
        }
    }
}
