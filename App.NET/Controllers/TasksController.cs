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
        public IActionResult New(int id)
        {
            Project project = _db.Projects.Find(id);
            if (project.Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin")){
                ViewBag.Project = project;

                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa adaugati un nou task!!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Projects");
            }

            
        }

        [HttpPost]
        public IActionResult New(int id, Task_table task)
        {

            var sanitizer = new HtmlSanitizer();

            if (ModelState.IsValid)
            {

                task.Title_task = sanitizer.Sanitize(task.Title_task);
                task.Description_task = sanitizer.Sanitize(task.Description_task);
                task.Media = sanitizer.Sanitize(task.Media);

                task.Project_id = id;
                task.Status = TaskStatus.NotStarted; // Setează statusul implicit

                _db.Tasks.Add(task);
                _db.SaveChanges();

                TempData["message"] = "Task-ul a fost adăugat!";
                return RedirectToAction("Show", "Projects", new { id = id });
            }
            else
            {
                Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
                ViewBag.Project = project;
                return View(task);
            }
        }
        public IActionResult Show(int id)
        {
            var task = _db.Tasks.Find(id);
            
            return View(task);
        }

        public IActionResult Add_Users(int id)//id from the task
        {

            var task = _db.Tasks.Find(id);
            var users =_db.Users.Where(user => user.UserProjects.Any(j => j.Project_id == task.Project_id) && user.User_task.All(j => j.Task_id != task.Id));
            if (users.Count() == 0) { ViewBag.none = true; }
            else { ViewBag.none = false; }
            ViewBag.project_id = id;
            return View(users);
        }
        public IActionResult Edit(int id)
        {
            Task_table task = _db.Tasks.FirstOrDefault(t => t.Id == id);
            Project project = _db.Projects.FirstOrDefault(p => p.Id == task.Project_id);

            ViewBag.Task = task;
            ViewBag.Project = project;

            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, Task_table updatedTask)
        {
            var sanitizer = new HtmlSanitizer();

            if (ModelState.IsValid)
            {
                Task_table task = _db.Tasks.FirstOrDefault(t => t.Id == id);

                updatedTask.Title_task = sanitizer.Sanitize(updatedTask.Title_task);
                updatedTask.Description_task = sanitizer.Sanitize(updatedTask.Description_task);
                updatedTask.Media = sanitizer.Sanitize(updatedTask.Media);

                task.Title_task = updatedTask.Title_task;
                task.Description_task = updatedTask.Description_task;
                task.Status = updatedTask.Status;
                task.Data_start = updatedTask.Data_start;
                task.Data_end = updatedTask.Data_end;
                task.Media = updatedTask.Media;

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

          
    }
}
