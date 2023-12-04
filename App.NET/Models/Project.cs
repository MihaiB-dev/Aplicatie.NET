using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Title_project { get; set; }
        public string ? User_id { get; set; } 
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public int ? Team_id { get; set; }
        public virtual Team? Team { get; set; }
        public virtual ICollection<Task_table> Tasks { get; set; }  
        public string title { get; set; }
        public string? description { get; set; }
        public string password { get; set; }

    }
}
