using Demo_03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_03.BLL.Interfaces
{
    public interface IDepartementRepository : IGenericRepository<Departement>
    {
        #region Non-Generic

        //IEnumerable<Departement> GetAll();

        //Departement GetById(int id);

        //int Add(Departement departement);

        //int Update(Departement departement);

        //int Delete(Departement departement);


        // Add , Update , Delete => Return Integer Because it represent Nums of affected row in DB

        #endregion


        public IQueryable<Departement> SearchDepartementByName(string SearchName);
    }
}
