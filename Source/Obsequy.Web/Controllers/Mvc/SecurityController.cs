using System.Web.Mvc;
using WebMatrix.WebData;

namespace Obsequy.Web.Controllers
{
	[InitializeSimpleMembership]
	public class SecurityController : BaseController
	{
		#region Logoff
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			WebSecurity.Logout();

			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Upgrade
		public ActionResult Upgrade()
		{
			if (IsSupportedBrowser())
				return RedirectHome();

			return View();
		}
		#endregion
	}
}