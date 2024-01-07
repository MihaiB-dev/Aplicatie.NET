using System;
using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int TaskId { get; set; }
        public virtual Task_table Task { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu")]
        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}
