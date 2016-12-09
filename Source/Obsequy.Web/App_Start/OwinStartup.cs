using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Obsequy.Web.OwinStartup))]
namespace Obsequy.Web
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}