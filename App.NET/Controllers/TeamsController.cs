﻿using App.NET.Data;
using App.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;
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
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index()
        {
            //preluam toate echipele din care face parte userul respectiv.
            var local_user = _userManager.GetUserId(User);
            
            var your_teams = _db.Teams.Where(team => team.Team_member.Any(j => j.User_id == local_user));
            ViewBag.your_teams = your_teams;
            
            //preluam toate echipele existente in care nu este userul nostru
            var teams = _db.Teams.Where(Team => Team.Team_member.All(j => j.User_id != local_user));

            var count = new Dictionary<int, int>(); 
            var creators = new Dictionary<int, string>();
            foreach(var team in teams)
            {
                var calculate_counter = _db.Team_members.Where(tm => tm.Team_id == team.Id).Count();
                var calculate_creator = "default";
                creators.Add(team.Id, calculate_creator);
                count.Add(team.Id, calculate_counter);
            }
            ViewBag.creator = creators;
            ViewBag.count = count;

            if (User.IsInRole("Admin"))
            {
                ViewBag.EsteAdmin = User.IsInRole("Admin"); 
            }
            else
            {
                ViewBag.EsteAdmin = false;
            }
            //paginare
            int _perPage = 4;
            int totalItems = teams.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0; //cate pagini s-au afisat deja, depinde de _perPage

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginateTeams = teams.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
            ViewBag.teams = paginateTeams;
            ViewBag.teams_count = teams.Count();
            return View(); // Asigură-te că transmiți lista de echipe la View
        }
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Auth(int id)
        {
            var team = _db.Teams.Where(team => team.Id == id).First();
            ViewBag.team = team;
            return View();
        }
        //folosit cand userul incearca sa intre intr-o echipa
        [HttpPost]
        public IActionResult Auth(int id, string password)
        {
            var team = _db.Teams.Where(team => team.Id == id).First();
            if(password == team.Password)
            {
                var local_user = _userManager.GetUserId(User);
                _db.Team_members.Add(new Team_member
                {
                    Team_id = team.Id,
                    User_id = local_user
                });
                _db.SaveChanges();
                TempData["message"] = "Ati intrat in echipa:" + team.Name + " cu succes";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Parola este incorecta";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Auth", "Teams", new {id = id});
            }
        }
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Your_Teams()
        {
            var local_user = _userManager.GetUserId(User);
            //aici facem lista de la userul curent cu toate echipele in care face parte.
            var your_teams = _db.Teams.Where(team => team.Team_member.Any(j => j.User_id == local_user));
            ViewBag.teams = your_teams;
            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            var local_user = _userManager.GetUserId(User);
            //daca userul este in echipa
            if (_db.Team_members.Where(p => p.Team_id == id && p.User_id == local_user).Count() == 0 && !User.IsInRole("Admin"))
            {
                TempData["message"] = "Nu aveti dreptul sa intrati in aceasta echipa";
                TempData["messageType"] = "alert-danger";

                return RedirectToAction("Index", "Teams");
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.esteAdmin = true;
            }
            else
            {
                ViewBag.esteAdmin = false;
            }

            var your_teams = _db.Teams.Where(team => team.Team_member.Any(j => j.User_id == local_user));
            ViewBag.your_teams = your_teams;

            Team team = _db.Teams.Find(id);

            var users = _db.Users.Where(user => user.Team_member.Any(j => j.Team_id == id));

            var max_afisare_useri = 5;
            ViewBag.users = users.Take(max_afisare_useri); //l-om doar primele x persoane
            if(users.Count() > max_afisare_useri) { ViewBag.countUsers = users.Count() - max_afisare_useri; }

            if (team == null)
            {
                return NotFound();
            }
            //integram proiectele care exista in aceasta echipa

            //afisez proiectele care le are userul curent
            var my_projects = _db.Projects.Where(project => project.Users_Id == local_user && project.Team_Id == id);
            if (my_projects.Count() != 0) { ViewBag.MyProjects = my_projects; }

            //afisez proiectele din care fac parte
            var current_projects = _db.Projects.Where(project => project.UserProjects.Any(j => j.User_id == local_user) && project.Users_Id != local_user && project.Team_Id == id);
            if (current_projects.Count() != 0) { ViewBag.CurrentProjects = current_projects; }

            //afisez proiectele din care userul nu face parte
            var other_projects = _db.Projects.Where(project => project.UserProjects.All(j => j.User_id != local_user) && project.Team_Id == id);
            if(other_projects.Count() != 0){ViewBag.OtherProjects = other_projects;}

            ViewBag.Team = team;

            return View(team);
            
        }
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Team team)
        {
                var local_user = _userManager.GetUserId(User);
                
                _db.Teams.Add(team);
                _db.SaveChanges();
                
                _db.Team_members.Add(new Team_member
                {
                    Team_id = team.Id,
                    User_id = local_user
                });

                _db.SaveChanges();
                return RedirectToAction("Index");
           
        }
        [Authorize(Roles = "User,Editor,Admin")]
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
