using Microsoft.AspNet.SignalR;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obsequy.Communication
{
	public class ConsumerHub : BaseHub
	{
		#region Properties

		#region Hub Context
		private IHubContext _hubContext;
		protected IHubContext HubContext
		{
			get { return _hubContext ?? (_hubContext = GlobalHost.ConnectionManager.GetHubContext<ConsumerHub>()); }
		}
		#endregion

		#endregion

		#region Identity Methods
		public void UpdateIdentity(ConsumerMember member)
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

		public void ResponseUpdated(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
		{
			// find the identities for this consumer portfolio ID
			var identities = this.Identities.Where(item => item.PortfolioId == consumerPortfolio.Id);

			if (!identities.Any())
				return;

			// build a consumer response scheme to send out
			var responseScheme = new ConsumerResponseScheme(response, consumerPortfolio, providerPortfolio);

			// send this out to all client identities
			foreach (var identity in identities)
			{
				// send out the updated resoponse scheme
				this.HubContext.Clients.Client(identity.ConnectionId).ResponseUpdated(responseScheme);

				// trace the transmission of this response
				this.Logger.Trace(string.Format("updated response for Id {0} automatically transmitted to consumer portfolio {1} for user Id {2} with connection Id {3}", response.Id, identity.PortfolioId, identity.UserId, identity.ConnectionId));
			}
		}

		#endregion
	}
}