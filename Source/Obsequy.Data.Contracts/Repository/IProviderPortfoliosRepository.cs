using MongoRepository;
using Obsequy.Communication;
using Obsequy.Model;
using System.Collections.Generic;

namespace Obsequy.Data.Contracts
{
	public interface IProviderPortfoliosRepository : IRepository<ProviderPortfolio>, IAccountSession, ISwitchboard
	{
		ProviderPortfolio FindById(string id);
		ProviderPortfolioScheme GetScheme(string id);

		List<ProviderPortfolio> GetProviderPortfolios();
		List<ProviderPortfolio> GetPendingProviderPortfolios();
		List<ProviderPortfolio> GetActiveProviderPortfolios();
		List<ProviderPortfolio> GetSuspendedProviderPortfolios();
		List<ProviderPortfolio> GetRejectedProviderGroups();

		ProviderPortfolio CreatePortfolio(ProviderPortfolio portfolio);
		ProviderPortfolio UpdatePortfolio(ProviderPortfolio portfolio);

		ProviderPortfolio UpdateAccountStatus(ProviderPortfolio portfolio);
	}
}
