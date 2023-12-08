﻿using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Task_table
    {
        [Key]
        public int Id { get; set; }
        public string Title_task { get; set; }
        public string? Description_task { get; set; }
        public int? Status { get; set; }
        public DateTime? Data_start { get; set; }
        public DateTime? Data_end { get; set; }
        public int? Project_id { get; set; }
        public virtual Project? Project { get; set; }
        // conectare many to many
        public virtual ICollection<User_task> User_task { get; set; }
    }
}