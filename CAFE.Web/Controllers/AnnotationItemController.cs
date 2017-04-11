using System;
using System.Web.Mvc;

namespace CAFE.Web.Controllers
{
    /// <summary>
    /// Anotation items view placeholder controller
    /// </summary>
    //[Authorize]
    public class AnnotationItemController : Controller
    {
        /// <summary>
        /// Returns main action
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index(Guid? id, Guid? cloningId, string filesId)
        {
            if (null == id && !User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.IsUserLoggedIn = User.Identity.IsAuthenticated;

            ViewBag.CloningId = cloningId;
            ViewBag.EditingId = id;
            ViewBag.FileIds = filesId;

            return View();
        }

        /// <summary>
        /// Returns main action
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult CreateFromTemplate(Guid[] filesId, Guid? cloningId)
        {
            var filesIdsString = string.Join(",", filesId);
            if (null != cloningId)
            {
                ViewBag.CloningId = cloningId;
                return RedirectToAction("Index", new { cloningId = cloningId, filesId = filesIdsString });
            }

            return RedirectToAction("Index", new { filesId = filesIdsString });
        }

        [Authorize]
        public ActionResult Clone(Guid id)
        {
            return RedirectToAction("Index", new { cloningId = id });
        }
    }
}