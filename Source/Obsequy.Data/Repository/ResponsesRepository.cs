using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Data
{
	public class ResponsesRepository : BaseRepository<Response>, IResponsesRepository
	{
		public ResponsesRepository() 
			: base() 
		{ 
		}

		#region Find By Id
		public Response FindById(string id)
		{
			return this.Collection.FindAll().Where(item => item.Id == id).FirstOrDefault();
		}
		#endregion

		#region Get Consumer Response Scheme
		public ConsumerResponseScheme GetConsumerResponseScheme(string id)
		{
			// get the responses for this consumer
			var response = this.Mongo.GetResponseById(id);

			// get the consumer portfolio
			var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

			// get the provider portfolio
			var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

			// build the scheme
			var responseScheme = new ConsumerResponseScheme(response, consumerPortfolio, providerPortfolio);

			return responseScheme;
		}
		#endregion

		#region Get Consumer Member Response Schemes
		public List<ConsumerResponseScheme> GetConsumerMemberResponseSchemes(string memberId)
		{
			var responseSchemes = new List<ConsumerResponseScheme>();

			// get the responses for this consumer member
			var responses = this.Mongo.GetResponsesForConsumerMember(memberId);

			// build the scheme for each response
			foreach (var response in responses)
			{
				// get the consumer portfolio
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// build the scheme
				responseSchemes.Add(new ConsumerResponseScheme(response, consumerPortfolio, providerPortfolio));
			}

			return responseSchemes;
		}
		#endregion

		#region Get Consumer Portfolio Response Schemes
		public List<ConsumerResponseScheme> GetConsumerPortfolioResponseSchemes(string portfolioId)
		{
			var responseSchemes = new List<ConsumerResponseScheme>();

			// get the responses for this consumer portfolio
			var responses = this.Mongo.GetResponsesForConsumerPortfolio(portfolioId);

			// build the scheme for each response
			foreach (var response in responses)
			{
				// get the consumer portfolio
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// build the scheme
				responseSchemes.Add(new ConsumerResponseScheme(response, consumerPortfolio, providerPortfolio));
			}

			return responseSchemes;
		}
		#endregion

		#region Get Provider Response Scheme
		public ProviderResponseScheme GetProviderResponseScheme(string id)
		{
			// get the responses for this consumer
			var response = this.Mongo.GetResponseById(id);

			// get the consumer portfolio
			var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

			// get the provider portfolio
			var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

			// build the scheme
			var responseScheme = new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio);

			return responseScheme;
		}
		#endregion

		#region Get Provider Member Response Schemes
		public List<ProviderResponseScheme> GetProviderMemberResponseSchemes(string memberId)
		{
			var responseSchemes = new List<ProviderResponseScheme>();

			// get the responses for this provider
			var responses = this.Mongo.GetResponsesForProviderMember(memberId);

			// build the scheme for each response
			foreach (var response in responses)
			{
				// get the consumer portfolio
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

				// get the consumer member (if accepted)
				var consumerMember = (response.Current.IsAccepted ? this.Mongo.GetConsumerPortfolioCreator(response.ConsumerPortfolioId) : null);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// build the scheme
				responseSchemes.Add(new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio, consumerMember));
			}

			return responseSchemes;
		}
		#endregion

		#region Get Provider Portfolio Response Schemes
		public List<ProviderResponseScheme> GetProviderPortfolioResponseSchemes(string portfolioId)
		{
			var responseSchemes = new List<ProviderResponseScheme>();

			// get the responses for this provider
			var responses = this.Mongo.GetResponsesForProviderPortfolio(portfolioId);

			// build the scheme for each response
			foreach (var response in responses)
			{
				// get the consumer portfolio
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

				// get the consumer member (if accepted)
				var consumerMember = (response.Current.IsAccepted ? this.Mongo.GetConsumerPortfolioCreator(response.ConsumerPortfolioId) : null);

				// get the provider portfolio
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// build the scheme
				responseSchemes.Add(new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio, consumerMember));
			}

			return responseSchemes;
		}
		#endregion

		#region Create
		public Response Create(string providerPortfolioId, string consumerPortfolioId)
		{
			// create the new model
			var model = new Response(providerPortfolioId, consumerPortfolioId, this.AccountSession);

			// insert the collection
			var result = this.Collection.Insert(model);

			if (result.Ok)
			{
				// log the creation
				this.Logger.Info(string.Format("created provider response for provider portfolio Id {0} and consumer portfolio Id {1} via user Id {2}", providerPortfolioId, consumerPortfolioId, this.AccountSession.MemberId));

				// return the created model
				return model;
			}
			else
			{
				// log the failure
				this.Logger.Info(string.Format("failed to create provider response for provider portfolio Id {0} and consumer portfolio Id {1} via user Id {2}. Code: {3}, Reason: {4}", providerPortfolioId, consumerPortfolioId, this.AccountSession.MemberId, result.Code, result.ErrorMessage));
			}

			return null;
		}
		#endregion

		#region Update
		public Response Update(string id, Response response)
		{
			// get the existing response
			var entity = FindById(id);

			return null;
		}
		#endregion

		#region Update Response As Accepted
		public ConsumerResponseScheme UpdateResponseAsAccepted(string id, Payment payment)
		{
			var response = FindById(id);

			if (response != null)
			{
				// update the response state to accepted (tracking receipts are updated)
				response.SetReceiptState(ResponseReceiptStates.Accepted, this.AccountSession, payment.Amount);

				// update to the DB
				response = Update(response);

				// log successful update
				this.Logger.Info(string.Format("successfully set response state to accepted for Id {0} via user {1}", response.Id, this.AccountSession.MemberId));

				// get all responses for this consumer portfolio
				var allResponses = this.Mongo.GetResponsesForConsumerPortfolio(response.ConsumerPortfolioId);

				// set all available responses to expired
				var availableResponses = allResponses.Where(item => item.State == ResponseReceiptStates.Available).ToList();
				foreach (var availableResponse in availableResponses)
				{
					// set the response state to expired
					availableResponse.SetReceiptState(ResponseReceiptStates.Expired, this.AccountSession);

					// update the DB
					Update(availableResponse);

					// log successful update
					this.Logger.Info(string.Format("updated available response state to expired for Id {0} because response {1} was accepted via user {1}", availableResponse.Id, response.Id, this.AccountSession.MemberId));
				}

				// set all pending responses to rejected
				var pendingResponses = allResponses.Where(item => item.State == ResponseReceiptStates.Pending).ToList();
				foreach (var pendingResponse in pendingResponses)
				{
					// set the response state to rejected
					pendingResponse.SetReceiptState(ResponseReceiptStates.Rejected, this.AccountSession);

					// update the DB
					Update(pendingResponse);

					// log successful update
					this.Logger.Info(string.Format("updated pending response state to rejected for Id {0} because response {1} was accepted via user {1}", pendingResponse.Id, response.Id, this.AccountSession.MemberId));
				}

				// get the portfolios for the accepted response
				var consumerMember = this.Mongo.GetConsumerPortfolioCreator(response.ConsumerPortfolioId);
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// update the consumer portfolio state as completed, set the payment, and save in the DB
				consumerPortfolio.Request.SetReceiptState(RequestReceiptStates.Completed, this.AccountSession);
				consumerPortfolio.Payment = payment;
				this.Mongo.Update(consumerPortfolio);

				// let the switch board process the accepted response
				this.Switchboard.ResponseUpdatedAsUserAccepted(response, providerPortfolio, consumerPortfolio, consumerMember);

				// let the switch board process each expired response
				foreach (var availableResponse in availableResponses)
					this.Switchboard.ResponseUpdatedAsExpired(availableResponse, this.Mongo.GetProviderPortfolio(availableResponse.ProviderPortfolioId), consumerPortfolio);

				// let the switch board process each auto rejected response
				foreach (var pendingResponse in pendingResponses)
					this.Switchboard.ResponseUpdatedAsAutoRejected(pendingResponse, this.Mongo.GetProviderPortfolio(pendingResponse.ProviderPortfolioId), consumerPortfolio);

				// return the updated scheme
				return GetConsumerResponseScheme(response.Id);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not set response to accepted for Id {0} because it does not exist via user {1}", id, this.AccountSession.MemberId));
			}

			return new ConsumerResponseScheme();
		}
		#endregion

		#region Update Response As Available
		public ProviderResponseScheme UpdateResponseAsAvailable(ProviderResponseScheme scheme)
		{
			var response = FindById(scheme.Id);

			if (response != null)
			{
				// clear all values that may have previously been set
				response.Clear();

				// update the response state to available (tracking receipts are updated)
				response.SetReceiptState(ResponseReceiptStates.Available, this.AccountSession);

				// update to the DB
				response = Update(response);

				// log successful update
				this.Logger.Info(string.Format("successfully set response state to available for Id {0} via user {1}", scheme.Id, this.AccountSession.MemberId));

				// create and return the new provider response scheme
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// notify the switch board
				this.Switchboard.ResponseUpdatedAsAvailable(response, providerPortfolio, consumerPortfolio);

				// return the updated scheme
				return GetProviderResponseScheme(response.Id);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not set response to available for Id {0} because it does not exist via user {1}", scheme.Id, this.AccountSession.MemberId));
			}

			return scheme;
		}
		#endregion

		#region Update Response As Dismissed
		public ProviderResponseScheme UpdateResponseAsDismissed(ProviderResponseScheme scheme)
		{
			var response = FindById(scheme.Id);

			if (response != null)
			{
				// clear all values that may have previously been set
				response.Clear();

				// update the response state to dismissed (tracking receipts are updated)
				response.SetReceiptState(ResponseReceiptStates.Dismissed, this.AccountSession);

				// update to the DB
				response = Update(response);

				// log successful update
				this.Logger.Info(string.Format("successfully set response state to dismissed for Id {0} via user {1}", scheme.Id, this.AccountSession.MemberId));

				// create and return the new provider response scheme
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// notify the switch board
				this.Switchboard.ResponseUpdatedAsDismissed(response, providerPortfolio, consumerPortfolio);

				// return the updated scheme
				return new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not set response to dismissed for Id {0} because it does not exist via user {1}", scheme.Id, this.AccountSession.MemberId));
			}

			return scheme;
		}
		#endregion

		#region Update Response As Pending
		public ProviderResponseScheme UpdateResponseAsPending(ProviderResponseScheme scheme)
		{
			var response = FindById(scheme.Id);

			if (response != null)
			{
				// update the response properties
				response.Update(scheme, this.AccountSession);

				// update the response state to pending (tracking receipts are updated)
				response.SetReceiptState(ResponseReceiptStates.Pending, this.AccountSession, scheme.Quote);

				// update to the DB
				response = Update(response);

				// log successful update
				this.Logger.Info(string.Format("successfully set response state to pending for Id {0} via user {1}", scheme.Id, this.AccountSession.MemberId));

				// create and return the new provider response scheme
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// notify the switch board
				this.Switchboard.ResponseUpdatedAsPending(response, providerPortfolio, consumerPortfolio);

				// return the updated scheme
				return new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not set response to pending for Id {0} because it does not exist via user {1}", scheme.Id, this.AccountSession.MemberId));
			}

			return scheme;
		}
		#endregion

		#region Update Response As Recalled
		public ProviderResponseScheme UpdateResponseAsRecalled(ProviderResponseScheme scheme)
		{
			//TODO: what do I do with this?  this can be done when the reponse is either dismissed or pending.. so, ideally
			// we'd just put back to available state ... but do I care what the previous bid was?? huh??
			var response = FindById(scheme.Id);

			if (response != null)
			{
				// update the response state to recalled (tracking receipts are updated)
				response.SetReceiptState(ResponseReceiptStates.Recalled, this.AccountSession);

				// update to the DB
				response = Update(response);

				// log successful update
				this.Logger.Info(string.Format("successfully set response state to recalled for Id {0} via user {1}", scheme.Id, this.AccountSession.MemberId));

				// create and return the new provider response scheme
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// notify the switch board
				//this.Switchboard.ResponseUpdated(response, consumerPortfolio, providerPortfolio);

				// return the updated scheme
				return GetProviderResponseScheme(response.Id);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not set response to recalled for Id {0} because it does not exist via user {1}", scheme.Id, this.AccountSession.MemberId));
			}

			return scheme;
		}
		#endregion

		#region Update Response As Rejected
		public ConsumerResponseScheme UpdateResponseAsRejected(ConsumerResponseScheme scheme)
		{
			var response = FindById(scheme.Id);

			if (response != null)
			{
				// update the response state to rejected (tracking receipts are updated)
				response.SetReceiptState(ResponseReceiptStates.Rejected, this.AccountSession);

				// update to the DB
				response = Update(response);

				// log successful update
				this.Logger.Info(string.Format("successfully set response state to rejected for Id {0} via user {1}", scheme.Id, this.AccountSession.MemberId));

				// create and return the new provider response scheme
				var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// notify the switch board
				this.Switchboard.ResponseUpdatedAsUserRejected(response, providerPortfolio, consumerPortfolio);

				// return the updated scheme
				return GetConsumerResponseScheme(response.Id);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not set response to rejected for Id {0} because it does not exist via user {1}", scheme.Id, this.AccountSession.MemberId));
			}

			return scheme;
		}
		#endregion
	}
}
