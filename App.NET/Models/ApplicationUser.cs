using Microsoft.AspNetCore.Identity;

namespace App.NET.Models
{
    //aici se afla userul nostru care mosteneste ce are IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public int total_points { get; set; }

        //legatura many-to-many cu echipe
        public virtual ICollection<Team_member>? Team_member { get; set; }

        //legatura many-to-many cu taskuri
        public virtual ICollection<User_task>? User_task { get; set; }

        // legatura cu comentarii
        public virtual ICollection<Comment>? Comments { get; set; }

        //legatura cu proiecte (daca este organizator)
        public virtual ICollection<Project>? Projects { get; set; }

        //legatura cu proiecte (daca face parte dintr-un proiect)
        public virtual ICollection<UserProject>? UserProjects { get; set; }
    }
}
