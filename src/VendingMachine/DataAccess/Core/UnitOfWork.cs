using System;
using System.Diagnostics.Contracts;
using VendingMachineApp.Business.Events;

namespace VendingMachineApp.DataAccess.Core
{
    [ContractClass(typeof(UnitOfWorkContract<>))]
    public abstract class UnitOfWork<TEntity> : IDisposable
        where TEntity : class
    {
        protected TEntity TrackedEntity { get; set; }

        public abstract TEntity Get<TIdentity>(TIdentity identity);

        public abstract void Commit<TEvent>(TEvent @event) where TEvent : IEvent;

        public abstract void Dispose();
    }

    [ContractClassFor(typeof(UnitOfWork<>))]
    public abstract class UnitOfWorkContract<TEntity> : UnitOfWork<TEntity> where TEntity : class
    {
        public override TEntity Get<TIdentity>(TIdentity identity)
        {
            return default(TEntity);
        }

        public override void Commit<TEvent>(TEvent @event)
        {
            Contract.Requires(@event != null);
        }

        public override void Dispose()
        {}
    }
}