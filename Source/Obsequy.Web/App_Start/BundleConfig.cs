using System.Web;
using System.Web.Optimization;

namespace Obsequy.Web
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/lib").Include(
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/braintree.js",
						"~/Scripts/bootstrap.js",
						"~/Scripts/moment.js",
						"~/Scripts/underscore.js",
						"~/Scripts/spin.js"));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
						"~/Scripts/angular.js",
						"~/Scripts/angular-route.js",
						"~/Scripts/angular-ui/ui-bootstrap.js",
						"~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

			bundles.Add(new ScriptBundle("~/bundles/angularApp").Include(
						"~/App/app.js",
                        "~/App/config.js",
                        "~/App/Controllers/adminController.js",
						"~/App/Controllers/consumerController.js",
						"~/App/Controllers/homeController.js",
						"~/App/Controllers/providerController.js",

						"~/App/Directives/applicationDirectives.js",
						"~/App/Directives/consumerDirectives.js",
						"~/App/Directives/providerDirectives.js",
						"~/App/Directives/utilityDirectives.js",

						"~/App/Forms/administratorRegistrationForm.js",
						"~/App/Forms/consumerRegistrationForm.js",
						"~/App/Forms/loginForm.js",
						"~/App/Forms/passwordRecoveryForm.js",
						"~/App/Forms/providerRegistrationForm.js",
						"~/App/Forms/responseForm.js",

						"~/App/Models/addressModel.js",
						"~/App/Models/adminMemberModel.js",
						"~/App/Models/changeReceiptModel.js",
						"~/App/Models/consumerMemberModel.js",
						"~/App/Models/consumerPreferenceModel.js",
						"~/App/Models/consumerPrincipalModel.js",
						"~/App/Models/consumerRequestModel.js",
						"~/App/Models/consumerScheduleModel.js",
						"~/App/Models/paymentModel.js",
						"~/App/Models/providerMemberModel.js",
						"~/App/Models/providerPrincipalModel.js",
						"~/App/Models/providerProfileModel.js",
						"~/App/Models/requestReceiptModel.js",
						"~/App/Models/responseAgreementModel.js",
						"~/App/Models/responseAlternateModel.js",
						"~/App/Models/responseModel.js",
						"~/App/Models/responseReceiptModel.js",

						"~/App/Schemes/consumerPortfolioScheme.js",
						"~/App/Schemes/consumerResponseScheme.js",
						"~/App/Schemes/providerPortfolioScheme.js",
						"~/App/Schemes/providerResponseScheme.js",
						
						"~/App/Services/apiService.js",
						"~/App/Services/enumService.js",
						"~/App/Services/libraryService.js",
						"~/App/Services/repositoryService.js",
						"~/App/Services/utilService.js",

						"~/App/Xtra/busyXtra.js",
						"~/App/Xtra/profilesXtra.js",
						"~/App/Xtra/profilerXtra.js"));

			bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
						"~/Scripts/jquery.signalR-{version}.js",
						"~/Scripts/signalR-server-generated.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			//bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/obsequy/css/app.css"));
		}
	}
}