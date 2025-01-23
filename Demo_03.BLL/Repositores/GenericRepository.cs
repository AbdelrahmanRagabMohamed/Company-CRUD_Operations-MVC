using Demo_03.BLL.Interfaces;
using Demo_03.DAL.Contexts;
using Demo_03.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.BLL.Repositores
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MvcProject_01_DbContext _dbContext;


        public GenericRepository(MvcProject_01_DbContext dbContext)  // Dependancy Injection
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T item)
        {
           await _dbContext.AddAsync(item);
            
        }

        public void Delete(T item)
        {
            _dbContext.Remove(item);
            
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // حل مؤقت
            if (typeof(T) == typeof(Employee))
            {
                // Lazy Loading
                return (IEnumerable<T>) await _dbContext.Set<Employee>().Include(E => E.Departement).ToListAsync();
            }

            return await _dbContext.Set<T>().ToListAsync();

            // The Best Soluation for this Problem that using => Specification Design Pattern
        }


        public async Task<T> GetByIdAsync(int id)
         => await _dbContext.Set<T>().FindAsync(id);


        public void Update(T item)
        {
            _dbContext.Update(item);
           
        }
    }
}
