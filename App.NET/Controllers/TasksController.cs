using App.NET.Data;
using App.NET.Models;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TaskStatus = App.NET.Models.TaskStatus; // Pentru a nu se confunda cu System.Threading.Tasks.Task

namespace App.NET.Controllers
{
    [Authorize]

    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public TasksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _db = context;

            _userManager = userManager;

            _roleManager = roleManager;

        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            var tasks = _db.Tasks.Include(t => t.Project).ToList();
            return View(tasks);
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult New()
        {

            var id = Convert.ToInt32(HttpContext.Request.Query["project"]); // preluam id-ul proiectului din query string
            Project project = _db.Projects.Find(id);  

            if (project.Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin")){ 
                ViewBag.Project = project; 

                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa adaugati un nou task!!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", "Projects", new {id = id}); 
            }

            
        }

        [HttpPost]
        public IActionResult New(Task_table task)
        {

            var sanitizer = new HtmlSanitizer();

            if (ModelState.IsValid)
            {

                task.Title_task = sanitizer.Sanitize(task.Title_task); 
                task.Description_task = sanitizer.Sanitize(task.Description_task);
                task.Media = sanitizer.Sanitize(task.Media);

                
               // task.Status = TaskStatus.NotStarted; // Setează statusul implicit
                //task.Status = TaskStatus.InProgress;
                //task.Status = TaskStatus.Completed;

                // pentru statusul task-ului

                


                _db.Tasks.Add(task);
                _db.SaveChanges();

                TempData["message"] = "Task-ul a fost adăugat!";
                return RedirectToAction("Show", "Projects", new { id = task.Project_id });
            }
            else
            {
                Project project = _db.Projects.FirstOrDefault(p => p.Id == task.Project_id); // preluam proiectul din baza de date
                ViewBag.Project = project; 
                return View(task);
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            
            var task = _db.Tasks.Include(t => t.User_task)
                    .Include(t => t.Comments)
                    .ThenInclude(comment => comment.User) 
                    .Where(t => t.Id == id)
                    .First();
            /*
            var task = _db.Tasks.Find(id);
            
            var comments = _db.Comments.Where(p => p.TaskId == id);
            ViewBag.Comments = comments;
            */
            if (task == null) 
            {
                return NotFound();
            }
            ViewBag.UserCurent = _userManager.GetUserId(User); 

            if (_db.Projects.Any(p => p.Id == task.Project_id &&
                                       (p.Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin")))) // daca utilizatorul curent este organizatorul proiectului sau este admin
            {
                SetAccessRights(_db.Projects.First(p => p.Id == task.Project_id).Users_Id == _userManager.GetUserId(User)); // daca utilizatorul curent este organizatorul proiectului ii dam acces la butoanele de editare si stergere a task-ului
            }
            else
            {
                ViewBag.AfisareButoane = false;
                ViewBag.EsteAdmin = false;
            }

            var localUser = _userManager.GetUserId(User); 


            if (_db.UserProjects.Count(up => up.User_id == localUser && up.Project_id == task.Project_id) == 0 && !User.IsInRole("Admin")) // daca utilizatorul curent nu este membru al proiectului si nu este admin
            {
                TempData["message"] = "Nu aveti dreptul la acest task!!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Teams");
            }

            return View(task);
        }







        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = DateTime.Now; 

            // preluam id-ul utilizatorului care posteaza comentariul
            comment.UserId = _userManager.GetUserId(User);

            if (true)
            {
                _db.Comments.Add(comment);
                _db.SaveChanges();
                return Redirect("/Tasks/Show/" + comment.TaskId); 
            }

            else
            {
                Task_table tsk = _db.Tasks.Include(tsk => tsk.User_task)
                                        .Include(tsk => tsk.Comments)
                                        .ThenInclude(comment => comment.User)
                                        .Where(tsk => tsk.Id == comment.TaskId) 
                                        .First();


                SetAccessRights(); 

                return View(tsk);
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Add_Users(int id)
        {
            var task = _db.Tasks.Find(id);
            if (_db.Projects.Where(p => p.Id == task.Project_id).First().Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                var users = _db.Users.Where(user => user.UserProjects.Any(j => j.Project_id == task.Project_id) && user.User_task.All(j => j.Task_id != task.Id)); // preluam toti utilizatorii care sunt membrii ai proiectului si nu sunt membrii ai task-ului
                if (users.Count() == 0) { ViewBag.none = true; }  
                else { ViewBag.none = false; }
                ViewBag.task = task;

                return View(users); // afisam utilizatorii care pot fi adaugati la task
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa adaugati membrii la task!!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", "Projects", new { id = id });
            }
        }

        [HttpPost]
        public IActionResult Add_Users(User_task usertask)
        {
            _db.User_tasks.Add(usertask);
            _db.SaveChanges();
            return RedirectToAction("Add_Users", new { id = usertask.Task_id });
        }
        public IActionResult Edit(int id)
        {
            Task_table task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            Project project = _db.Projects.FirstOrDefault(p => p.Id == task.Project_id);

            ViewBag.Task = task;
            ViewBag.Project = project;

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(int id, Task_table updatedTask)
        {
            var sanitizer = new HtmlSanitizer();

            if (ModelState.IsValid)
            {
                Task_table task = _db.Tasks.FirstOrDefault(t => t.Id == id);

                task.Title_task = sanitizer.Sanitize(updatedTask.Title_task);
                task.Description_task = sanitizer.Sanitize(updatedTask.Description_task);
                task.Media = sanitizer.Sanitize(updatedTask.Media);

                //task.Title_task = updatedTask.Title_task;
                //task.Description_task = updatedTask.Description_task;
                task.Status = updatedTask.Status;
                task.Data_start = updatedTask.Data_start;
                task.Data_end = updatedTask.Data_end;
                //task.Media = updatedTask.Media;

                _db.SaveChanges();

                TempData["message"] = "Task-ul a fost modificat!";
                return RedirectToAction("Show", "Projects", new { id = task.Project_id });
            }
            else
            {
                Project project = _db.Projects.FirstOrDefault(p => p.Id == updatedTask.Project_id);
                ViewBag.Project = project;
                return View(updatedTask);
            }
        }





        [HttpPost]
        public IActionResult Delete(int id)
        {
            Task_table task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            int projectId = (int)task.Project_id; 

            _db.Tasks.Remove(task);
            _db.SaveChanges();

            TempData["message"] = "Task-ul a fost șters!";
            return RedirectToAction("Show", "Projects", new { id = projectId });
        }

        private void SetAccessRights(bool organizer = false)
        {

            ViewBag.AfisareButoane = true; //ori este organizator ori admin
            if(organizer)
            {
                ViewBag.EsteOrganizator = true;
            }
            else
            {
                ViewBag.EsteOrganizator = false;
            }
            ViewBag.EsteAdmin = User.IsInRole("Admin"); // daca utilizatorul curent este admin ii dam acces la butoanele de editare si stergere a task-ului


        }
    }
}
