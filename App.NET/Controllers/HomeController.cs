using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {   
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Teams", new { area = "" });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Panel()
        {
            if (User.IsInRole("Admin"))
            {
                //in acest panel Adminul poate sa dea kick la useri
                var all_users = _db.Users;

                return View(all_users);
            }
            else
            {
                return RedirectToAction("Index", "Teams");
            }
        }

        [HttpPost]
        public IActionResult Panel(string User_Id)
        {
            var user = _db.Users.Find(User_Id);
            //il scoatem de la toate proiectele la care este organizator
            var Projects = _db.Projects.Where(p => p.Users_Id == User_Id);
            foreach(var project in Projects)
            {
                project.Users_Id = null;
                
            }
            _db.SaveChanges();
            //stergem utilizatorul de tot
            _db.Users.Remove(user);
            _db.SaveChanges();

            return RedirectToAction("Panel", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}