using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Data
{
	public class ConsumerMembersRepository : BaseRepository<ConsumerMember>, IConsumerMembersRepository
	{
		#region Fields

		private ConsumerPortfolioComparer _consumerPortfolioComparer = new ConsumerPortfolioComparer();

		#endregion

		#region Constructor
		public ConsumerMembersRepository() : base() 
		{ 
		}
		#endregion

		#region Find By Id
		public ConsumerMember FindById(string id)
		{
			var entity = this.Collection.FindAll().Where(pg => pg.Id == id).FirstOrDefault();

			return entity;
		}
		#endregion

		#region Get Scheme
		public ConsumerMember GetScheme(string memberId, string portfoliId = null)
		{
			// get the current member
			var member = this.Collection.FindAll().Where(item => item.Id == memberId).FirstOrDefault();

			if (portfoliId == null)
			{
				// create all the portfolio schemes
				var portfolios = this.Mongo.GetConsumerMemberPortfolios(memberId).OrderBy(item => item, _consumerPortfolioComparer).ToList();

				foreach (var portfolio in portfolios) 
				{
					// create the portfolio scheme
					var portfolioScheme = new ConsumerPortfolioScheme(portfolio);

					// add the members
					var members = this.Mongo.GetConsumerPortfolioMembers(portfolio.Id, member.Id);
					portfolioScheme.Members.AddRange(members);

					// add to the member's list of portfolio schemes
					member.Portfolios.Add(portfolioScheme);
				}
			}
			else
			{
				// create this portfolio schemes
				var portfolio = this.Mongo.GetConsumerPortfolio(portfoliId);

				// create the portfolio scheme
				var portfolioScheme = new ConsumerPortfolioScheme(portfolio);

				// add the members
				var members = this.Mongo.GetConsumerPortfolioMembers(portfolio.Id, member.Id);
				portfolioScheme.Members.AddRange(members);

				// add to the member's list of portfolio schemes
				member.Portfolios.Add(portfolioScheme);
			}

			return member;
		}
		#endregion

		#region Create Member
		public ConsumerMember CreateMember(ConsumerRegistrationForm form, string membershipId)
		{
			// create the model
			var model = new ConsumerMember(form, membershipId);

			// insert the collection
			var result = this.Collection.Insert(model);

			if (result.Ok)
			{
				// adjust the receipts to indicate the current user as the creator
				model.Created = new ChangeReceipt()
				{
					On = this.TimeStamp,
					By = model.Id
				};

				model.Modified = new ChangeReceipt()
				{
					On = this.TimeStamp,
					By = model.Id
				};

				// update with tracking receipts
				model = Update(model);

				// log the creation
				this.Logger.Info(string.Format("created consumer member for email {0} with ID {1}", model.Email, model.Id));

				// return the created model
				return model;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to create consumer member for email {0}. Code: {1}, Reason: {2}", model.Email, result.Code, result.ErrorMessage));
			}

			return null;
		}
		#endregion

		#region Update Member
		public ConsumerMember UpdateMember(ConsumerMember member)
		{
			var entity = FindById(member.Id);

			if (entity != null)
			{
				if (!entity.HasChanged(member))
					return entity;

				// update the entity
				entity.Update(member);
				
				// update the tracking receipt
				entity.Modified = new ChangeReceipt(this.AccountSession);

				// update in DB
				Update(entity);

				// log the operation
				this.Logger.Info(string.Format("successfully updated consumer member for ID {0} via user {1}", member.Id, this.AccountSession.MemberId));

				// return the updated entity
				return entity;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to update consumer member for ID {0} because user does not exist", member.Id));
			}

			return null;
		}
		#endregion
	}
}

