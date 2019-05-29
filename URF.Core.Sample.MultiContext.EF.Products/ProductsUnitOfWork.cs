using URF.Core.Sample.MultiContext.Abstractions;

namespace URF.Core.Sample.MultiContext.EF.Products
{
    public class ProductsUnitOfWork : UnitOfWork<ProductsDbContext>, IProductsUnitOfWork
    {
        public ProductsUnitOfWork(ProductsDbContext context, IRepository<Product, ProductsDbContext> repository) : base(context)
        {
            ProductsRepository = repository;
        }

        public IRepository<Product, ProductsDbContext> ProductsRepository { get; }
    }
}
