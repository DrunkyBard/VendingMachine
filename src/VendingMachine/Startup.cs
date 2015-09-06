using Microsoft.Owin;
using Owin;
using VendingMachineApp;

[assembly: OwinStartup(typeof(Startup))]
namespace VendingMachineApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
