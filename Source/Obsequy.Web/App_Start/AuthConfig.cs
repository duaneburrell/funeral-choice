using System;
using System.Threading;
using Obsequy.Security;
using Obsequy.Utility;

namespace Obsequy.Web
{
	public class AuthConfig
	{
		private static SimpleMembershipInitializer _initializer;
		private static object _initializerLock = new object();
		private static bool _isInitialized;

		public static void RegisterAuth()
		{
			// Ensure ASP.NET Simple Membership is initialized only once per app start
			LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
		}

		private class SimpleMembershipInitializer
		{
			protected NLog.Logger Logger { get; set; }

			public SimpleMembershipInitializer()
			{
				Logger = NLog.LogManager.GetLogger(Definitions.Logger.Name);

				try
				{
					var authenticationSecurity = new AuthenticationSecurity();

					authenticationSecurity.InitializeDatabaseConnection();

					authenticationSecurity.CreateRole(Definitions.Account.Roles.Elevated);
					authenticationSecurity.CreateRole(Definitions.Account.Roles.Standard);

					authenticationSecurity.CreateRole(Definitions.Account.Roles.Administrator);
					authenticationSecurity.CreateRole(Definitions.Account.Roles.Consumer);
					authenticationSecurity.CreateRole(Definitions.Account.Roles.Provider);

					authenticationSecurity.CreateAdministratorAccount();

					Logger.Info("Simple Membership Initialized");
				}
				catch (Exception ex)
				{
					Logger.Error("Simple Membership Initializer exception detected: ", ex);

					throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
				}
			}
		}
	}
}