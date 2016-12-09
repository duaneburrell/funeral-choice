using Obsequy.Utility;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Obsequy.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.Clear();

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapHttpRoute(
				name: "AccountService",
				routeTemplate: "api/account/{action}/{id}",
				defaults: new { controller = "Account", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "AdministratorService",
				routeTemplate: "api/administrator/{action}/{id}",
				defaults: new { controller = "Administrator", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "ConsumerService",
				routeTemplate: "api/consumer/{action}/{id}",
				defaults: new { controller = "Consumer", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "ProviderService",
				routeTemplate: "api/provider/{action}/{id}",
				defaults: new { controller = "Provider", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "ResponseService",
				routeTemplate: "api/response/{action}/{id}",
				defaults: new { controller = "Response", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "SessionService",
				routeTemplate: "api/session/{action}/{id}",
				defaults: new { controller = "Session", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "ValidationService",
				routeTemplate: "api/validation/{action}/{id}",
				defaults: new { controller = "Validation", id = RouteParameter.Optional }
			);

			routes.MapRoute(
				name: "Administrator",
				url: "a/{action}/{id}",
				defaults: new { controller = "Administrator", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Consumer",
				url: "c/{action}/{id}",
				defaults: new { controller = "Consumer", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Provider",
				url: "p/{action}/{id}",
				defaults: new { controller = "Provider", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "SecurityUpgrade",
				url: string.Format("{0}/{1}", Definitions.Controllers.Security.Route.Server, Definitions.Controllers.Security.Views.Upgrade),
				defaults: new { controller = "Security", action = "Upgrade", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Security",
				url: "s/{action}/{id}",
				defaults: new { controller = "Security", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}