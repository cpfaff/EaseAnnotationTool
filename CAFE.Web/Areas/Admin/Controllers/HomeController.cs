
using System.Web.Mvc;
using CAFE.Core.Configuration;
using CAFE.Core.Misc;
using CAFE.Web.Areas.Admin.Models.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAFE.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.AdminRoleName)]
    public class HomeController : Controller
    {
        private readonly IConfigurationProvider _configurationProvider;
        public HomeController(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            var appConfiguration = _configurationProvider.Get<ApplicationConfiguration>();
            model.ShowAcceptance = appConfiguration.IsUsersAcceptanceNeed;

            model.ShowNotifyAboutNewVersion = VersionChecker.Instance.NeedToNotify();

            return View(model);
        }
    }
}