using Demo_03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        #region Non-Generic

        //IEnumerable<Employee> GetAll();

        //Employee GetById(int id);

        //int Add(Employee employee);

        //int Update(Employee employee);

        //int Delete(Employee employee);


        // Add , Update , Delete => Return Integer Because it represent Nums of affected row in DB

        #endregion


        IQueryable<Employee> GetEmployeesAddressById(string address);

        public IQueryable<Employee> SearchEmployeeByName(string Name);

    }
}
