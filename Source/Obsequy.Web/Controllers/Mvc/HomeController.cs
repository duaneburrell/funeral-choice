using Obsequy.Security;
using System.Web.Mvc;

namespace Obsequy.Web.Controllers
{
	[InitializeSimpleMembership]
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			if (!IsSupportedBrowser())
				return RedirectBrowserUpgrade();

			if (IsAuthorizedUser())
				return RedirectAuthorizedUser();

			// make sure they are logged out
			AuthenticationSecurity.Logout();

			return View();
		}
	}
}