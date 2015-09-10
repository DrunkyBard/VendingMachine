using System.Data.Entity;
using System.Reflection;
using Autofac;
using VendingMachineApp.Business.Events;
using VendingMachineApp.DataAccess.Core;
using VendingMachineApp.DataAccess.EF;
using VendingMachineApp.DataAccess.Entities;
using Module = Autofac.Module;

namespace VendingMachineApp.DataAccess
{
    public sealed class TypeRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<VendingMachineDbContext>()
                .As<DbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof (EntityFrameworkUnitOfWork<>))
                .As(typeof (UnitOfWork<>))
                .InstancePerDependency();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsClosedTypeOf(typeof (IEntityFactory<,>)))
                .AsImplementedInterfaces()
                .InstancePerDependency();
            builder.RegisterType<CoinsRefundedEventCommitter>()
                .As(typeof (EntityFrameworkTrackedEventCommitter<UserVendingMachineAggregationEntity, CoinsRefundedEvent>))
                .InstancePerDependency();
            builder.RegisterType<GoodsBuyedEventCommitter>()
                .As(typeof (EntityFrameworkTrackedEventCommitter<UserVendingMachineAggregationEntity, GoodsBuyedEvent>))
                .InstancePerDependency();
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //    .Where(x => x.IsClosedTypeOf(typeof (ITrackedEventCommitter<,>)))
            //    .As(typeof(EntityFrameworkTrackedEventCommitter<,>))
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();
        }
    }
}
