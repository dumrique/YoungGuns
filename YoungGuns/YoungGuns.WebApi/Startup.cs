using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YoungGuns.WebApi.Startup))]

namespace YoungGuns.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
