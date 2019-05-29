using URF.Core.Abstractions;

namespace URF.Core.Sample.MultiContext.Abstractions
{
    public interface IUnitOfWork<TDbContext> : IUnitOfWork
    {
    }
}
