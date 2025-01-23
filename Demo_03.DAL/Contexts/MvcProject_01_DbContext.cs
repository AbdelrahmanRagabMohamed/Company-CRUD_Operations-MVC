using Demo_03.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.DAL.Contexts
{
    public class MvcProject_01_DbContext : IdentityDbContext<ApplicationUser>
    {

        // Another way for Connection string
        public MvcProject_01_DbContext(DbContextOptions<MvcProject_01_DbContext> options) : base(options)
        {
            
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = . ;Database = MvcAppDemo01 ;Trusted_Connection = true");
        //}

        public DbSet<Departement> Departements { get; set; }

        public DbSet<Employee> Employees { get; set; }

        
    }
}
