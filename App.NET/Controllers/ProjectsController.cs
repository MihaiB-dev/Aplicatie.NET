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

            var projects = db.Projects.Where(p => p.Team_Id == id);
            ViewBag.team_id = id;
            // ViewBag.Projects = projects;

            return View(projects);
        }

        public IActionResult Show(int id)
        {
            var project =  db.Projects.Include("Tasks").Where(p => p.Id == id).First();
                          
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        //trb sa verificam daca acest user este creatorul proiectului
        public IActionResult Add_Users(int id)//id-ul reprezinta id-ul proiectului la care adaugam useri
        {
            var project = db.Projects.Find(id);
            if(project.Users_Id != _userManager.GetUserId(User))
            {
                //return a TempData aswell (TODO)

                return RedirectToAction("Show", "Projects", new { id =id });
            }
            //luam toti utilizatorii care se afla in echipa si care nu se afla deja in proiect;
            var users = db.Users.Where(user => user.Team_member.Any(j => j.Team_id == project.Team_Id) && user.UserProjects.All(j=> j.Project_id != project.Id));
            if(users.Count() == 0) { ViewBag.none =  true; }
            else { ViewBag.none =  false; }
            ViewBag.project_id = id;
            return View(users);
        }

        [HttpPost]
        public IActionResult Add_Users(UserProject userproject)
        {
            db.UserProjects.Add(userproject);
            db.SaveChanges();
            return RedirectToAction("Add_Users", new {id = userproject.Project_id});
        }
        public IActionResult New()
        {
            var team_id = Convert.ToInt32(HttpContext.Request.Query["team"]);
            ViewBag.team = db.Teams.Find(team_id);
            //Project newProject = new Project();
            //newProject
            return View();
        }



        [HttpPost]
        //team se ia din link, project se ia din post
        public IActionResult New(Project project)
        {
            if(ModelState.IsValid)
            {
                //TODO trebuie sa verficam daca persoana se afla in echipa respectiva

                project.Users_Id = _userManager.GetUserId(User);
                db.Projects.Add(project);
                db.SaveChanges();

                db.UserProjects.Add(new UserProject
                {
                    User_id = _userManager.GetUserId(User),
                    Project_id = project.Id
                });
                db.SaveChanges();

                TempData["message"] = "Proiectul a fost adăugat!";
                return RedirectToAction("Show", "Teams",new { id = project.Team_Id });
            }
            else
            {
                TempData["error"] = $"Eroare la adăugarea proiectului: ";
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
