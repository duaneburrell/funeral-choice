using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Obsequy.Model;
using Obsequy.Utility;

namespace Obsequy.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class AuthorizationFilter : AuthorizationFilterAttribute
	{
		#region Fields

		private List<AccountType> _accountTypes = new List<AccountType>();
		
		#endregion

		#region Constructors
		
		public AuthorizationFilter(AccountType accountType)
		{
			_accountTypes.Add(accountType);
		}

		public AuthorizationFilter(List<AccountType> accountTypes)
		{
			_accountTypes.AddRange(accountTypes);
		}
		#endregion

		#region On Authorization
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if (_accountTypes.Any())
			{
				// verify that the current account type matches one in the list
				var accountSession = (actionContext.ControllerContext.Controller as BaseApiController).AccountSession;

				if (!_accountTypes.Contains(accountSession.AccountType))
				{
					// create unauthorized response
					actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized);
					return;
				}
			}

			base.OnAuthorization(actionContext);
		}
		#endregion
	}
}
