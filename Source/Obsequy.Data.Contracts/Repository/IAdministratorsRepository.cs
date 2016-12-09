using System.Collections.Generic;
using MongoRepository;
using Obsequy.Communication;
using Obsequy.Model;

namespace Obsequy.Data.Contracts
{
	public interface IAdministratorsRepository : IRepository<AdminMember>, IAccountSession, ISwitchboard
    {
		AdminMember FindById(string id);
		List<AdminMember> GetMembers();

		AdminMember CreateMember(AdministratorRegistrationForm registrationForm, string membershipId);
		AdminMember UpdateMember(AdminMember member);
		void DeleteMember(AdminMember member);
    }
}

