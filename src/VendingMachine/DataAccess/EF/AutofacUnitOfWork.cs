using System;
using Autofac;
using VendingMachineApp.DataAccess.Core;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class AutofacUnitOfWork<TEntity> : UnitOfWork<TEntity>
        where TEntity : class
    {
        private readonly ILifetimeScope _childScope;
        private bool _isDisposed;

        public AutofacUnitOfWork(ILifetimeScope parentScope)
        {
            _childScope = parentScope.BeginLifetimeScope();
        }

        public override TEntity Get<TIdentity>(TIdentity identity)
        {
            if (TrackedEntity == null)
            {
                TrackedEntity = _childScope.Resolve<IEntityFactory<TEntity, TIdentity>>().Create(identity);
            }

            //TODO: Define mapping
            //_mappedSnapshot = default(TSnapshot);

            return TrackedEntity;
        }

        public override void Commit<TEvent>(TEvent @event)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().AssemblyQualifiedName);
            }

            if (TrackedEntity != null)
            {
                var trackedEventCommitter = _childScope.Resolve<ITrackedEventCommitter<TEntity, TEvent>>();
                trackedEventCommitter.Commit(TrackedEntity, @event);
            }
            else
            {
                var simpleEventCommiter = _childScope.Resolve<IEventCommitter<TEvent>>();
                simpleEventCommiter.Commit(@event);
            }

            Dispose();
        }

        public override void Dispose()
        {
            if (_isDisposed) return;

            _childScope.Dispose();
            _isDisposed = true;
        }
    }
}
