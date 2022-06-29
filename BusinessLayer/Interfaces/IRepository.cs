using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T AddInToTable(T obj);
        int RemoveFromTable(object id);
        T Update(T obj);
        T GetRecordById(object id);
        IEnumerable<T> GetAllRecords(); 
    }
}
