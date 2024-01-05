using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{

    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed
    }



    public class Task_table
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu.")]
        public string Title_task { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie.")]
        public string Description_task { get; set; }

        [Required(ErrorMessage = "Statusul este obligatoriu.")]
        public TaskStatus Status { get; set; }

        [Required(ErrorMessage = "Data de start este obligatorie.")]
        public DateTime Data_start { get; set; }

        [Required(ErrorMessage = "Data de finalizare este obligatorie.")]
        public DateTime Data_end { get; set; }

        [Required(ErrorMessage = "Trebuie sa scrii ceva aici!")]
        public string Media { get; set; }

        public int? Project_id { get; set; }
        public virtual Project? Project { get; set; }
        // conectare many to many
        public virtual ICollection<User_task>? User_task { get; set; }
    }
}
