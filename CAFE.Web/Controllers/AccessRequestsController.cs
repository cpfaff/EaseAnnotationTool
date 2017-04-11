using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAFE.Web.Controllers
{
    [Authorize]
    public class AccessRequestsController : Controller
    {
        // GET: AccessRequests
        public ActionResult Incoming()
        {
            return View();
        }

        public ActionResult Outgoing()
        {
            return View();
        }
    }
}