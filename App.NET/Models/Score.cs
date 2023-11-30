using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }
        public int team_member_id { get; set; }
        public virtual Team_member? Team_member { get; set; }
        public DateTime date { get; set; }
        public int score { get; set; }

        public int? bonus { get; set; }
    }
}
