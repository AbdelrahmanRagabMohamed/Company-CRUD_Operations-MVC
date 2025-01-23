using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        // Signature for Property for Each and Every Repository Interface 

        public IEmployeeRepository EmployeeRepository { get; set; }

        public IDepartementRepository DepartementRepository { get; set; }

        Task<int> CompleteAsync();

        // void Dispose(); ==> CLR Will Enforce Me on Specific Signature by Implement IDisposable interface
    }
}
