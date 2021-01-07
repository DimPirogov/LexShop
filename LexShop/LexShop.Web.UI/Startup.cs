using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LexShop.Web.UI.Startup))]
namespace LexShop.Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
