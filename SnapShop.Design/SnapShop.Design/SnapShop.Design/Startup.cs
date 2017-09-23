using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SnapShop.Design.Startup))]
namespace SnapShop.Design
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
