using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Obsequy.Utility;

namespace Obsequy.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			// archive any existing log files
			ArchiveLog();

			//System.Diagnostics.Debugger.Break();
			AreaRegistration.RegisterAllAreas();

			// Tell WebApi to use our custom Ioc (Ninject)
			IocConfig.RegisterIoc(GlobalConfiguration.Configuration);

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			GlobalConfig.CustomizedConfig(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();
		}

		private void ArchiveLog()
		{
			var rootDirectory = Server.MapPath("/");
			var logDirectory = string.Format("{0}/Logs", rootDirectory);

			var logFile = string.Format("{0}/{1}{2}", rootDirectory, Definitions.Logger.FileName, Definitions.Logger.FileExt);
			var archiveFile = string.Format("{0}/{1}.{2}{3}", logDirectory, Definitions.Logger.FileName, DateTimeHelper.FileNameNow, Definitions.Logger.FileExt);

			// create the log directory if it doesn't exist
			if (!Directory.Exists(logDirectory))
				Directory.CreateDirectory(logDirectory);

			// archive the log file into the Logs directory
			if (File.Exists(logFile))
				File.Move(logFile, archiveFile);
		}
	}
}