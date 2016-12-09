using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Data
{
	public class ProviderMembersRepository : BaseRepository<ProviderMember>, IProviderMembersRepository
	{
		public ProviderMembersRepository() : base() { }

		#region Find By Id
		public ProviderMember FindById(string id)
		{
			var entity = this.Collection.FindAll().Where(pg => pg.Id == id).FirstOrDefault();

			return entity;
		}
		#endregion

		#region Get Scheme
		public ProviderMember GetScheme(string memberId, string portfoliId = null)
		{
			// get the current member
			var member = this.Collection.FindAll().Where(item => item.Id == memberId).FirstOrDefault();

			if (portfoliId == null)
			{
				// create all the portfolio schemes
				var portfolios = this.Mongo.GetProviderPortfolios(memberId);

				foreach (var portfolio in portfolios)
				{
					// create the portfolio scheme
					var portfolioScheme = new ProviderPortfolioScheme(portfolio);

					// add the members
					var members = this.Mongo.GetProviderPortfolioMembers(portfolio.Id, member.Id);
					portfolioScheme.Members.AddRange(members);

					// add to the member's list of portfolio schemes
					member.Portfolios.Add(portfolioScheme);
				}
			}
			else
			{
				// create all the portfolio schemes
				var portfolio = this.Mongo.GetProviderPortfolio(portfoliId);

				// create the portfolio scheme
				var portfolioScheme = new ProviderPortfolioScheme(portfolio);

				// add the members
				var members = this.Mongo.GetProviderPortfolioMembers(portfolio.Id, member.Id);
				portfolioScheme.Members.AddRange(members);

				// add to the member's list of portfolio schemes
				member.Portfolios.Add(portfolioScheme);
			}

			return member;
		}
		#endregion

		#region Create Member
		public ProviderMember CreateMember(ProviderRegistrationForm registrationForm, string membershipId)
		{
			// create the model
			var model = new ProviderMember()
			{
				AccountType = AccountType.Provider,
				AccountStatus = AccountStatus.Pending,
				AccountPrestige = AccountPrestige.Creator,
				MembershipId = membershipId,
				Email = registrationForm.Member.Email.Scrub(),
				FirstName = registrationForm.Member.FirstName.Scrub(),
				LastName = registrationForm.Member.LastName.Scrub(),
				PortfolioIds = new List<string>(),
				PortfolioId = string.Empty,
			};

			// insert the collection
			var result = this.Collection.Insert(model);

			if (result.Ok)
			{
				// add the receipts
				model.Created = new ChangeReceipt()
				{
					By = model.Id,
					On = this.TimeStamp
				};

				model.Modified = new ChangeReceipt()
				{
					By = model.Id,
					On = this.TimeStamp
				};

				// update the entity
				model = Update(model);

				// log the creation
				this.Logger.Info(string.Format("created provider member for email {0} with ID {1}", model.Email, model.Id));

				// return the created model
				return model;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to create provider member for email {0}. Code: {1}, Reason: {2}", model.Email, result.Code, result.ErrorMessage));
			}

			return null;
		}
		#endregion

		#region Update Member
		public ProviderMember UpdateMember(ProviderMember member)
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
				this.Logger.Info(string.Format("successfully updated provider member for ID {0} via user {1}", member.Id, this.AccountSession.MemberId));

				// return the updated entity
				return entity;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to update provider member for ID {0} because user does not exist", member.Id));
			}

			return null;
		}
		#endregion
	}
}

