using Obsequy.Utility;
using System.Web.Mvc;

namespace Obsequy.Web.Controllers
{
	[InitializeSimpleMembership]
	public class ConsumerController : BaseController
	{
		public ActionResult Index()
		{
			if (!IsSupportedBrowser())
				return RedirectBrowserUpgrade();

			if (!IsAuthorizedUser(AccountType.Consumer))
				return RedirectHome();

			return View();
		}
	}
}