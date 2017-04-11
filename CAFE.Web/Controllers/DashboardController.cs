using CAFE.Core.Configuration;
using System.Web.Mvc;

namespace CAFE.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IConfigurationProvider _configurationProvider;
        public DashboardController(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public ActionResult Index()
        {
            var applicationConfiguration = _configurationProvider.Get<Core.Configuration.ApplicationConfiguration>();
            ViewBag.isUsersAcceptanceNeed = applicationConfiguration.IsUsersAcceptanceNeed;

            return View();
        }
    }
}