using Microsoft.EntityFrameworkCore;
using URF.Core.EF;

namespace URF.Core.Sample.MultiContext.Abstractions
{
    public class UnitOfWork<TDbContext> : UnitOfWork, IUnitOfWork<TDbContext>
        where TDbContext : DbContext
    {
        public UnitOfWork(TDbContext context) : base(context)
        {
        }
    }
}
