
using Obsequy.Communication;
namespace Obsequy.Data.Contracts
{
    /// <summary>
    /// Interface for the AES "Unit of Work"
    /// </summary>
    public interface IObsequyUow : IAccountSession, ISwitchboard
    {
        //void SetAsSystemSession();

        // Save pending changes to the data store.
        void Commit();

        // Repositories
		IAdministratorsRepository Administrators { get; }
		IConsumerPortfoliosRepository ConsumerPortfolios { get; }
		IConsumerMembersRepository ConsumerMembers { get; }
		IProviderPortfoliosRepository ProviderPortfolios { get; }
		IProviderMembersRepository ProviderMembers { get; }
		IResponsesRepository Responses { get; }
    }
}
