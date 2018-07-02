using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HIS.Startup))]
namespace HIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}