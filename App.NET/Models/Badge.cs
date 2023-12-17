using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        public int ScoreId { get; set; }
        public virtual Score? Score { get; set; }
        public string? TitleBadge { get; set; }
    }
}
