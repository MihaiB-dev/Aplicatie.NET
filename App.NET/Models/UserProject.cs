using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class UserProject
    {
        [Key]
        //public int Id { get; set; }
        public string User_id { get; set; }
        public int Project_id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Project? Project { get; set; }
    }
}
