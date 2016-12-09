using Obsequy.Communication;
using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Data
{
	public class ConsumerPortfoliosRepository : BaseRepository<ConsumerPortfolio>, IConsumerPortfoliosRepository
	{
		public ConsumerPortfoliosRepository() : base() { }

		#region Find By Id
		public ConsumerPortfolio FindById(string id)
		{
			var entity = this.Collection.FindAll().Where(pg => pg.Id == id).FirstOrDefault();

			return entity;
		}
		#endregion

		#region Get Scheme
		public ConsumerPortfolioScheme GetScheme(string id)
		{
			// get the portfolio
			var portfolio = this.Mongo.GetConsumerPortfolio(id);

			// create the portfolio scheme
			var portfolioScheme = new ConsumerPortfolioScheme(portfolio);

			// add the members
			var members = this.Mongo.GetConsumerPortfolioMembers(portfolio.Id);
			portfolioScheme.Members.AddRange(members);

			return portfolioScheme;
		}
		#endregion

		#region Get All
		public List<ConsumerPortfolio> GetAll()
		{
			// get all consumer portfolios
			return this.Mongo.GetConsumerPortfolios();
		}
		#endregion

		#region Create Portfolio
		public ConsumerPortfolio CreatePortfolio(ConsumerPortfolio scheme)
		{
			var id = string.Empty;

			// create the new model
			var portfolio = new ConsumerPortfolio()
			{
				AccountStatus = AccountStatus.Active,
				Created = new ChangeReceipt(this.AccountSession),
				Modified = new ChangeReceipt(this.AccountSession),
				Request = new ConsumerRequest(this.AccountSession),
				Principal = new ConsumerPrincipal(scheme.Principal)
			};

			// geo-code the address
			portfolio.Principal.Address.GeoCodeLocation();

			// insert the collection
			var result = this.Collection.Insert(portfolio);

			if (result.Ok)
			{
				// log the creation
				this.Logger.Info(string.Format("created consumer portfolio with ID {0} via user {1}", portfolio.Id, this.AccountSession.MemberId));

				// attach the portfolio to the current member
				var member = this.Mongo.AddMemberPortfolio(this.AccountSession.MemberId, portfolio);

				// notify the switch board
				this.Switchboard.ConsumerPortfolioCreated(member, portfolio);

				// return the created model
				return portfolio;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to create consumer portfolio for user ID {0}. Code: {1}, Reason: {2}", this.AccountSession.MemberId, result.Code, result.ErrorMessage));
			}

			return null;
		}
		#endregion

		#region Update Portfolio
		public ConsumerPortfolio UpdatePortfolio(ConsumerPortfolio scheme)
		{
			var portfolio = FindById(scheme.Id);

			if (portfolio != null)
			{
				if (!portfolio.HasChanged(scheme))
					return portfolio;

				if (portfolio.Principal.HasChanged(scheme.Principal))
				{
					// update the principal and track changes
					portfolio.Principal.Update(scheme.Principal);
				}

				if (portfolio.Preference.HasChanged(scheme.Preference))
				{
					// is a geo-code update required?
					var updateGeoCode = portfolio.Preference.Proximity.IsGeoCodingRequired(scheme.Preference.Proximity);

					// update the preference
					portfolio.Preference.Update(scheme.Preference);

					// geo-code if required
					if (updateGeoCode)
						portfolio.Preference.Proximity.GeoCodeLocation();
				}

				if (portfolio.Schedule.HasChanged(scheme.Schedule))
				{
					// update the schedule and track changes
					portfolio.Schedule.Update(scheme.Schedule);
				}

				// log this update
				portfolio.Modified = new ChangeReceipt(this.AccountSession);

				// update to the DB
				portfolio = Update(portfolio);

				// log successful update
				this.Logger.Info(string.Format("updated consumer portfolio for Id {0} via user {1}", scheme.Id, this.AccountSession.MemberId));
			}
			else
			{
				// log non-existent consumer portfolio
				this.Logger.Warn(string.Format("could not update consumer portfolio for Id {0} because it does not exist", scheme.Id));
			}

			return portfolio;
		}
		#endregion

		#region Delete Portfolio
		public void DeletePortfolio(string id)
		{
			// TODO: we'll need to add real-time handling of this via SignalR
			var consumerPortfolio = this.Mongo.GetConsumerPortfolio(id); 

			if (consumerPortfolio != null)
			{
				// delete all entities related to this profile
				var members = this.Mongo.DeleteConsumerPortfolio(id);

				// log successful update
				this.Logger.Info(string.Format("deleted consumer portfolio for Id {0} via user {1}", id, this.AccountSession.MemberId));

				// notify the switch board
				this.Switchboard.ConsumerPortfolioDeleted(members);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not delete consumer portfolio for Id {0} because it does not exist", id));
			}
		}
		#endregion

		#region Update Portfolio As Pending
		public ConsumerPortfolio UpdatePortfolioAsPending(string id)
		{
			var portfolio = this.Mongo.GetConsumerPortfolio(id);

			if (portfolio != null)
			{
				// set the receipt state
				portfolio.Request.SetReceiptState(RequestReceiptStates.Pending, this.AccountSession);

				// log this update
				portfolio.Modified = new ChangeReceipt(this.AccountSession);

				// update to the DB
				portfolio = Update(portfolio);

				// log successful update
				this.Logger.Info(string.Format("updated consumer request state for Id {0} to {1} via user {2}", id, RequestReceiptStates.Pending, this.AccountSession.MemberId));

				// notify the switch board
				this.Switchboard.ConsumerRequestSetToPending(portfolio);
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not update consumer request state for Id {0} to {1} because it does not exist", id, RequestReceiptStates.Pending));
			}

			return portfolio;
		}
		#endregion

		#region Update Request As Draft
		public ConsumerPortfolio UpdatePortfolioAsDraft(string id)
		{
			var portfolio = this.Mongo.GetConsumerPortfolio(id);

			if (portfolio != null)
			{
				// set the receipt state
				portfolio.Request.SetReceiptState(RequestReceiptStates.Draft, this.AccountSession);

				// log this update
				portfolio.Modified = new ChangeReceipt(this.AccountSession);

				// update to the DB
				portfolio = Update(portfolio);

				// log successful update
				this.Logger.Info(string.Format("updated consumer request state for Id {0} to {1} via user {2}", id, RequestReceiptStates.Draft, this.AccountSession.MemberId));
			}
			else
			{
				// log non-existent consumer group
				this.Logger.Warn(string.Format("could not update consumer request state for Id {0} to {1} because it does not exist", id, RequestReceiptStates.Draft));
			}

			return portfolio;
		}
		#endregion

        #region Remind Portfolio
        public void RemindPortfolio(string id)
        {
            var portfolio = this.Mongo.GetConsumerPortfolio(id);

            if (portfolio != null)
            {
                // notify the switchboard
                Switchboard.ConsumerPortfolioReminded(portfolio);

                // update the timestamp
                portfolio.Reminded = new ChangeReceipt
                {
                    On = DateTimeHelper.Now,
                    By = this.AccountSession.MemberId
                };

                // save to the database
                Update(portfolio);

                // log successful update
                this.Logger.Info(string.Format("reminded consumer portfolio for Id {0} via user {1}", id, this.AccountSession.MemberId));
            }
            else
            {
                // log non-existent consumer portfolio
                this.Logger.Warn(string.Format("could not remind consumer portfolio for Id {0} because it does not exist", id));
            }
        }
        #endregion
	}
}

