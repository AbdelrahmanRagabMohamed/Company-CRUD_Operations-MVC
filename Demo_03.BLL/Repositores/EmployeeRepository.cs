using Demo_03.BLL.Interfaces;
using Demo_03.DAL.Contexts;
using Demo_03.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.BLL.Repositores
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcProject_01_DbContext _dbContext;
        #region Non-Generic

        //private readonly MvcProject_01_DbContext _dbContext;

        //public EmployeeRepository(MvcProject_01_DbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        //public int Add(Employee employee)
        //{
        //    _dbContext.Add(employee);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Employee employee)
        //{
        //    _dbContext.Remove(employee);
        //    return _dbContext.SaveChanges();
        //}

        //public IEnumerable<Employee> GetAll()
        //{
        //    return _dbContext.Employees.ToList();
        //}

        //public Employee GetById(int id)
        //{
        //    // First Way :-  Search in Local First if Exsisted instead of Send A new Request 
        //    var Employee = _dbContext.Employees.Local.Where(D => D.Id == id).FirstOrDefault();

        //    if (Employee == null)
        //        Employee = _dbContext.Employees.Where(D => D.Id == id).FirstOrDefault();

        //    return Employee;
        //}

        //public int Update(Employee employee)
        //{
        //    _dbContext.Update(employee);
        //    return _dbContext.SaveChanges();
        //}

        #endregion


        public EmployeeRepository(MvcProject_01_DbContext dbContext) : base(dbContext)  // CTOR Chainning
        {
            _dbContext = dbContext;  // open Connection with DB
        }

        public IQueryable<Employee> GetEmployeesAddressById(string address)
        {
            return _dbContext.Employees.Where(E => E.Address == address);
        }


        // Search Task
        public IQueryable<Employee> SearchEmployeeByName(string SearchName)
        
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(SearchName.ToLower() ));








    }
}
