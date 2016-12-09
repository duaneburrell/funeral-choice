using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Obsequy.Web
{
	[AuthorizationFilter(AccountType.Consumer)]
    public class ConsumerController : BaseApiController
    {
		#region Constuction
		public ConsumerController(IObsequyUow uow)
			: base (uow)
		{
		}
		#endregion

		#region Update Member
		[ActionName("consumer")]
		[HttpPut]
		public HttpResponseMessage UpdateMember(ConsumerMember member)
		{
			var modelValidation = member.Validate(this.AccountSession, ValidationMode.Update);

			if (modelValidation.IsValid)
			{
				try
				{
					// update settings
					var consumer = this.Uow.ConsumerMembers.UpdateMember(member);

					// get updated entity
					var results = this.Uow.ConsumerMembers.FindById(member.Id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to update consumer for Id {0} via user {1}", member.Id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Create Portfolio
		[ActionName("portfolio")]
		[HttpPost]
		public HttpResponseMessage CreatePortfolio(ConsumerPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Create);

			if (modelValidation.IsValid)
			{
				try
				{
					// create the consumer portfolio for this portfolio
					var consumerPortfolio = this.Uow.ConsumerPortfolios.CreatePortfolio(portfolio);

					// get the updated consumer member (with new consumer portfolio)
					var results = this.Uow.ConsumerMembers.GetScheme(this.AccountSession.MemberId);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to create consumer principal via user {0}", this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Update Portfolio
		[ActionName("portfolio")]
		[HttpPut]
		public HttpResponseMessage UpdatePortfolio(ConsumerPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Update);

			if (modelValidation.IsValid)
			{
				// portfolio ID
				var id = portfolio.Id;

				try
				{
					// create the consumer group for this portfolio
					this.Uow.ConsumerPortfolios.UpdatePortfolio(portfolio);

					// get updated portfolio scheme
					var results = this.Uow.ConsumerPortfolios.GetScheme(id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to update consumer portfolio {0} via user {1}", id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Delete Portfolio
		[ActionName("deleteportfolio")]
		[HttpPost]
		public HttpResponseMessage DeletePortfolio(ConsumerPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Delete);

			if (modelValidation.IsValid)
			{
				// portfolio ID
				var id = portfolio.Id;

				try
				{
					// delete the specified portfolio
					this.Uow.ConsumerPortfolios.DeletePortfolio(id);

					// get updated member (get the member since it will have the updated portfolio list)
					var results = this.Uow.ConsumerMembers.GetScheme(this.AccountSession.MemberId);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to delete consumer portfolio {0} via user {1}", id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Update Request As Draft
		[ActionName("updaterequestasdraft")]
		[HttpPut]
		public HttpResponseMessage UpdateRequestAsDraft(ConsumerPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Recalled);

			if (modelValidation.IsValid)
			{
				// portfolio ID
				var id = portfolio.Id;

				try
				{
					// update request state to draft
					var draftPortfolio = this.Uow.ConsumerPortfolios.UpdatePortfolioAsDraft(id);

					// let the agent handle processing of the request state change
					this.Agent.ConsumerRequestSetToDraft(draftPortfolio);

					// get updated portfolio scheme
					var results = this.Uow.ConsumerPortfolios.GetScheme(id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to update consumer portfolio request state to {0} for Id {1} via user {2}", RequestReceiptStates.Draft, id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion

		#region Update Request As Pending
		[ActionName("updaterequestaspending")]
		[HttpPut]
		public HttpResponseMessage UpdateRequestAsPending(ConsumerPortfolio portfolio)
		{
			var modelValidation = portfolio.Validate(this.AccountSession, ValidationMode.Pending);

			if (modelValidation.IsValid)
			{
				// portfolio ID
				var id = portfolio.Id;

				try
				{
					// update request state to pending
					var pendingPortfolio = this.Uow.ConsumerPortfolios.UpdatePortfolioAsPending(id);

					// let the agent handle processing of the request state change
					this.Agent.ConsumerRequestSetToPending(pendingPortfolio);

					// get updated portfolio scheme
					var results = this.Uow.ConsumerPortfolios.GetScheme(id);

					return CreateSuccessResponse(new { success = true, results = results }, HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					// log exception
					Logger.Error(string.Format("Exception detected attempting to update consumer portfolio request state to {0} for Id {1} via user {2}", RequestReceiptStates.Pending, id, this.AccountSession.MemberId), ex);

					return CreateErrorResponse(ex);
				}
			}

			// invalid parameters, generate response
			return CreateInvalidResponse(modelValidation);
		}
		#endregion
	}
}
