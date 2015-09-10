using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Module = Autofac.Module;

namespace VendingMachineApp
{
    public sealed class CompositionRoot : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule<DataAccess.TypeRegistrationModule>();
            builder.RegisterModule<Commands.TypeRegistrationModule>();
        }
    }
}
