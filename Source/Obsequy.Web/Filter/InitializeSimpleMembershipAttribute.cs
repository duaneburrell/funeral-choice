using Obsequy.Security;
using Obsequy.Utility;
using System;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Obsequy.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
			// Note: this is now done at start up instead of when the controller is invoked
            //LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }
    }
}
