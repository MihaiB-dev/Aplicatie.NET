using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.NET.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext db;
        public ProjectsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index(int team_id)
        {
            var projects = db.Projects.Include("ApplicationUser").Include("Team")
                .Where(project => project.User_id == User.Identity.Name && project.Team_id == team_id);
            return View();
        }

        public IActionResult Show(int id)
        {
            Project project = db.Projects.Find(id);

            ICollection<Task_table> tasks = db.Tasks.Include("Project").Where(task => task.Project_id == id).ToList();

            ViewBag.Tasks = tasks;
            return View(project);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]

        public IActionResult New(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                TempData["message"] = "Proiectul a fost adaugat!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(project);
            }
        }

        public IActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);
        }

        [HttpPut]
        public IActionResult Edit (int id, Project requestProject)
        {
            Project project = db.Projects.Find(id);
           
           
                if (ModelState.IsValid)
                {
                    project.title = requestProject.title;
                    project.description = requestProject.description;
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
            db.Projects.Remove(project);
            db.SaveChanges();

            TempData["message"] = "Proiectul a fost sters!";
            return RedirectToAction("Index");
        }   
    }
}
