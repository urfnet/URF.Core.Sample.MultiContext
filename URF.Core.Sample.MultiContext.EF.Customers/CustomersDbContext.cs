using Microsoft.EntityFrameworkCore;

namespace URF.Core.Sample.MultiContext.EF.Customers
{
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
    }
}
