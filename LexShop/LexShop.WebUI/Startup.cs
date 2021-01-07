using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LexShop.WebUI.Startup))]
namespace LexShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
