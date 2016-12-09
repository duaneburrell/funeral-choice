using MongoRepository;
using Obsequy.Communication;
using Obsequy.Model;
using Obsequy.Utility;
using System.Collections.Generic;

namespace Obsequy.Data.Contracts
{
	public interface IConsumerPortfoliosRepository : IRepository<ConsumerPortfolio>, IAccountSession, ISwitchboard
	{
		ConsumerPortfolio FindById(string id);
		ConsumerPortfolioScheme GetScheme(string id);
		List<ConsumerPortfolio> GetAll();

		ConsumerPortfolio CreatePortfolio(ConsumerPortfolio portfolio);
		ConsumerPortfolio UpdatePortfolio(ConsumerPortfolio portfolio);

		void DeletePortfolio(string id);

		ConsumerPortfolio UpdatePortfolioAsPending(string id);
		ConsumerPortfolio UpdatePortfolioAsDraft(string id);

        void RemindPortfolio(string id);
	}
}
