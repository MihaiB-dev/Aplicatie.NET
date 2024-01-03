using App.NET.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;

namespace App.NET.Data
{
    //adaugat ApplicationUser la IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        //aici se adauga toate tabelele create
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Team_member> Team_members { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Score> Scores { get; set; }

        public DbSet<Badge> Badges { get; set; }
        // adaug clasele project, task, comment, user_task
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task_table> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User_task> User_tasks { get; set; }

        //sfarsit de tabele


        //am facut legatura many-to-many intre ApplicationUser is Team
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<Team_member>()
            .HasKey(ac => new { ac.User_id, ac.Team_id });
            // definire relatii cu modelele Category si Article (FK)
            modelBuilder.Entity<Team_member>()
            .HasOne(ac => ac.User)
            .WithMany(ac => ac.Team_member)
            .HasForeignKey(ac => ac.User_id);
            modelBuilder.Entity<Team_member>()
            .HasOne(ac => ac.Team)
            .WithMany(ac => ac.Team_member)
            .HasForeignKey(ac => ac.Team_id);

            // definire primary key compus
            modelBuilder.Entity<User_task>()
                .HasKey(ac => new { ac.User_id, ac.Task_id });
            // definire relatii cu modelele Category si Article (FK)
            modelBuilder.Entity<User_task>()
                .HasOne(ac => ac.User)
                .WithMany(ac => ac.User_task)
                .HasForeignKey(ac => ac.User_id);
            modelBuilder.Entity<User_task>()
                .HasOne(ac => ac.Task)
                .WithMany(ac => ac.User_task)
                .HasForeignKey(ac => ac.Task_id);

            
         
        }

        // facem CRUD pentru team, team-member si user
        public DbSet<App.NET.Models.User_task> User_task { get; set;}


    }
}