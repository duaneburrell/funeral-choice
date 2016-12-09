using MongoRepository;
using Obsequy.Communication;
using Obsequy.Model;
using System.Collections.Generic;

namespace Obsequy.Data.Contracts
{
	public interface IProviderMembersRepository : IRepository<ProviderMember>, IAccountSession, ISwitchboard
	{
		ProviderMember FindById(string id);
		ProviderMember GetScheme(string memberId, string portfoliId = null);

		ProviderMember CreateMember(ProviderRegistrationForm registrationForm, string membershipId);
		ProviderMember UpdateMember(ProviderMember member);
	}
}
