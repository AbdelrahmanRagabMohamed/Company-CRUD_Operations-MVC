using Demo_03.BLL.Interfaces;
using Demo_03.DAL.Contexts;
using Demo_03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo_03.BLL.Repositores
{
    public class DepartementRepository : GenericRepository<Departement>, IDepartementRepository
    {
        private readonly MvcProject_01_DbContext _dbContext;

        #region Non-Generic

        //private readonly MvcProject_01_DbContext _dbContext;

        //// Dependancy Injecation
        //public DepartementRepository(MvcProject_01_DbContext dbContext)  // Ask CLR for Object From DbContext
        //{
        //    _dbContext = dbContext;
        //}


        //// Add , Update , Delete => Return Integer Because it represent Nums of affected Rows in DB

        //public int Add(Departement departement)
        //{
        //    _dbContext.Add(departement);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Departement departement)
        //{
        //    _dbContext.Remove(departement);
        //    return _dbContext.SaveChanges();
        //}

        //public IEnumerable<Departement> GetAll()
        //{
        //    return _dbContext.Departements.ToList();
        //}

        //public Departement GetById(int id)

        //    // First Way :-  Search in Local First if Exsisted instead of Send A new Request 

        //    //var Departemnt = _dbContext.Departements.Local.Where(D =>  D.Id == id).FirstOrDefault(); 

        //    //if (Departemnt == null)
        //    //    Departemnt = _dbContext.Departements.Where(D => D.Id == id).FirstOrDefault();
        //    //return Departemnt;

        //    // Second Way
        //    => _dbContext.Departements.Find(id);



        //public int Update(Departement departement)
        //{
        //    _dbContext.Update(departement);
        //    return _dbContext.SaveChanges();
        //}

        #endregion

        public DepartementRepository(MvcProject_01_DbContext dbContext) : base(dbContext)  // CTOR Chainning
        {
           _dbContext = dbContext;
        }

        public IQueryable<Departement> SearchDepartementByName(string SearchName)
             => _dbContext.Departements.Where(D=>D.Name.ToLower().Contains(SearchName.ToLower() ));
    }
}
