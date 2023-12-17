using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }
        public int? Team_memberId { get; set; }
        public virtual Team_member? Team_member { get; set; }
        public DateTime DateScore { get; set; }
        public int? Points { get; set; }

        public int? Bonus { get; set; }
    }
}
