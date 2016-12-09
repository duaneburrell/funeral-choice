using Obsequy.Data.Contracts;
using Obsequy.Model;
using Obsequy.Utility;
using System.Linq;
using System.Collections.Generic;

namespace Obsequy.Data
{
	public class AdministratorsRepository : BaseRepository<AdminMember>, IAdministratorsRepository
	{
		#region Get Member(s)
		public AdminMember FindById(string id)
		{
			var entity = this.Collection.FindAll().FirstOrDefault(item => item.Id == id);

			return entity;
		}

		public List<AdminMember> GetMembers()
		{
			return this.Collection.FindAll().ToList();
		}
		#endregion

		#region Create Member
		public AdminMember CreateMember(AdministratorRegistrationForm registrationForm, string membershipId)
		{
			// create the model
			var model = new AdminMember()
			{
				AccountType = AccountType.Administrator,
				AccountStatus = AccountStatus.Active,
				AccountPrestige = AccountPrestige.None,
				MembershipId = membershipId,
				Email = registrationForm.Member.Email.Scrub(),
				FirstName = registrationForm.Member.FirstName.Scrub(),
				LastName = registrationForm.Member.LastName.Scrub(),
				Created = new ChangeReceipt(this.AccountSession),
				Modified = new ChangeReceipt(this.AccountSession),
				IsNotifiedOnConsumerRegistrations = registrationForm.Member.IsNotifiedOnConsumerRegistrations,
				IsNotifiedOnProviderRegistrations = registrationForm.Member.IsNotifiedOnProviderRegistrations,
				IsNotifiedOnAcceptedResponses = registrationForm.Member.IsNotifiedOnAcceptedResponses,
				IsNotifiedOnExceptions = registrationForm.Member.IsNotifiedOnExceptions
			};

			// insert the collection
			var result = this.Collection.Insert(model);

			if (result.Ok)
			{
				// log the creation
				this.Logger.Info(string.Format("created administrator member for email {0} with ID {1}", model.Email, model.Id));

				// return the created model
				return model;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to create administrator member for email {0}. Code: {1}, Reason: {2}", model.Email, result.Code, result.ErrorMessage));
			}

			return null;
		}
		#endregion

		#region Update Member
		public AdminMember UpdateMember(AdminMember member)
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
				this.Logger.Info(string.Format("successfully updated administrator member for ID {0} via user {1}", member.Id, this.AccountSession.MemberId));

				// return the updated entity
				return entity;
			}
			else
			{
				// log the failure
				this.Logger.Error(string.Format("failed to update administrator member for ID {0} because user does not exist", member.Id));
			}

			return null;
		}
		#endregion

		#region Delete Member
		public void DeleteMember(AdminMember member)
		{
			var entity = FindById(member.Id);

			if (entity != null)
			{
				// delete the specified administrator member
				this.Mongo.DeleteAdministratorMember(member.Id);

				// log successful deletion
				this.Logger.Info(string.Format("deleted administrator member for Id {0} via user {1}", member.Id, this.AccountSession.MemberId));
			}
			else
			{
				// log non-existent administrator member
				this.Logger.Warn(string.Format("could not delete administrator member for Id {0} because it does not exist", member.Id));
			}
		}
		#endregion
	}
}

