using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Team_member
    {
        [Key]
        public int Id { get; set; }
        public string User_id { get; set; }
        public int Team_id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Team? Team { get; set; }
    }
}
