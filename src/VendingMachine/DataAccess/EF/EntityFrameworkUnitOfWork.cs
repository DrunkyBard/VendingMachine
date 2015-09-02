using System;
using System.Diagnostics.Contracts;
using Autofac;
using VendingMachineApp.DataAccess.Core;

namespace VendingMachineApp.DataAccess.EF
{
    internal sealed class EntityFrameworkUnitOfWork<TEntity> : UnitOfWork<TEntity>
        where TEntity : class
    {
        private readonly ILifetimeScope _childScope;
        private bool _isDisposed;
        private IDisposable _eventCommitter;

        public EntityFrameworkUnitOfWork(ILifetimeScope parentScope)
        {
            Contract.Requires(parentScope != null);

            _childScope = parentScope.BeginLifetimeScope();
        }

        public override TEntity Get<TIdentity>(TIdentity identity)
        {
            return TrackedEntity ?? _childScope.Resolve<IEntityFactory<TEntity, TIdentity>>().Create(identity);
        }

        public override void Commit<TEvent>(TEvent @event)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().AssemblyQualifiedName);
            }

            if (TrackedEntity != null)
            {
                var trackedEventCommitter = _childScope.Resolve<EntityFrameworkTrackedEventCommitter<TEntity, TEvent>>();
                _eventCommitter = trackedEventCommitter;
                trackedEventCommitter.Commit(TrackedEntity, @event);
            }
            else
            {
                var simpleEventCommiter = _childScope.Resolve<EntityFrameworkSimpleEventCommitter<TEvent>>();
                _eventCommitter = simpleEventCommiter;
                simpleEventCommiter.Commit(@event);
            }

            Dispose();
        }

        public override void Dispose()
        {
            if (_isDisposed) return;

            _eventCommitter.Dispose();
            _childScope.Dispose();
            _isDisposed = true;
        }
    }
}
