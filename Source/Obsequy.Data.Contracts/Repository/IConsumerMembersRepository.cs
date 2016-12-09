using MongoRepository;
using Obsequy.Communication;
using Obsequy.Model;
using System.Collections.Generic;

namespace Obsequy.Data.Contracts
{
	public interface IConsumerMembersRepository : IRepository<ConsumerMember>, IAccountSession, ISwitchboard
    {
		ConsumerMember FindById(string id);
		ConsumerMember GetScheme(string memberId, string portfoliId = null);

		ConsumerMember CreateMember(ConsumerRegistrationForm registrationForm, string membershipId);
		ConsumerMember UpdateMember(ConsumerMember member);
    }
}

