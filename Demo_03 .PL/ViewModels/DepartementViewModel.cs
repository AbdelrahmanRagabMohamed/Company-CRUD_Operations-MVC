using Demo_03.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo_03_.PL.ViewModels
{
    public class DepartementViewModel
    {
        public int Id { get; set; }  // Pk

        [Required(ErrorMessage = "Name is Requried")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is Requried")]
        public int Code { get; set; }

        [Required(ErrorMessage = "Date Of Creation is Required")]
        public DateTime DateOfCreation { get; set; }

        public IFormFile file { get; set; }
        public string FileName { get; set; }

        [InverseProperty("Departement")]
        public IEnumerable<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
