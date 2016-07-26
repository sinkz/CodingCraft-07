using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodingCraft_07.Startup))]
namespace CodingCraft_07
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
