﻿using Microsoft.AspNetCore.Identity;

namespace App.NET.Models
{
    //aici se afla userul nostru care mosteneste ce are IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public int total_points { get; set; }

        //legatura many-to-many cu echipe
        public virtual ICollection<Team_member>? Team_member { get; set; }
    }
}