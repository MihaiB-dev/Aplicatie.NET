using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Title_project { get; set; }
        public string ? Users_Id { get; set; } 
        public virtual ApplicationUser? Users { get; set; }
        public int? Team_Id { get; set; }
        public virtual Team? Team { get; set; }
        public virtual ICollection<Task_table>? Tasks { get; set; }  
        public string? Description { get; set; }

        //un proiect are mai multe persoane
        public virtual ICollection<UserProject>? UserProjects { get; set; }
    }
}
