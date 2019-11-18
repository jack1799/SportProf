using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SportProf.Startup))]
namespace SportProf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
