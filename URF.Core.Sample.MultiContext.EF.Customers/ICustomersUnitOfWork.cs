using URF.Core.Sample.MultiContext.Abstractions;

namespace URF.Core.Sample.MultiContext.EF.Customers
{
    public interface ICustomersUnitOfWork : IUnitOfWork<CustomersDbContext>
    {
        IRepository<Customer, CustomersDbContext> CustomersRepository { get; }
    }
}