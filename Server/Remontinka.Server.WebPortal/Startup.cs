using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Remontinka.Server.WebPortal.Startup))]
namespace Remontinka.Server.WebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
