using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;

namespace VendingMachineApp.DataAccess.EF
{
    internal abstract class EntityFrameworkTrackedEventCommitter<TEntity, TEvent> :
        ITrackedEventCommitter<TEntity, TEvent>,
        IDisposable 
        where TEntity : class 
        where TEvent : IEvent
    {
        protected readonly DbContext DbContext;
        private bool _disposed;

        protected EntityFrameworkTrackedEventCommitter(DbContext dbContext)
        {
            Contract.Requires(dbContext != null);

            DbContext = dbContext;
        }

        public abstract void Commit(TEntity entity, TEvent @event);

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            DbContext.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        ~EntityFrameworkTrackedEventCommitter()
        {
            Dispose();
        }
    }

    internal abstract class EntityFrameworkSimpleEventCommitter<TEvent> :
        IEventCommitter<TEvent>,
        IDisposable where TEvent : IEvent
    {
        protected readonly DbContext DbContext;
        private bool _disposed;

        protected EntityFrameworkSimpleEventCommitter(DbContext dbContext)
        {
            Contract.Requires(dbContext != null);

            DbContext = dbContext;
        }

        public abstract void Commit(TEvent @event);

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            DbContext.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        ~EntityFrameworkSimpleEventCommitter()
        {
            Dispose();
        }
    }
}
