using System;
using System.Diagnostics.Contracts;
using VendingMachineApp.Business.Events;

namespace VendingMachineApp.DataAccess.Core
{
    [ContractClass(typeof(TrackedEventCommitterContract<,>))]
    public interface ITrackedEventCommitter<in TEntity, in TEvent> 
        where TEntity : class
        where TEvent : IEvent
    {
        void Commit(TEntity entity, TEvent @event);
    }

    [ContractClass(typeof(SimpleEventCommitterContract<>))]
    public interface IEventCommitter<in TEvent> where TEvent : IEvent
    {
        void Commit(TEvent @event);
    }

    [ContractClassFor(typeof(ITrackedEventCommitter<,>))]
    public abstract class TrackedEventCommitterContract<TEntity, TEvent> : ITrackedEventCommitter<TEntity, TEvent> 
        where TEntity : class 
        where TEvent : IEvent
    {
        public void Commit(TEntity entity, TEvent @event)
        {
            Contract.Requires(entity != null);
            Contract.Requires(@event != null);
        }
    }

    [ContractClassFor(typeof(IEventCommitter<>))]
    public abstract class SimpleEventCommitterContract<TEvent> : IEventCommitter<TEvent> where TEvent : IEvent
    {
        public void Commit(TEvent @event)
        {
            Contract.Requires(@event != null);
        }
    }

}
