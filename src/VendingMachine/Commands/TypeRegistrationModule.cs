using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace VendingMachineApp.Commands
{
    public sealed class TypeRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsClosedTypeOf(typeof (VendingMachineCommandHandler<,>)))
                .AsSelf()
                .InstancePerDependency();
        }
    }
}