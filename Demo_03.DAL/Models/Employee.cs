using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }  // PK

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }

        public string Address { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;


        public string ImageName { get; set; }

        [ForeignKey("Departement")]
        public int? DepartementId { get; set; }  // FK

        // FK => Optional (nullable)  ==> OnDelete is Restrict
        // FK => Required  ==> OnDelete is Cascade

        [InverseProperty("Employees")]
        public Departement Departement { get; set; }


    }

}
