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


        public AppDbContext() : base("AppConnection") { }

        public DbSet<Shop> GetShops { get; set; }
        public DbSet<Service> GetServices { get; set; }
        public DbSet<Booking> GetBookings { get; set; }
        public DbSet<Category> GetCategories { get; set; }
        public DbSet<Product> GetProducts { get; set; }
        public DbSet<Order> GetOrders { get; set; }
    }
}
