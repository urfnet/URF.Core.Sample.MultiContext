using URF.Core.Sample.MultiContext.Abstractions;

namespace URF.Core.Sample.MultiContext.EF.Products
{
    public interface IProductsUnitOfWork : IUnitOfWork<ProductsDbContext>
    {
        IRepository<Product, ProductsDbContext> ProductsRepository { get; }
    }
}