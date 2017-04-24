using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRUD_OnlineStore.Startup))]
namespace CRUD_OnlineStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
