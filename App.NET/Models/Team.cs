using System.ComponentModel.DataAnnotations;

namespace App.NET.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        //connection many to many with user
        public virtual ICollection <Team_member> Team_member { get; set; }


    }
}
