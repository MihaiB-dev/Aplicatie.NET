using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TaskStatus = App.NET.Models.TaskStatus; // Pentru a nu se confunda cu System.Threading.Tasks.Task

namespace App.NET.Controllers
{
<<<<<<< Updated upstream
    [Authorize]
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            Project project = _db.Projects.Find(id);
            if (project.Users_Id == _userManager.GetUserId(User) || User.IsInRole("Admin")){
                ViewBag.Project = project;
=======

            Project project = _db.Projects.FirstOrDefault(p => p.Id == projectId);
            ViewBag.Project = project;
>>>>>>> Stashed changes

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
            if (ModelState.IsValid)
            {
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
            if (ModelState.IsValid)
            {
                Task_table task = _db.Tasks.FirstOrDefault(t => t.Id == id);

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
