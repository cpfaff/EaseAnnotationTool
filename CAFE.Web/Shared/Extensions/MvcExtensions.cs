using System.Web.Mvc;
using System.Web.Routing;

namespace CAFE.Web.Shared.Extensions
{
	/// <summary>
	/// Extensions for System.Web.Mvc types
	/// Mainly used to eliminate magic strings in frequently used URL-generation method's arguments
	/// </summary>
	public static class MvcExtensions
	{

		/// <summary>
		/// Returns the fully qualified URL to an action method by using specified action name
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="controller"></param>
		/// <param name="actionName">Action name. It is recommended to use the "nameof(ACTION_NAME)" construct here</param>
		/// <returns>The fully qualified URL to an action method.</returns>
		public static string ActionUrl<T>(this Controller controller, string actionName) where T : Controller
		{
			var fullTypeName = typeof(T).Name;
			var controllerName = TruncateControllerTokenEnding(fullTypeName);
			return controller.Url.Action(actionName, controllerName);
		}

		/// <summary>
		/// Returns the fully qualified URL to an action method by using specified action name and route values
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="controller"></param>
		/// <param name="actionName">Action name. It is recommended to use the "nameof(ACTION_NAME)" construct here</param>
		/// <param name="routeValues">Route values</param>
		/// <returns>The fully qualified URL to an action method.</returns>
		public static string ActionUrl<T>(this Controller controller, string actionName, RouteValueDictionary routeValues)
		{
			var fullTypeName = typeof(T).Name;
			var controllerName = TruncateControllerTokenEnding(fullTypeName);
			return controller.Url.Action(actionName, controllerName, routeValues);
		}

		/// <summary>
		/// Returns the fully qualified URL to an action method by using specified action name and route values
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="controller"></param>
		/// <param name="actionName">Action name. It is recommended to use the "nameof(ACTION_NAME)" construct here</param>
		/// <param name="routeValues">Route values</param>
		/// <returns>The fully qualified URL to an action method.</returns>
		public static string ActionUrl<T>(this Controller controller, string actionName, object routeValues)
		{
			var fullTypeName = typeof(T).Name;
			var controllerName = TruncateControllerTokenEnding(fullTypeName);
			return controller.Url.Action(actionName, controllerName, routeValues);
		}

		/// <summary>
		/// Returns a fully qualified URL for an action method by using the specified action name,
		/// route values, and protocol to use.
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="controller"></param>
		/// <param name="actionName">Action name. It is recommended to use the "nameof(ACTION_NAME)" construct here</param>
		/// <param name="routeValues">Route values</param>
		/// <param name="protocol">The protocol for the URL, such as "http" or "https"</param>
		/// <returns></returns>
		public static string ActionUrl<T>(this Controller controller, string actionName, RouteValueDictionary routeValues, string protocol)
		{
			var fullTypeName = typeof(T).Name;
			var controllerName = TruncateControllerTokenEnding(fullTypeName);
			return controller.Url.Action(actionName, controllerName, routeValues, protocol);
		}

		/// <summary>
		/// Returns a fully qualified URL for an action method by using the specified action name,
		/// route values, and protocol to use.
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="controller"></param>
		/// <param name="actionName">Action name. It is recommended to use the "nameof(ACTION_NAME)" construct here</param>
		/// <param name="routeValues">Route values</param>
		/// <param name="protocol">The protocol for the URL, such as "http" or "https"</param>
		/// <returns></returns>
		public static string ActionUrl<T>(this Controller controller, string actionName, object routeValues, string protocol)
		{
			var fullTypeName = typeof(T).Name;
			var controllerName = TruncateControllerTokenEnding(fullTypeName);
			return controller.Url.Action(actionName, controllerName, routeValues, protocol);
		}

		/// <summary>
		/// Returns the Controller name without the "Controller" token at the end
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="controller"></param>
		/// <returns>
		/// The original Type Name with truncated "Controller" ending
		/// or the original Type Name if the token was not found at the end
		/// </returns>
		public static string ControllerNameForRouting<T>(this Controller controller)
		{
			var fullTypeName = typeof(T).Name;
			return TruncateControllerTokenEnding(fullTypeName);
		}

		/// <summary>
		/// Returns the Controller name without the "Controller" token at the end
		/// </summary>
		/// <typeparam name="T">Controller type</typeparam>
		/// <param name="wvp"></param>
		/// <returns>
		/// The original Type Name with truncated "Controller" ending
		/// or the original Type Name if the token was not found at the end
		/// </returns>
		public static string ControllerNameForRouting<T>(this WebViewPage wvp)
		{
			var fullTypeName = typeof(T).Name;
			return TruncateControllerTokenEnding(fullTypeName);
		}

		/// <summary>
		/// Removes the token "Controller" at the end of the string
		/// </summary>
		/// <param name="stringToTruncate">string to remove the token from</param>
		/// <returns>
		/// The original string with truncated "Controller" ending
		/// or the original string if the token was not found at the end of the string
		/// </returns>
		private static string TruncateControllerTokenEnding(string stringToTruncate)
		{
			var controllerName = stringToTruncate.EndsWith("Controller")
				? stringToTruncate.Substring(0, stringToTruncate.Length - 10)
				: stringToTruncate;
			return controllerName;
		}

	}
}