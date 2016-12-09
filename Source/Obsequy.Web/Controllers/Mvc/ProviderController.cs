using Obsequy.Utility;
using System.Web.Mvc;

namespace Obsequy.Web.Controllers
{
	[InitializeSimpleMembership]
	public class ProviderController : BaseController
	{
		public ActionResult Index()
		{
			if (!IsSupportedBrowser())
				return RedirectBrowserUpgrade();

			if (!IsAuthorizedUser(AccountType.Provider))
				return RedirectHome();

			return View();
		}
	}
}