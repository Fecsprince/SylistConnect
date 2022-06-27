using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DbEFContext
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(): base("AppConnection") {}

        public DbSet<Employee> Employees { get; set; }
    }
}
