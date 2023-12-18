using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.NET.Controllers
{
    public class TeamsController : Controller
    {
        //adaugam user si roluri 
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _db;
        public TeamsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var teams = _db.Teams.OrderBy(team => team.Id).ToList();
            ViewBag.Teams = teams;
            var userName = HttpContext.User.Identity.Name;
            ViewBag.UserName = userName;

            return View(teams); // Asigură-te că transmiți lista de echipe la View
        }


        public IActionResult Show(int id)
        {
            Team team = _db.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }

            ICollection<Team_member> teamMembers = _db.Team_members.Include(tm => tm.User)
                                                                   .Where(tm => tm.Team_id == id)
                                                                   .ToList();

            ViewBag.Team = team;
            ViewBag.TeamMembers = teamMembers;

            return View(team);
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
                _db.Teams.Add(team);
                var userName = HttpContext.User.Identity.Name;
                ApplicationUser user = _db.ApplicationUsers.SingleOrDefault(u => u.UserName == userName);

                if (user != null)
                {
                    _db.Team_members.Add(new Team_member
                    {
                        Team_id = team.Id,
                        User_id = user.Id
                    });
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            Team team = _db.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }

            ViewBag.Team = team;
            return View(team);
        }

        [HttpPost]
        public IActionResult Edit(int id, Team requestTeam)
        {
            Team team = _db.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }

            try
            {
                team.Name = requestTeam.Name;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", new { id });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Team team = _db.Teams.Find(id);

            if (team == null)
            {
                return NotFound();
            }

            _db.Teams.Remove(team);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
