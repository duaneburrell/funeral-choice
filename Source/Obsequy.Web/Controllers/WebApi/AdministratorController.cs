using Obsequy.Data.Contracts;
using Obsequy.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Obsequy.Security;
using Obsequy.Utility;
using System.Threading.Tasks;

namespace Obsequy.Web
{
	[InitializeSimpleMembership]
	[AuthorizationFilter(AccountType.Administrator)]
    public class AdministratorController : BaseApiController
	{
		#region Properties

		#region Authentication Security
		private AuthenticationSecurity _authenticationSecurity;
		protected AuthenticationSecurity AuthenticationSecurity
		{
			get { return (_authenticationSecurity ?? (_authenticationSecurity = new AuthenticationSecurity() { Uow = this.Uow })); }
		}
		#endregion

		#endregion

		#region Constuction
		public AdministratorController(IObsequyUow uow)
			: base(uow)
		{
		}
		#endregion

		#region Get Administrator Member
		[ActionName("member")]
		[HttpGet]
		public HttpResponseMessage GetAdministratorMember(string id)
		{
			try
			{
				// get all administrator member
				var results = this.Uow.Administrators.FindById(id);

				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve administrator member {0} via user Id {1}", id, this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Administrator Members
		[ActionName("members")]
		[HttpGet]
		public HttpResponseMessage GetAdministratorMembers()
		{
			try
			{
				// get all administrators
				var results = this.Uow.Administrators.GetMembers();

				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve administrator members via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Create Administrator Member
		[ActionName("member")]
		[HttpPost]
		public HttpResponseMessage CreateAdministratorMember(AdministratorRegistrationForm registrationForm)
		{
			var formValidation = registrationForm.Validate(this.AccountSession, ValidationMode.Create);

			if (formValidation.IsValid)
			{
				try
				{
					// create the account
					var member = this.AuthenticationSecurity.CreateMember(registrationForm);

					// add appropriate roles to the account
					var roles = new string[2] { Definitions.Account.Roles.Elevated, Definitions.Account.Roles.Administrator };

					this.AuthenticationSecurity.AddRolesToUser(registrationForm.Member.Email, roles);

					// get the updated administrator member
					var results = this.Uow.Administrators.GetById(member.Id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to create administrator member via user {0}", this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}
		#endregion

		#region Update Administrator Member
		[ActionName("member")]
		[HttpPut]
		public HttpResponseMessage UpdateAdministratorMember(AdminMember adminMember)
		{
			var modelValidation = adminMember.Validate(this.AccountSession, ValidationMode.Update);

			if (modelValidation.IsValid)
			{
				try
				{
					// update the account
					var results = this.AuthenticationSecurity.UpdateMember(adminMember);

					// request successful
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					Logger.Error(string.Format("Exception detected attempting to update administration member {0} via user Id {1}", adminMember.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Delete Administrator Member
		[ActionName("deletemember")]
		[HttpPost]
		public HttpResponseMessage DeleteAdministratorMember(AdminMember adminMember)
		{
			var modelValidation = adminMember.Validate(this.AccountSession, ValidationMode.Delete);

			if (modelValidation.IsValid)
			{
				try
				{
					// delete the administrator member account
					this.AuthenticationSecurity.DeleteMember(adminMember);

					// request successful
					return CreateSuccessResponse(new { success = true }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					Logger.Error(string.Format("Exception detected attempting to delete administration member {0} via user Id {1}", adminMember.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Get Providers
		[ActionName("providers")]
		[HttpGet]
		public HttpResponseMessage GetProviders()
		{
			try
			{
				// get all providers
				var results = this.Uow.ProviderPortfolios.GetProviderPortfolios();

				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve providers via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Pending Providers
		[ActionName("pendingproviders")]
		[HttpGet]
		public HttpResponseMessage GetPendingProviders()
		{
			try
			{
				// get all providers with a pending status
				var results = this.Uow.ProviderPortfolios.GetPendingProviderPortfolios();
				
				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve pending providers via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Active Providers
		[ActionName("activeproviders")]
		[HttpGet]
		public HttpResponseMessage GetActiveProviders()
		{
			try
			{
				// get all providers with a pending status
				var results = this.Uow.ProviderPortfolios.GetActiveProviderPortfolios();
				
				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve active providers via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Rejected Providers
		[ActionName("rejectedproviders")]
		[HttpGet]
		public HttpResponseMessage GetRejectedProviders()
		{
			try
			{
				// get all providers with a pending status
				var results = this.Uow.ProviderPortfolios.GetRejectedProviderGroups();

				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve rejected providers via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Suspended Providers
		[ActionName("suspendedproviders")]
		[HttpGet]
		public HttpResponseMessage GetSuspendedProviders()
		{
			try
			{
				// get all providers with a pending status
				var results = this.Uow.ProviderPortfolios.GetSuspendedProviderPortfolios();

				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve suspended providers via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Update Provider Account Status
		[ActionName("updateprovideraccountstatus")]
		[HttpPut]
		public HttpResponseMessage UpdateProviderGroupAccountStatus(ProviderPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.State);

			if (modelValidation.IsValid)
			{
				try
				{
					// update the account status
					var results = this.Uow.ProviderPortfolios.UpdateAccountStatus(portfolio);

					// let the agent handle any additional processing
					this.Agent.ProviderPortfolioAccountStatusChanged(results);

					// request successful
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					Logger.Error(string.Format("Exception detected attempting to retrieve pending providers via user Id {0}", this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Get Consumer Portfolios
		[ActionName("consumerportfolios")]
		[HttpGet]
		public HttpResponseMessage GetConsumerPortfolios()
		{
			try
			{
				// get all consumer portfolios (not schemes)
				var results = this.Uow.ConsumerPortfolios.GetAll();

				// request successful
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve consumer portfolios via user Id {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Consumer Portfolio
		[ActionName("consumerportfolio")]
		[HttpGet]
		public HttpResponseMessage GetConsumerPortfolio(string id)
		{
			try
			{
				// get the consumer portfolio
				var portfolio = this.Uow.ConsumerPortfolios.GetScheme(id);

				// get all responses for the portfolio
				var responses = this.Uow.Responses.GetConsumerPortfolioResponseSchemes(id);

				// get all provider principal details
				foreach (var response in responses)
					response.Principal = this.Uow.ProviderPortfolios.FindById(response.ProviderPortfolioId).Principal;

				// request successful
				return CreateSuccessResponse(new { success = true, results = new { portfolio = portfolio, responses = responses } }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve consumer portfolio {0} via user Id {1}", id, this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Provider Portfolio
		[ActionName("providerportfolio")]
		[HttpGet]
		public HttpResponseMessage GetProviderPortfolio(string id)
		{
			try
			{
				// get the consumer portfolio
				var portfolio = this.Uow.ProviderPortfolios.GetScheme(id);

				// get all responses for the portfolio
				var responses = this.Uow.Responses.GetProviderPortfolioResponseSchemes(id);

				// get all consumer principal details
				foreach (var response in responses)
					response.Principal = this.Uow.ConsumerPortfolios.FindById(response.ConsumerPortfolioId).Principal;

				// request successful
				return CreateSuccessResponse(new { success = true, results = new { portfolio = portfolio, responses = responses } }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to retrieve consumer portfolio {0} via user Id {1}", id, this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

        #region Delete Consumer Portfolio
        [ActionName("deleteconsumerportfolio")]
        [HttpPost]
        public HttpResponseMessage DeleteConsumerPortfolio(string id)
        {
            try
            {
                // delete the consumer portfolio
                this.Uow.ConsumerPortfolios.DeletePortfolio(id);

                // request successful
                return CreateSuccessResponse(new { success = true }, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Exception detected attempting to delete consumer portfolio {0} via user Id {1}", id, this.AccountSession.MemberId), ex);

                return CreateErrorResponse(ex);
            }
        }
        #endregion

        #region Remind Consumer Portfolio
        [ActionName("remindconsumerportfolio")]
        [HttpPost]
        public HttpResponseMessage RemindConsumerPortfolio(string id)
        {
            try
            {
                // send a reminder to this portfolio
                this.Uow.ConsumerPortfolios.RemindPortfolio(id);

                // get updated portfolio scheme
                var results = this.Uow.ConsumerPortfolios.GetScheme(id);

                return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                // log exception
                Logger.Error(string.Format("Exception detected attempting to remind consumer portfolio {0} via user Id {1}", id, this.AccountSession.MemberId), ex);

                return CreateErrorResponse(ex);
            }
        }
        #endregion

		#region Test Payment
		[ActionName("testpayment")]
		[HttpPost]
		public async Task<HttpResponseMessage> TestPayment(Payment payment)
		{
			var modelValidation = payment.Validate(this.AccountSession, ValidationMode.Accept);

			if (modelValidation.IsValid)
			{
				try
				{
					// create the payment processor
					var paymentProcessor = new PaymentProcessor() { AccountSession = this.AccountSession, Logger = this.Logger };

					// wait until the payment is processed
					var result = await paymentProcessor.ProcessTestPaymentAsync(payment);

					if (result.IsSuccess())
					{
						// extract payment transaction data
						var transaction = paymentProcessor.CreatePaymentTransaction(result.Target);

						// return the updated results
						return CreateSuccessResponse(new { success = true, results = transaction }, HttpStatusCode.OK);
					}
					else
					{
						// generate an invalid payment validation response
						return CreateInvalidResponse(BraintreeValidator.AsInvalidPayment(result));
					}
				}
				catch (Exception ex)
				{
					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion
	}
}
