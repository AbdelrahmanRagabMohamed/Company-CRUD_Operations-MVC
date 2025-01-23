using Demo_03.BLL.Interfaces;
using Demo_03.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.BLL.Repositores
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly MvcProject_01_DbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get; set; }

        public IDepartementRepository DepartementRepository { get; set; }


        public UnitOfWork(MvcProject_01_DbContext dbContext)  // Inject (Ask CLR For Object From DbContext)
        {
            EmployeeRepository = new EmployeeRepository(dbContext);  // Passing By Parameter 
            DepartementRepository = new DepartementRepository(dbContext);  // Passing By Parameter 

            _dbContext = dbContext;
        }


        // Save Changes in DB
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        // Close Connection With DB
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
