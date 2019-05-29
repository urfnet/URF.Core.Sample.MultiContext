using Microsoft.EntityFrameworkCore;

namespace URF.Core.Sample.MultiContext.EF.Products
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
