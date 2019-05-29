using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace URF.Core.Sample.MultiContext.EF.Products
{
    public class ProductsDbContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MsSqlLocalDb;initial catalog=ProductsDb;Integrated Security=True; MultipleActiveResultSets=True");
            return new ProductsDbContext(optionsBuilder.Options);
        }
    }
}
