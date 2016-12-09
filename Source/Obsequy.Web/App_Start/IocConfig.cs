using System.Web.Http;
using Obsequy.Data;
using Obsequy.Data.Contracts;
using Obsequy.Utility;
using Ninject;

namespace Obsequy.Web
{
	public class IocConfig
	{
		public static void RegisterIoc(HttpConfiguration config)
		{
			var kernel = new StandardKernel(); // Ninject IoC

			// These registrations are "per instance request".
			// See http://blog.bobcravens.com/2010/03/ninject-life-cycle-management-or-scoping/

			kernel.Bind<RepositoryFactories>().To<RepositoryFactories>()
				.InSingletonScope();

			kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
			kernel.Bind<IObsequyUow>().To<ObsequyUow>();

			// Tell WebApi how to use our Ninject IoC
			config.DependencyResolver = new NinjectDependencyResolver(kernel);

			// log this registration
			LoggerHelper.Logger.Info(string.Format("IOC Configuration successful"));
		}
	}
}