using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        // ApplicationUser => Represent Table in DB (AspNetUsers)
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        public bool IAgree { get; set; }

        
    }
}
