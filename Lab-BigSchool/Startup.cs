using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lab_BigSchool.Startup))]
namespace Lab_BigSchool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
