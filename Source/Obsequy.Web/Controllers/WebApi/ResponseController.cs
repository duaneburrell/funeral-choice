using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Security;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Obsequy.Utility;

namespace Obsequy.Web
{
    public class ResponseController : BaseApiController
    {
		#region Constuction
		public ResponseController(IObsequyUow uow)
			: base(uow)
		{
		}
		#endregion

		#region Get Payment Configuration
		[ActionName("paymentconfiguration")]
		[HttpGet]
		public HttpResponseMessage GetPaymentConfiguration()
		{
			try
			{
				// get the API key
				//var results = Security.BalancedConfiguration.ApiKey;
				var results = Security.BraintreeConfiguration.CseKey;

				// return the updated results
				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to get the Balanced payment keys via user {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Consumer Responses
		[ActionName("consumerresponses")]
		[AuthorizationFilter(AccountType.Consumer)]
		[HttpGet]
		public HttpResponseMessage GetConsumerResponses()
		{
			try
			{
				// get all consumer response schemes
				var results = this.Uow.Responses.GetConsumerMemberResponseSchemes(this.AccountSession.MemberId);

				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to get consumer responses via user {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Get Provider Responses
		[ActionName("providerresponses")]
		[AuthorizationFilter(AccountType.Provider)]
		[HttpGet]
		public HttpResponseMessage GetProviderResponses()
		{
			try
			{
				// get all provider response schemes
				var results = this.Uow.Responses.GetProviderMemberResponseSchemes(this.AccountSession.MemberId);

				return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				// log exception
				Logger.Error(string.Format("Exception detected attempting to get provider response schemes for via user {0}", this.AccountSession.MemberId), ex);

				return CreateErrorResponse(ex);
			}
		}
		#endregion

		#region Update Response As Accepted
		[ActionName("accepted")]
		[AuthorizationFilter(AccountType.Consumer)]
		[HttpPut]
		public async Task<HttpResponseMessage> UpdateResponseAsAccepted(ResponseForm responseForm)
		{
			var formValidation = responseForm.Validate(this.AccountSession, ValidationMode.Accept, responseForm);

			if (formValidation.IsValid)
			{
				try
				{
					// get the current response
					var response = this.Uow.Responses.FindById(responseForm.Id);

					// create the payment processor
					var paymentProcessor = new PaymentProcessor() { AccountSession = this.AccountSession, Logger = this.Logger };

					// wait until the payment is processed
					var result = await paymentProcessor.ProcessPaymentAsync(response, responseForm.Payment);

					if (result.IsSuccess())
					{
						// log invalid transaction
						this.Logger.Warn("Payment transaction succeeded for Id {0} via user {1}", responseForm.Id, this.AccountSession.MemberId);

						// extract payment transaction data
						var payment = paymentProcessor.CreatePaymentTransaction(result.Target);

						// handle the submission to the pending state
						var results = this.Uow.Responses.UpdateResponseAsAccepted(responseForm.Id, payment);

						// return the updated results
						return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
					}
					else
					{
						// log invalid transaction
						this.Logger.Warn("Payment transaction failed for Id {0} via user {1}: {2}", responseForm.Id, this.AccountSession.MemberId, result.Message);

						// generate an invalid payment validation response
						return CreateInvalidResponse(BraintreeValidator.AsInvalidPayment(result));
					}
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to set state to accepted for Id {0} via user {1}", responseForm.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(formValidation);
		}
		#endregion

		#region Update Response As Available
		[ActionName("available")]
		[AuthorizationFilter(AccountType.Provider)]
		[HttpPut]
		public HttpResponseMessage UpdateResponseAsAvailable(ProviderResponseScheme response)
		{
			var schemeValidation = response.Validate(this.AccountSession, ValidationMode.Available);

			if (schemeValidation.IsValid)
			{
				try
				{
					// handle the submission to the available state
					var results = this.Uow.Responses.UpdateResponseAsAvailable(response);

					// return the updated results
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to set state to available for Id {0} via user {1}", response.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(schemeValidation);
		}
		#endregion

		#region Update Response As Dismissed
		[ActionName("dismissed")]
		[AuthorizationFilter(AccountType.Provider)]
		[HttpPut]
		public HttpResponseMessage UpdateResponseAsDismissed(ProviderResponseScheme response)
		{
			var schemeValidation = response.Validate(this.AccountSession, ValidationMode.Dismiss);

			if (schemeValidation.IsValid)
			{
				try
				{
					// handle the submission to the dismissed state
					var results = this.Uow.Responses.UpdateResponseAsDismissed(response);

					// return the updated results
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to set state to dismissed for Id {0} via user {1}", response.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(schemeValidation);
		}
		#endregion

		#region Update Response As Pending
		[ActionName("pending")]
		[AuthorizationFilter(AccountType.Provider)]
		[HttpPut]
		public HttpResponseMessage UpdateResponseAsPending(ProviderResponseScheme response)
		{
			var schemeValidation = response.Validate(this.AccountSession, ValidationMode.Pending);

			if (schemeValidation.IsValid)
			{
				try
				{
					// handle the submission to the pending state
					var results = this.Uow.Responses.UpdateResponseAsPending(response);

					// return the updated results
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to set state to pending for Id {0} via user {1}", response.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(schemeValidation);
		}
		#endregion

		#region Update Response As Recalled
		[ActionName("recalled")]
		[AuthorizationFilter(AccountType.Provider)]
		[HttpPut]
		public HttpResponseMessage UpdateResponseAsRecalled(ProviderResponseScheme response)
		{
			var schemeValidation = response.Validate(this.AccountSession, ValidationMode.Recalled);

			if (schemeValidation.IsValid)
			{
				try
				{
					// handle the submission to the recalled state
					var results = this.Uow.Responses.UpdateResponseAsRecalled(response);

					// return the updated results
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to set state to recalled for Id {0} via user {1}", response.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(schemeValidation);
		}
		#endregion

		#region Update Response As Rejected
		[ActionName("rejected")]
		[AuthorizationFilter(AccountType.Consumer)]
		[HttpPut]
		public HttpResponseMessage UpdateResponseAsRejected(ConsumerResponseScheme response)
		{
			var schemeValidation = response.Validate(this.AccountSession, ValidationMode.Reject);

			if (schemeValidation.IsValid)
			{
				try
				{
					// handle the submission to the rejected state
					var results = this.Uow.Responses.UpdateResponseAsRejected(response);

					// return the updated results
					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to set state to rejected for Id {0} via user {1}", response.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(schemeValidation);
		}
		#endregion
	}
}