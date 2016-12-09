using Obsequy.Security;
using Obsequy.Utility;
using System.Web.Mvc;
using System;

namespace Obsequy.Web
{
	public class BaseController : Controller
	{
		#region Is Authorized User
		protected bool IsAuthorizedUser()
		{
			var accountType = AuthenticationSecurity.UserAccountType(this.User);

			if (accountType != AccountType.None)
				return true;

			return false;
		}

		protected bool IsAuthorizedUser(AccountType accountType)
		{
			if (AuthenticationSecurity.UserAccountType(this.User) == accountType)
				return true;

			return false;
		}
		#endregion

		#region Is Supported Browser
		protected bool IsSupportedBrowser()
		{
			var browser = Request.Browser.Browser.ToLower();
			var id = Request.Browser.Id;
			var type = Request.Browser.Type;
			var version = Request.Browser.Version;
			var majorVersion = Request.Browser.MajorVersion;

			var isIeNotOk = (browser.Equals("ie") || browser.Equals("internetexplorer")) && (majorVersion <= 6);

			//var isChromeOk = browser.Equals("chrome");
			//var isIeOk = (browser.Equals("ie") || browser.Equals("internetexplorer")) && (majorVersion >= 7);
			//var isSafariOk = browser.Equals("safari");
			//var isFirefoxOK = browser.Equals("firefox");
			//var isMozillaOK = browser.Equals("mozilla");
			//var isOperaOK = browser.Equals("opera");

			// is this browser unsupported?
			var isUnsupportedBrowser = isIeNotOk;
			if (isUnsupportedBrowser)
				Logger.Error(string.Format("Unsupported Browser Detected: Browser: {0}, ID: {1}, Type: {2}, Version: {3}, Major Version: {4}", browser, id, type, version, majorVersion), new Exception());

			// set the viewbag for UI control
			ViewBag.IsSupportedBrowser = !isUnsupportedBrowser;

			return !isUnsupportedBrowser;
		}
		#endregion

		#region Logger
		private NLog.Logger _logger;
		protected NLog.Logger Logger
		{
			get { return _logger ?? (_logger = NLog.LogManager.GetLogger(Definitions.Logger.Name)); }
		}
		#endregion

		#region Redirect Authorized User
		public ActionResult RedirectAuthorizedUser()
		{
			var accountType = AuthenticationSecurity.UserAccountType(this.User);

			if (accountType == AccountType.Administrator)
				return RedirectToAction(Definitions.Controllers.Administrator.Views.Index, Definitions.Controllers.Administrator.Route.Server);

			if (accountType == AccountType.Consumer)
				return RedirectToAction(Definitions.Controllers.Consumer.Views.Index, Definitions.Controllers.Consumer.Route.Server);

			if (accountType == AccountType.Provider)
				return RedirectToAction(Definitions.Controllers.Provider.Views.Index, Definitions.Controllers.Provider.Route.Server);

			return null;
		}
		#endregion

		#region Redirect Home
		public ActionResult RedirectHome()
		{
			// redirect to the home controller
			return RedirectToAction(Definitions.Controllers.Home.Views.Index, Definitions.Controllers.Home.Route.Server);
		}
		#endregion

		#region Redirect Browser Upgrade
		public ActionResult RedirectBrowserUpgrade()
		{
			// redirect to the browser upgrade
			return RedirectToAction(Definitions.Controllers.Security.Views.Upgrade, Definitions.Controllers.Security.Route.Server);
		}
		#endregion
	}
}