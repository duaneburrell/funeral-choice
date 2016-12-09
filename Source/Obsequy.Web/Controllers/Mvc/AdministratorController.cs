using Obsequy.Utility;
using System.Web.Mvc;

namespace Obsequy.Web.Controllers
{
	[InitializeSimpleMembership]
	public class AdministratorController : BaseController
	{
		public ActionResult Index()
		{
			if (!IsSupportedBrowser())
				return RedirectBrowserUpgrade();

			if (!IsAuthorizedUser(AccountType.Administrator))
				return RedirectHome();

			return View();
		}
	}
}