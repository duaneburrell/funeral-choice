using Microsoft.AspNet.SignalR;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obsequy.Communication
{
	public class AdministratorHub : BaseHub
	{
		#region Properties

		#region Hub Context
		private IHubContext _hubContext;
		protected IHubContext HubContext
		{
			get { return _hubContext ?? (_hubContext = GlobalHost.ConnectionManager.GetHubContext<AdministratorHub>()); }
		}
		#endregion

		#endregion

		#region Methods

		public void ProviderPortfolioCreated(ProviderPortfolio providerPortfolio)
		{
			// find the identities for any administrators
			var identities = this.Identities.Where(item => item.AccountType == AccountType.Administrator);

			if (!identities.Any())
				return;

			// build a provider portfolio scheme to send out
			var providerPortfolioScheme = new ProviderPortfolioScheme(providerPortfolio);

			// send this out to all client identities
			foreach (var identity in identities)
			{
				// send out the created provider portfolio
				this.HubContext.Clients.Client(identity.ConnectionId).ProviderPortfolioCreated(providerPortfolioScheme);

				// trace the transmission of this response
				this.Logger.Trace(string.Format("created provider portfolio for Id {0} automatically transmitted to for user Id {1} with connection Id {2}", providerPortfolioScheme.Id, identity.UserId, identity.ConnectionId));
			}
		}

		#endregion
	}
}