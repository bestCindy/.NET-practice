using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Coach.Startup))]
namespace Coach
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
