using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Data
{
	public class ProviderPortfoliosRepository : BaseRepository<ProviderPortfolio>, IProviderPortfoliosRepository
	{
		public ProviderPortfoliosRepository() : base() { }

		#region Find By Id
		public ProviderPortfolio FindById(string id)
		{
			var entity = this.Collection.FindAll().Where(pg => pg.Id == id).FirstOrDefault();

			return entity;
		}
		#endregion

		#region Get Scheme
		public ProviderPortfolioScheme GetScheme(string id)
		{
			// get the portfolio
			var portfolio = this.Mongo.GetProviderPortfolio(id);

			// create the portfolio scheme
			var portfolioScheme = new ProviderPortfolioScheme(portfolio);

			// add the members
			var members = this.Mongo.GetProviderPortfolioMembers(portfolio.Id);
			portfolioScheme.Members.AddRange(members);

			return portfolioScheme;
		}
		#endregion

		#region Get Provider Groups
		public List<ProviderPortfolio> GetProviderPortfolios()
		{
			var entities = this.Collection.FindAll().ToList();

			return entities;
		}
		#endregion

		#region Get Active Provider Groups
		public List<ProviderPortfolio> GetActiveProviderPortfolios()
		{
			var entities = this.Collection.FindAll().Where(pg => (AccountStatus)pg.AccountStatus == AccountStatus.Active).ToList();

			return entities;
		}
		#endregion

		#region Get Rejected Provider Groups
		public List<ProviderPortfolio> GetRejectedProviderGroups()
		{
			var entities = this.Collection.FindAll().Where(pg => (AccountStatus)pg.AccountStatus == AccountStatus.Rejected).ToList();

			return entities;
		}
		#endregion

		#region Get Suspend Provider Groups
		public List<ProviderPortfolio> GetSuspendedProviderPortfolios()
		{
			var entities = this.Collection.FindAll().Where(pg => (AccountStatus)pg.AccountStatus == AccountStatus.Suspended).ToList();

			return entities;
		}
		#endregion

		#region Get Pending Provider Groups
		public List<ProviderPortfolio> GetPendingProviderPortfolios()
		{
			var entities = this.Collection.FindAll().Where(pg => (AccountStatus)pg.AccountStatus == AccountStatus.Pending).ToList();

			return entities;
		}
		#endregion

		#region Create Portfolio

		public ProviderPortfolio CreatePortfolio(ProviderPortfolio scheme)
		{
			var id = string.Empty;

			// create the new portfolio
			var portfolio = new ProviderPortfolio()
			{
				AccountStatus = AccountStatus.None,
				Principal = new ProviderPrincipal(scheme.Principal),
				Profile = new ProviderProfile(),
				Created = new ChangeReceipt(this.AccountSession),
				Modified = new ChangeReceipt(this.AccountSession)
			};

			// geo-code the address
			portfolio.Principal.Address.GeoCodeLocation();

			// insert the collection
			var result = this.Collection.Insert(portfolio);

			if (result.Ok)
			{
				// log the creation
				this.Logger.Info(string.Format("created provider portfolio with ID {0} via user {1}", portfolio.Id, this.AccountSession.MemberId));

				// attach the portfolio to the current member
				var member = this.Mongo.AddMemberPortfolio(this.AccountSession.MemberId, portfolio);

				// notify the switch board
				this.Switchboard.ProviderPortfolioCreated(member, portfolio);

				// return the created model
				return portfolio;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to provider provider portfolio for user ID {0}. Code: {1}, Reason: {2}", this.AccountSession.MemberId, result.Code, result.ErrorMessage));
			}

			return null;
		}
		#endregion

		#region Update Portfolio
		public ProviderPortfolio UpdatePortfolio(ProviderPortfolio scheme)
		{
			var portfolio = FindById(scheme.Id);

			if (portfolio != null)
			{
				if (!portfolio.HasChanged(scheme))
					return portfolio;

				if (portfolio.Principal.HasChanged(scheme.Principal))
				{
					// is a geo-code update required?
					var updateGeoCode = portfolio.Principal.Address.IsGeoCodingRequired(scheme.Principal.Address);

					// update the principal
					portfolio.Principal.Update(scheme.Principal);

					// geo-code if required
					if (updateGeoCode)
						portfolio.Principal.Address.GeoCodeLocation();
				}

				if (portfolio.Profile.HasChanged(scheme.Profile))
				{
					// update the profile and track changes
					portfolio.Profile.Update(scheme.Profile);
				}

				// see if the account status can go to pending
				if (portfolio.AccountStatus == AccountStatus.None && portfolio.IsComplete)
				{
					// set the state to pending
					portfolio.AccountStatus = AccountStatus.Pending;

					// notify the switch board
					this.Switchboard.ProviderPortfolioAccountStatusChanged(portfolio);
				}

				// log this update
				portfolio.Modified = new ChangeReceipt(this.AccountSession);

				// update to the DB
				portfolio = Update(portfolio);

				// log successful update
				this.Logger.Info(string.Format("updated provider portfolio for Id {0} with account status {1} via user {2}", scheme.Id, portfolio.AccountStatus, this.AccountSession.MemberId));
			}
			else
			{
				// log non-existent provider group
				this.Logger.Warn(string.Format("could not update provider portfolio for Id {0} because it does not exist", scheme.Id));
			}

			return portfolio;
		}
		#endregion

		#region Delete Portfolio
		public void DeletePortfolio(string id)
		{
			// TODO: we'll need to add real-time handling of this via SignalR
			var providerPortfolio = FindById(id);

			if (providerPortfolio != null)
			{
				// delete all entities related to this profile
				var members = this.Mongo.DeleteProviderPortfolio(providerPortfolio.Id);

				// log successful update
				this.Logger.Info(string.Format("deleted provider portfolio for Id {0} via user {1}", id, this.AccountSession.MemberId));

				// notify the switch board
				this.Switchboard.ProviderPortfolioDeleted(members);
			}
			else
			{
				// log non-existent provider group
				this.Logger.Warn(string.Format("could not delete provider portfolio for Id {0} because it does not exist", id));
			}
		}
		#endregion

		#region Update Account Status
		public ProviderPortfolio UpdateAccountStatus(ProviderPortfolio portfolio)
		{
			var entity = this.Collection.FindAll().Where(pg => pg.Id == portfolio.Id).FirstOrDefault();

			if (entity != null)
			{
				// save the current status for logging
				var status = entity.AccountStatus;

				// change the account status
				entity.AccountStatus = portfolio.AccountStatus;
				
				// update in the DB
				entity = Update(entity);

				// log the update
				this.Logger.Info(string.Format("updated provider {0} status to {1} from {2}", entity.Id, status, portfolio.AccountStatus));

				// notify the switch board
				this.Switchboard.ProviderPortfolioAccountStatusChanged(portfolio);

				// return the updated entity
				return entity;
			}

			return null;
		}
		#endregion
	}
}

