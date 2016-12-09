using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;

namespace Obsequy.Communication
{
	public class Switchboard
	{
		#region Properties

		#region Administrator Hub
		private AdministratorHub _administratorHub;
		protected AdministratorHub AdministratorHub
		{
			get { return _administratorHub ?? (_administratorHub = new AdministratorHub()); }
		}
		#endregion

		#region Consumer Hub
		private ConsumerHub _consumerHub;
		protected ConsumerHub ConsumerHub
		{
			get { return _consumerHub ?? (_consumerHub = new ConsumerHub()); }
		}
		#endregion

		#region Mailer
		private Mailer _mailer;
		protected Mailer Mailer
		{
			get { return _mailer ?? (_mailer = new Mailer()); }
		}
		#endregion

		#region Logger
		private NLog.Logger _logger;
		protected NLog.Logger Logger
		{
			get { return _logger ?? (_logger = NLog.LogManager.GetLogger(Definitions.Logger.Name)); }
		}
		#endregion

		#region Provider Hub
		private ProviderHub _providerHub;
		protected ProviderHub ProviderHub
		{
			get { return _providerHub ?? (_providerHub = new ProviderHub()); }
		}
		#endregion

		#endregion

		#region Consumer Handling

		#region Consumer Member Created
		public void ConsumerMemberCreated(ConsumerMember member)
		{
			// send out mail notifications
			this.Mailer.ConsumerMemberCreated(member);
		}
		#endregion

		#region Consumer Portfolio Created
		public void ConsumerPortfolioCreated(ConsumerMember member, ConsumerPortfolio consumerPortfolio)
		{
			// update the consumer hub identity
			this.ConsumerHub.UpdateIdentity(member);
		}
		#endregion

		#region Consumer Portfolio Deleted
		public void ConsumerPortfolioDeleted(List<ConsumerMember> members)
		{
			// update the consumer hub identities
			foreach (var member in members)
			{
				this.ConsumerHub.UpdateIdentity(member);
			}
		}
		#endregion

        #region Consumer Portfolio Reminded
		public void ConsumerPortfolioReminded(ConsumerPortfolio consumerPortfolio)
		{
            // send reminder email to consumer member
            Mailer.ConsumerPortfolioReminded(consumerPortfolio);
		}
		#endregion

		#region Consumer Request Set To Pending
		public void ConsumerRequestSetToPending(ConsumerPortfolio consumerPortfolio)
		{
			// send out mail notifications
			this.Mailer.ConsumerRequestSetToPending(consumerPortfolio);
		}
		#endregion

		#endregion

		#region Provider Handling

		#region Provider Member Created
		public void ProviderMemberCreated(ProviderMember member)
		{
			// send out mail notifications
			this.Mailer.ProviderMemberCreated(member);
		}
		#endregion

		#region Provider Portfolio Created
		public void ProviderPortfolioCreated(ProviderMember member, ProviderPortfolio providerPortfolio)
		{
			// update the provider hub identity
			this.ProviderHub.UpdateIdentity(member);

			// notify all interested administrators that the provider portfolio has been created 
			this.AdministratorHub.ProviderPortfolioCreated(providerPortfolio);
			
			// send out mail notifications
			this.Mailer.ProviderPortfolioCreated(providerPortfolio);
		}
		#endregion

		#region Provider Portfolio Deleted
		public void ProviderPortfolioDeleted(List<ProviderMember> members)
		{
			// update the provider hub identities
			foreach (var member in members)
			{
				this.ProviderHub.UpdateIdentity(member);
			}
		}
		#endregion

		#region Provider Portfolio Account Status Changed
		public void ProviderPortfolioAccountStatusChanged(ProviderPortfolio providerPortfolio)
		{
			// send out mail notifications
			this.Mailer.ProviderPortfolioAccountStatusChanged(providerPortfolio);
		}
		#endregion

		#endregion

		#region Response Handling

		#region Response Updated As Available
		public void ResponseUpdatedAsAvailable(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio)
		{
			// notify all interested parties that the response has been set to available
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsAvailable(response, consumerPortfolio, providerPortfolio);
		}
		#endregion

		#region Response Updated As Dismissed
		public void ResponseUpdatedAsDismissed(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio)
		{
			// notify all interested parties that the response has been set to dismissed
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsDismissed(response);
		}
		#endregion

		#region Response Updated As Expired
		public void ResponseUpdatedAsExpired(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio)
		{
			// notify all interested parties that the response has been set to dismissed
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsExpired(response);
		}
		#endregion

		#region Response Updated As Pending
		public void ResponseUpdatedAsPending(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio)
		{
			// notify all interested parties that the response has been set to pending
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsPending(response, consumerPortfolio, providerPortfolio);
		}
		#endregion

		#region Response Updated As User Accepted
		public void ResponseUpdatedAsUserAccepted(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio, ConsumerMember consumerMember)
		{
			// notify all interested parties that the response has been set to accepted
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio, consumerMember);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsUserAccepted(response, consumerPortfolio, providerPortfolio);
		}
		#endregion

		#region Response Updated As Auto Rejected
		public void ResponseUpdatedAsAutoRejected(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio)
		{
			// notify all interested parties that the response has been set to rejected
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsAutoRejected(response, consumerPortfolio, providerPortfolio);
		}
		#endregion

		#region Response Updated As User Rejected
		public void ResponseUpdatedAsUserRejected(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio)
		{
			// notify all interested parties that the response has been set to rejected
			SendUpdatedReponse(response, providerPortfolio, consumerPortfolio);

			// send out mail notifications
			this.Mailer.ResponseUpdatedAsUserRejected(response, consumerPortfolio, providerPortfolio);
		}
		#endregion

		#region Send Updated Reponse
		private void SendUpdatedReponse(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio, ConsumerMember consumerMember = null)
		{
			// notify all interested consumers that the response has been updated
			this.ConsumerHub.ResponseUpdated(response, consumerPortfolio, providerPortfolio);

			// notify all interested providers that the response has been updated
			this.ProviderHub.ResponseUpdated(response, consumerPortfolio, providerPortfolio);
		}
		#endregion

		#endregion
	}
}
