using URF.Core.Sample.MultiContext.Abstractions;

namespace URF.Core.Sample.MultiContext.EF.Customers
{
    public class CustomersUnitOfWork : UnitOfWork<CustomersDbContext>, ICustomersUnitOfWork
    {
        public CustomersUnitOfWork(CustomersDbContext context, IRepository<Customer, CustomersDbContext> repository) : base(context)
        {
            CustomersRepository = repository;
        }

        public IRepository<Customer, CustomersDbContext> CustomersRepository { get; }
    }
}
