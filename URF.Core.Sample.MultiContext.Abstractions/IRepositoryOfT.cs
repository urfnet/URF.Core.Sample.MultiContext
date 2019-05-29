using URF.Core.Abstractions;

namespace URF.Core.Sample.MultiContext.Abstractions
{
    public interface IRepository<TEntity, TDbContext> : IRepository<TEntity>
        where TEntity : class
    {
    }
}
