using System.Data.Entity;
using System.Diagnostics.Contracts;
using VendingMachineApp.DataAccess.Core;

namespace VendingMachineApp.DataAccess.EF
{
    internal abstract class EfContextEntityFactory<TContext, TEntity, TIdentity> : IEntityFactory<TEntity, TIdentity> 
        where TContext : DbContext
    {
        protected readonly TContext DbContext;

        public EfContextEntityFactory(TContext dbContext)
        {
            Contract.Requires(dbContext != null);

            DbContext = dbContext;
        }

        public abstract TEntity Create(TIdentity identity);
    }
}
