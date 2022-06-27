using BusinessLayer.Interfaces;
using DomainLayer.DbEFContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private AppDbContext _context = null;
        private DbSet<T> table = null;


        public Repository()
        {
            this._context = new AppDbContext();
            this.table = _context.Set<T>();
        }


          public Repository(AppDbContext context)
        {
            this._context = context;
            table = _context.Set<T>();
        }



        public T AddInToTable(T obj)
        {
            table.Add(obj);
            _context.SaveChanges();
            return obj;
        }
    }
}
