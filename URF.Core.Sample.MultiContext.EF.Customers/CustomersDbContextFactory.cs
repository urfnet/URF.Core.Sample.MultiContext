using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace URF.Core.Sample.MultiContext.EF.Customers
{
    public class CustomersDbContextFactory : IDesignTimeDbContextFactory<CustomersDbContext>
    {
        public CustomersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomersDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MsSqlLocalDb;initial catalog=CustomersDb;Integrated Security=True; MultipleActiveResultSets=True");
            return new CustomersDbContext(optionsBuilder.Options);
        }
    }
}
