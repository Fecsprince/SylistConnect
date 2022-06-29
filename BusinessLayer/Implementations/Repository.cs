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

        public IEnumerable<T> GetAllRecords() 
        {
            return table.ToList();
        }

        public T GetRecordById(object id) 
        {
            //FIND RECORD FIRST
            var dbRec = table.Find(id);
            if (dbRec != null)
            {
                return dbRec;
            }
            else
            {
                return dbRec;
            }
        }

        public int RemoveFromTable(object id)
        {
           var rec = table.Find(id);
            if (rec != null)
            {
                table.Remove(rec);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public T Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
            return obj;
        }
    }
}
