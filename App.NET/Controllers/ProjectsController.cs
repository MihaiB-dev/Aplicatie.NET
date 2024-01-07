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
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            var project =  db.Projects.Where(p => p.Id == id).First();
            //avem 4 type de taskuri: Your tasks, Notstarted, Inprogress, Completed
            var local_user = _userManager.GetUserId(User);

            //daca userul care a incercat sa intre in show nu face parte din proiect si nu este admin
            if(db.UserProjects.Where(p => p.User_id == local_user && p.Project_id == project.Id).Count() == 0 && !User.IsInRole("Admin"))
            {
                TempData["message"] = "Nu aveti dreptul sa intrati in acest proiect";
                TempData["messageType"] = "alert-danger";

                return RedirectToAction("Index", "Teams");
            }
            //1. Your tasks: sa se afiseze taskurile notstarted sau Inprogress din proiectul curent unde face parte userul curent
            var your_tasks = db.Tasks.Where(
                p => p.Project_id == id
                && (p.Status == Models.TaskStatus.NotStarted || p.Status == Models.TaskStatus.InProgress)
                && p.User_task.Any(j => j.User_id == local_user));

            //2. Notstarted: sa se afiseze taskurile notstarted din proiectul curent unde NU face parte userul curent
            var Notstarted = db.Tasks.Where(
                p => p.Project_id == id
                && p.Status == Models.TaskStatus.NotStarted
                && p.User_task.All(j => j.User_id != local_user));

            //3. Inprogress: sa se afiseze taskurile Inprogress din proiectul curent unde NU face parte userul curent
            var Inprogress = db.Tasks.Where(
                p => p.Project_id == id
                && p.Status == Models.TaskStatus.InProgress
                && p.User_task.All(j => j.User_id != local_user));

            //4. Completed: sa se afiseze taskurile Completed din proiectul curent unde NU face parte userul curent
            var Completed = db.Tasks.Where(
                p => p.Project_id == id
                && p.Status == Models.TaskStatus.Completed
                && p.User_task.All(j => j.User_id != local_user));


            //var tasks = db.Tasks.Where(task =>  task.Project_id == id);
            ViewBag.your_tasks = your_tasks;
            ViewBag.Notstarted = Notstarted;
            ViewBag.InProgress = Inprogress;
            ViewBag.Completed = Completed;

            ViewBag.your_tasks_count = your_tasks.Count();
            ViewBag.Notstarted_count = Notstarted.Count();
            ViewBag.InProgress_count = Inprogress.Count();
            ViewBag.Completed_count = Completed.Count();

            var owner = db.Users.Where(p => p.Id == project.Users_Id);
            if(owner.Count() == 0)
            {
                ViewBag.UserName = "owner was kicked out";
            }
            else
            {
                ViewBag.UserName = owner.First().UserName;

            }
            ViewBag.teamName = db.Teams.Where(p => p.Id == project.Team_Id).First().Name;

            var users = db.Users.Where(user => user.UserProjects.Any(j => j.Project_id == id));

            var max_afisare_useri = 5;
            ViewBag.users = users.Take(max_afisare_useri); //l-om doar primele x persoane
            if (users.Count() > max_afisare_useri) { ViewBag.countUsers = users.Count() - max_afisare_useri; }

            if (project == null)
            {
                return NotFound();
            }


            //verificare daca este organizator
            if(project.Users_Id == local_user || User.IsInRole("Admin"))
            {
                SetAccessRights();
            }
            else
            {
                ViewBag.AfisareButoane = false;
                ViewBag.EsteAdmin = false;
            }
            return View(project);
        }
        [Authorize(Roles = "User,Editor,Admin")]
        //trb sa verificam daca acest user este creatorul proiectului
        public IActionResult Add_Users(int id)//id-ul reprezinta id-ul proiectului la care adaugam useri
        {
            var project = db.Projects.Find(id);
            if(project.Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                //luam toti utilizatorii care se afla in echipa si care nu se afla deja in proiect;
                var users = db.Users.Where(user => user.Team_member.Any(j => j.Team_id == project.Team_Id) && user.UserProjects.All(j => j.Project_id != project.Id));
                if (users.Count() == 0) { ViewBag.none = true; }
                else { ViewBag.none = false; }
                ViewBag.project = project;


                return View(users);
            }
            ViewBag.AfisareButoane = false;
            ViewBag.EsteAdmin = false;
            TempData["message"] = "Nu aveti dreptul sa adaugati utilizatori la acest proiect";
            TempData["messageType"] = "alert-danger";

            return RedirectToAction("Show", "Projects", new { id = id });
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


        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            if (db.Projects.Find(id).Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(project);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa adaugati utilizatori la acest proiect";
                TempData["messageType"] = "alert-danger";

                return RedirectToAction("Index", "Teams");
            }

           

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
            if (db.Projects.Find(id).Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Projects.Remove(project);
                db.SaveChanges();

                TempData["message"] = "Proiectul a fost sters!";
                return RedirectToAction("Show", "Teams", new {id = project.Id});
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergi acest proiect";
                TempData["messageType"] = "alert-danger";

                return RedirectToAction("Index", "Teams");
            }
            
        }

        //este activat doar daca userul este organizator sau admin
        private void SetAccessRights()
        {
            
            ViewBag.AfisareButoane = true; //ori este organizator ori admin

            ViewBag.EsteAdmin = User.IsInRole("Admin"); //folosit pentru zonele in care doar adminul poate face lucruri

            
        }
    }
}
