using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Linq.Expressions;

namespace App.NET.Controllers
{
    public class TeamsController : Controller
    {
        // facem CRUD pentru Team

        private readonly ApplicationDbContext db;
        public TeamsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var teams = from team in db.Team
                        orderby team.Id
                        select team;
            ViewBag.Teams = teams;
            var UserName = HttpContext.User.Identity.Name;
            ViewBag.UserName = UserName;
            return View();
        }
      
        public IActionResult Show(int id)
        {
            
           Team teams = db.Team.Find(id);
           Team_member team_member = db.Team_member.Include("Team")
                                        .Where(team_member => team_member.Team_id == id)
                                        .First();
            ApplicationUser users = (ApplicationUser)db.ApplicationUsers.Where(user => team_member.User_id == user.Id);
            


            ViewBag.Teams = teams;
            ViewBag.Users = users;

            return View(teams);
           
        }
     

        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Team team)
        {
            try
            {
                
                db.Team.Add(team);
                var UserName= HttpContext.User.Identity.Name;
                ApplicationUser user = db.ApplicationUsers.Where(user => user.UserName == UserName).First();

                db.Team_member.Add(new Team_member
                {
                    Team_id = team.Id,
                    User_id = user.Id
                });
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return View();
            }
        }
        /*
        catch (Exception)
        {
            return View();
        }
        */

        public IActionResult Edit(int id)
        {
            Team team = db.Team.Find(id);
            ViewBag.Team = team;
            return View(team);
        }

        [HttpPost]
        public IActionResult Edit(int id, Team requestTeam)
        {
            Team team = db.Team.Find(id);

            try
            {
                team.Name = requestTeam.Name;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", team.Id);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Team team = db.Team.Find(id);
            db.Team.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


  

    }

    
}
