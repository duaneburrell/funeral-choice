using Obsequy.Communication;
using Obsequy.Data;
using Obsequy.Model;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsequy.Agent
{
	public class WebAgent
	{
		#region Properties

		#region Logger
		protected NLog.Logger Logger
		{
			get { return NLog.LogManager.GetLogger(Definitions.Logger.Name); }
		}
		#endregion

		#region Mongo
		private MongoDbContext _mongo;
		public MongoDbContext Mongo
		{
			get { return _mongo ?? (_mongo = new MongoDbContext()); }
		}
		#endregion

		#region Switchboard
		private Switchboard _switchboard = null;
		protected Switchboard Switchboard
		{
			get { return _switchboard ?? (_switchboard = new Switchboard()); }
		}
		#endregion

		#endregion

		#region Construction / Initialization

		public WebAgent()
		{

		}

		#endregion

		#region Consumer Methods

		#region Consumer Request Set To Draft
		public void ConsumerRequestSetToDraft(ConsumerPortfolio portfolio)
		{
			throw new NotImplementedException("Please update WebAgent.ConsumerRequestSetToDraft method");
		}
		#endregion

		#region Consumer Request Set To Pending
		public void ConsumerRequestSetToPending(ConsumerPortfolio consumerPortfolio)
		{
			// find all provider portfolios who can provide services for this 
			var responses = this.Mongo.GenerateResponses(consumerPortfolio);

			// for each response, log a generation message and send a notification via the switchboard
			foreach (var response in responses)
			{
				// log the create message
				this.Logger.Info(string.Format("response {0} created for consumer portfolio {1} on behalf of provider portfolio {2} with the state {3}", response.Id, response.ConsumerPortfolioId, response.ProviderPortfolioId, response.State));

				// get the provider portfolio for this response
				var providerPortfolio = this.Mongo.GetProviderPortfolio(response.ProviderPortfolioId);

				// send this to the switchboard for distribution
				this.Switchboard.ResponseUpdatedAsAvailable(response, providerPortfolio, consumerPortfolio);
			}
		}
		#endregion

		#endregion

		#region Provider Methods

		#region Provider Portfolio Account Status Changed
		public void ProviderPortfolioAccountStatusChanged(ProviderPortfolio providerPortfolio)
		{
			// generate responses if the account status is now active
			if (providerPortfolio.AccountStatus == AccountStatus.Active)
			{
				// generate responses for the newly created provider portfolio
				var responses = this.Mongo.GenerateResponses(providerPortfolio);

				// for each response, log a generation message and send a notification via the switchboard
				foreach (var response in responses)
				{
					// log the create message
					this.Logger.Info(string.Format("response {0} created for consumer portfolio {1} on behalf of provider portfolio {2} with the state {3}", response.Id, response.ConsumerPortfolioId, response.ProviderPortfolioId, response.State));

					// get the consumer portfolio for this response
					var consumerPortfolio = this.Mongo.GetConsumerPortfolio(response.ConsumerPortfolioId);

					// send this to the switchboard for distribution
					this.Switchboard.ResponseUpdatedAsAvailable(response, providerPortfolio, consumerPortfolio);
				}
			}
		}
		#endregion

		#endregion
	}
}
