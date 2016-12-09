using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Obsequy.Web
{
	[AuthorizationFilter(AccountType.Provider)]
    public class ProviderController : BaseApiController
    {
		#region Constuction
		public ProviderController(IObsequyUow uow)
			: base(uow)
		{
		}
		#endregion

		#region Update Member
		[ActionName("provider")]
		[HttpPut]
		public HttpResponseMessage UpdateMember(ProviderMember member)
		{
			var modelValidation = member.Validate(this.AccountSession, ValidationMode.Update);

			if (modelValidation.IsValid)
			{
				try
				{
					// update settings
					var consumer = this.Uow.ProviderMembers.UpdateMember(member);

					// get updated entity
					var results = this.Uow.ProviderMembers.FindById(member.Id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to update provider for Id {0} via user {1}", member.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Get Provider Member
		[ActionName("provider")]
		[HttpGet]
		public HttpResponseMessage GetProvider()
		{
			try
			{
				var results = this.Uow.ProviderMembers.FindById(this.AccountSession.MemberId);

				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to get provider via user {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Create Provider Portfolio
		[ActionName("portfolio")]
		[HttpPost]
		public HttpResponseMessage CreateProviderPortfolio(ProviderPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Create);

			if (modelValidation.IsValid)
			{
				try
				{
					// create the provider portfolio for this principal
					var portfolioTemp = this.Uow.ProviderPortfolios.CreatePortfolio(portfolio);

					// get the updated provider member (with new provider portfolio)
					var results = this.Uow.ProviderMembers.GetScheme(this.AccountSession.MemberId);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to create provider portfolio via user {0}", this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Update Provider Portfolio
		[ActionName("portfolio")]
		[HttpPut]
		public HttpResponseMessage UpdateProviderPortfolio(ProviderPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Update);

			if (modelValidation.IsValid)
			{
				try
				{
					// update the provider portfolio
					this.Uow.ProviderPortfolios.UpdatePortfolio(portfolio);

					// get updated portfolio scheme
					var results = this.Uow.ProviderPortfolios.GetScheme(portfolio.Id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to update provider portfolio via user {0}", this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion
	}
}