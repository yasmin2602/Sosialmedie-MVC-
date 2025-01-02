using Microsoft.AspNetCore.Identity;
using System;

namespace photogram.Models
{
    public class User : IdentityUser
    {
        // Egendefinerte felter
        //public string Name { get; set; } // 
        public DateTime CreatedAt { get; set; } = DateTime.Now; // creates time

    }
}
