using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Mvc;

namespace Obsequy.Web
{
	[Authorize]
	[InitializeSimpleMembership]
	public class SessionController : BaseController
	{
		#region Login
		//[AllowAnonymous]
		//[HttpPost]
		////[ValidateAntiForgeryToken]
		//public ActionResult Login(LoginForm form)
		//{
		//	if (!IsSupportedBrowser())
		//		return BrowserUpgrade();

		//	var formValidation = form.Validate();

		//	if (formValidation.IsValid)
		//	{
		//		// attempt to login the user
		//		try
		//		{
		//			if (WebSecurity.Login(form.Email, form.Password, persistCookie: form.RememberMe))
		//			{
		//				// can't redirect from here with SPA, notify the client to do it for us
		//				// note: the Home controller will know where to gdo, the client will just perform the action
		//				return Json(new { success = true, redirect = "/Home/Index" });
		//			}
		//			else
		//			{
		//				// force invalid password error
		//				return Json(new { invalid = true, results = FormatResults(form.AsInvalidPassword()) });
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			// If we got this far, something failed
		//			return Json(new { error = true, results = ex.Message });
		//		}
		//	}

		//	// invalidated by the model
		//	return Json(new { invalid = true, results = FormatResults(formValidation) });
		//}
		#endregion

		#region Logoff
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			//if (!IsSupportedBrowser())
			//	return BrowserUpgrade();

			//WebSecurity.Logout();

			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region Incoming Token
		[AllowAnonymous]
		[HttpGet]
		public ActionResult IncomingToken(string param)
		{
			//if (!IsSupportedBrowser())
			//	return BrowserUpgrade();

			//if (string.IsNullOrWhiteSpace(param))
			//{
			//	return new HttpNotFoundResult("The specified token is invalid");
			//}

			//ViewBag.Token = param;
			//ViewBag.HasError = false;
			//ViewBag.IsSuccess = false;
			//ViewBag.IsPasswordReset = true;

			return View();
		}

		#endregion

		#region Reset Password
		[AllowAnonymous]
		[HttpPost]
		public ActionResult ResetPassword()
		{
			//if (!IsSupportedBrowser())
			//	return BrowserUpgrade();

			//string p1 = Request.Form["Password1"].ToString();
			//string p2 = Request.Form["Password2"].ToString();
			//string token = Request.Form["token"].ToString();

			//if (!string.IsNullOrWhiteSpace(p1) && p1.Length >= 6 && string.Equals(p1, p2))
			//{
			//	if (WebSecurity.ResetPassword(token, p1))
			//	{
			//		ViewBag.Token = token;
			//		ViewBag.HasError = false;
			//		ViewBag.IsSuccess = true;
			//		ViewBag.IsPasswordReset = true;
			//		ViewBag.ErrorText = "Reset password failed!  Reset code has expiration of 24 hours.";
			//		return View("IncomingToken");

			//	}
			//	else
			//	{
			//		ViewBag.Token = token;
			//		ViewBag.HasError = true;
			//		ViewBag.IsSuccess = false;
			//		ViewBag.IsPasswordReset = true;
			//		ViewBag.ErrorText = "Reset password failed!  Reset code has expiration of 24 hours.";
			//		return View("IncomingToken");
			//	}
			//}
			//else
			//{
			//	ViewBag.Token = token;
			//	ViewBag.HasError = true;
			//	ViewBag.IsSuccess = false;
			//	ViewBag.IsPasswordReset = true;
			//	ViewBag.ErrorText = "The specified passwords were invalid.";
			//	return View("IncomingToken");
			//}

			return null;
		}

		#endregion
	}
}