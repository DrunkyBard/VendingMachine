using VendingMachineApp.Business.Events;

namespace VendingMachineApp.DataAccess.Core
{
    public interface ITrackedEventCommitter<in TEntity, in TEvent> where TEvent : IEvent
    {
        void Commit(TEntity entity, TEvent @event);
    }

    public interface IEventCommitter<in TEvent> where TEvent : IEvent
    {
        void Commit(TEvent @event);
    }
}
