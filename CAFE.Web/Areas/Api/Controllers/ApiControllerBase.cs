
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CAFE.Core.Security;
using Microsoft.AspNet.Identity;
using NLog;

namespace CAFE.Web.Areas.Api.Controllers
{
    /// <summary>
    /// Base API controller with predefined common behavior
    /// </summary>
    [Authorize]
    public abstract class ApiControllerBase : ApiController
    {
        private readonly ISecurityService _securityService;

        /// <summary>
        /// Default controller with dependencies
        /// </summary>
        /// <param name="logger">Common controller logger</param>
        /// <param name="securityService">Security service</param>
        protected ApiControllerBase(
            ILogger logger,
            ISecurityService securityService)
        {
            Logger = logger;
            _securityService = securityService;
        }

        /// <summary>
        /// Common controller logger
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Execute operation in catching scope
        /// </summary>
        /// <typeparam name="T">Type that operation get</typeparam>
        /// <param name="action">Main action of scope</param>
        /// <param name="onSuccess">Action which run when main operation is successfully</param>
        /// <param name="onFail">Action which run when main operation is failed</param>
        /// <param name="rethrowException">Indicate when need to rethrow catched exception</param>
        protected void Execute<T>(
            Func<T> action,
            Action<T> onSuccess,
            Action<Exception> onFail,
            bool rethrowException = false)
        {
            try
            {
                onSuccess(action());
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                if (rethrowException)
                    throw;
                onFail.Invoke(exception);
            }
            
        }

        /// <summary>
        /// Execute operation in catching scope with explicit value type
        /// </summary>
        /// <typeparam name="T">Returns type when success or fail occured</typeparam>
        /// <param name="action">Main action of scope</param>
        /// <param name="onSuccess">Action which run when main operation is successfully</param>
        /// <param name="onFail">Action which run when main operation is failed</param>
        /// <returns>Main action result</returns>
        protected T ExecuteExplicit<T>(
                 Action action,
                 Func<T> onSuccess,
                 Func<Exception, T> onFail)
        {
            try
            {
                action();
                return onSuccess();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return onFail.Invoke(exception);
            }
        }

        /// <summary>
        /// Returns current loggined user
        /// </summary>
        /// <returns>Current user</returns>
        protected User GetCurrentUser()
        {
            return _securityService.GetUserById(User.Identity.GetUserId());
        }

        /// <summary>
        /// Returns current loggined user asynchronously
        /// </summary>
        /// <returns>Current user</returns>
        protected async Task<User> GetCurrentUserAsync()
        {
            return await Task.Run(() => GetCurrentUser());
        }

        /// <summary>
        /// Throw common internal server error (500) with message
        /// </summary>
        /// <param name="message">User-friendly message</param>
        protected void ThrowInternalServerError(string message)
        {
            throw new HttpResponseException(new HttpResponseMessage
            {
                Content = new StringContent(message),
                StatusCode = HttpStatusCode.InternalServerError
            });
        }

        /// <summary>
        /// Returns internal server error (500) with message
        /// </summary>
        /// <param name="exception">Catched exception</param>
        /// <returns>Http error message</returns>
        protected IHttpActionResult Fail(Exception exception)
        {
            return InternalServerError(exception);
        }
    }
}