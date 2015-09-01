using System;
using VendingMachineApp.Business.Events;

namespace VendingMachineApp.DataAccess.Core
{
    public abstract class UnitOfWork<TEntity> : IDisposable
        where TEntity : class
    {
        protected TEntity TrackedEntity { get; set; }

        public abstract TEntity Get<TIdentity>(TIdentity identity);

        public abstract void Commit<TEvent>(TEvent @event) where TEvent : IEvent;

        public abstract void Dispose();
    }
}