using Demo_03.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo_03_.PL.ViewModels
{
    public class EmployeeViewModel
    {

        public int Id { get; set; }  // PK

        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min Length is 5 Chars")]
        public string Name { get; set; }

        [Range(22, 40, ErrorMessage = "Age Must be in Range From 22 To 35")]
        public int? Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
           ErrorMessage = "Address Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }



        public IFormFile Image { get; set; }

        public string ImageName { get; set; }   // Binding




        [ForeignKey("Departement")]
        public int? DepartementId { get; set; }  // FK

        // FK => Optional (nullable)  ==> OnDelete is Restrict
        // FK => Required  ==> OnDelete is Cascade

        [InverseProperty("Employees")]
        public Departement Departement { get; set; }

    }

}
