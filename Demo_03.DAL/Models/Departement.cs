using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.DAL.Models
{
    public class Departement
    {
        public int Id { get; set; }  // Pk

        [Required]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        public string FileName { get; set; }

        [InverseProperty("Departement")]
        public IEnumerable<Employee> Employees { get; set; } = new HashSet<Employee>();
        
    }
}
