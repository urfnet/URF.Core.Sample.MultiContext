using Microsoft.EntityFrameworkCore;
using URF.Core.EF;

namespace URF.Core.Sample.MultiContext.Abstractions
{
    public class Repository<TEntity, TDbContext> : Repository<TEntity>, IRepository<TEntity, TDbContext>
        where TDbContext : DbContext
        where TEntity : class
    {
        public Repository(TDbContext context) : base(context)
        {
        }
    }
}
