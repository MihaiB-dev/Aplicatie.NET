using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.NET.Controllers
{
    public class ProjectsController : Controller
    {
        //adaugam user si roluri 
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext db;
        public ProjectsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "User, Editor, Admin")]

        public IActionResult Index(int id)
        {

            var projects = db.Projects.Where(p => p.TeamId == id);
            ViewBag.team_id = id;
            // ViewBag.Projects = projects;

            return View(projects);
        }

        public IActionResult Show(int id)
        {
            var project =  db.Projects.Include("Task").Where(p => p.Id == id);
                          
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public IActionResult New()
        {
            // ViewBag.Teams = new SelectList(db.Teams, "Id", "Name");

            Project newProject = new Project();
            
            return View(newProject);
        }



        [HttpPost]
        //team se ia din link, project se ia din post
        public IActionResult New(Project project)
        {
            try
            {
                //TODO trebuie sa verficam daca persoana se afla in echipa respectiva

                //preiua id-ul de la echipa din care a apasat click. Il ia din view->show de la project (link)
                project.TeamId =  Convert.ToInt32(HttpContext.Request.Query["team"]);

                // project.Tasks = new List<Task_table>();

                project.UsersId = _userManager.GetUserId(User);

                db.Projects.Add(project);
                db.SaveChanges();

                TempData["message"] = "Proiectul a fost adăugat!";
                return RedirectToAction("Index", new { id = project.TeamId});
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Eroare la adăugarea proiectului: {ex.Message}";
                return RedirectToAction("New");
            }
        }



        public IActionResult Edit(int id)
        {
            Project project = db.Projects.FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }


        [HttpPost]
        public IActionResult Edit(int id, Project requestProject)
        {
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                project.Title_project = requestProject.Title_project;
                project.Description = requestProject.Description;
                db.SaveChanges();
                TempData["message"] = "Proiectul a fost modificat!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestProject);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            TempData["message"] = "Proiectul a fost sters!";
            return RedirectToAction("Index");
        }
    }
}
