using Microsoft.EntityFrameworkCore;
// Add the ContosoPets.Domain.Models using statement
using ContosoPets.Domain.Models;

namespace ContosoPets.DataAccess.Data
{
    public partial class ContosoPetsContext : DbContext
    {
        public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options)
            : base(options)
        {
        }

        // Add the DbSet<T> properties
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
    }
}
