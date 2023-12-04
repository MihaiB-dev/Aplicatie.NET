﻿using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int Id_task { get; set; }
        public virtual Task_table? Task { get; set; }
        public int Id_user { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string Text { get; set; }
        public DateTime date { get; set;}
        
    }
    
}
