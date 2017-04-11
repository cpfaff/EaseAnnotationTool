using System.Web.Http;
using CAFE.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CAFE.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureOAuth(app);

            app.UseWebApi(new HttpConfiguration());
        }
    }
}

//Enable-Migrations 
//Add-Migration init
//Update-Database