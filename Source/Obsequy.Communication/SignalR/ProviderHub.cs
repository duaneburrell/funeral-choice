using Microsoft.AspNet.SignalR;
using Obsequy.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obsequy.Communication
{
	public class ProviderHub : BaseHub
	{
		#region Properties

		#region Hub Context
		private IHubContext _hubContext;
		protected IHubContext HubContext
		{
			get { return _hubContext ?? (_hubContext = GlobalHost.ConnectionManager.GetHubContext<ProviderHub>()); }
		}
		#endregion

		#endregion

		#region Identity Methods
		public void UpdateIdentity(ProviderMember member)
		{
			var identity = this.Identities.Where(item => item.UserId == member.Id).FirstOrDefault();

			if (identity != null)
			{
				identity.PortfolioId = member.PortfolioId;
				identity.PortfolioIds.Clear();
				identity.PortfolioIds.AddRange(member.PortfolioIds);
			}
		}
		#endregion

		#region Methods

		#region Response Updated
		public void ResponseUpdated(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
		{
			// find the identities for this provider portfolio ID
			var identities = this.Identities.Where(item => item.PortfolioId == providerPortfolio.Id);

			if (!identities.Any())
				return;

			// build a provider response scheme to send out
			var responseScheme = new ProviderResponseScheme(response, providerPortfolio, consumerPortfolio);

			// send this out to all client identities
			foreach (var identity in identities)
			{
				// send out the updated resoponse scheme
				this.HubContext.Clients.Client(identity.ConnectionId).ResponseUpdated(responseScheme);

				// trace the transmission of this response
				this.Logger.Trace(string.Format("updated response for Id {0} automatically transmitted to provider portfolio {1} for user Id {2} with connection Id {3}", response.Id, identity.PortfolioId, identity.UserId, identity.ConnectionId));
			}
		}
		#endregion

		#region Push Responses
		protected void PushResponses()
		{
			// build all response schemes for this provider
			// TODO: this will need to be throttled in the future!
			var responses = this.Mongo.GetResponseSchemesForProviderMember(this.Identity.UserId);

			//// find the identities for this provider portfolio ID
			//var identities = this.Identities.Where(item => item.PortfolioId == providerPortfolio.Id);

			//if (!identities.Any())
			//	return;

			//// build a provider response scheme to send out
			//var responseScheme = new ProviderResponseScheme(response, consumerPortfolio, providerPortfolio);

			//// send this out to all client identities
			//foreach (var identity in identities)
			//{
			//	// send out the updated resoponse scheme
			//	this.HubContext.Clients.Client(identity.ConnectionId).ResponseUpdated(responseScheme);

			//	// trace the transmission of this response
			//	this.Logger.Trace(string.Format("updated response for Id {0} automatically transmitted to provider portfolio {1} for user Id {2} with connection Id {3}", response.Id, identity.PortfolioId, identity.UserId, identity.ConnectionId));
			//}
		}
		#endregion

		#endregion
	}
}