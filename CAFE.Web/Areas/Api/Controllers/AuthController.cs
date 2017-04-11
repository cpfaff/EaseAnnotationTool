using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CAFE.Web.Areas.Api.Controllers
{
    [Authorize]
    public class AuthController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]        
        public HttpResponseMessage Login()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}