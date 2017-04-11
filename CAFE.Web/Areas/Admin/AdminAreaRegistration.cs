using System.Web.Mvc;

namespace CAFE.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults:new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                namespaces: new []{ "CAFE.Web.Areas.Admin.Controllers" }
            );
        }
    }
}