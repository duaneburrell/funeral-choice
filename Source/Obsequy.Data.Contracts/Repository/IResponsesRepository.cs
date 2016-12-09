using MongoRepository;
using Obsequy.Communication;
using Obsequy.Model;
using System.Collections.Generic;

namespace Obsequy.Data.Contracts
{
	public interface IResponsesRepository : IRepository<Response>, IAccountSession, ISwitchboard
	{
		// get a single response by its object ID
		Response FindById(string id);

		// get the response schemes for the specified ID
		ConsumerResponseScheme GetConsumerResponseScheme(string id);

		// get the response schemes for the specified consumer member ID
		List<ConsumerResponseScheme> GetConsumerMemberResponseSchemes(string memberId);

		// get the response schemes for the specified consumer portfolio ID
		List<ConsumerResponseScheme> GetConsumerPortfolioResponseSchemes(string portfolioId);

		// get the response schemes for the specified ID
		ProviderResponseScheme GetProviderResponseScheme(string id);

		// get the response schemes for the specified provider member ID
		List<ProviderResponseScheme> GetProviderMemberResponseSchemes(string memberId);

		// get the response schemes for the specified provider portfolio ID
		List<ProviderResponseScheme> GetProviderPortfolioResponseSchemes(string portfolioId);

		// create the specified response for the provider and consumer tuple
		Response Create(string providerPortfolioId, string consumerPortfolioId);

		// update the specified response for the specified Id
		Response Update(string id, Response response);

		// update the response state as accepted
		ConsumerResponseScheme UpdateResponseAsAccepted(string id, Payment payment);

		// update the response state as available
		ProviderResponseScheme UpdateResponseAsAvailable(ProviderResponseScheme response);

		// update the response state as dismissed
		ProviderResponseScheme UpdateResponseAsDismissed(ProviderResponseScheme response);

		// update the response state as pending
		ProviderResponseScheme UpdateResponseAsPending(ProviderResponseScheme response);

		// update the response state as recalled
		ProviderResponseScheme UpdateResponseAsRecalled(ProviderResponseScheme response);

		// update the response state as rejected
		ConsumerResponseScheme UpdateResponseAsRejected(ConsumerResponseScheme response);
	}
}
