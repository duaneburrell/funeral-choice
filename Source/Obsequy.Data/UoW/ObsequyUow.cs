using Obsequy.Communication;
using Obsequy.Data.Contracts;
using Obsequy.Model;
using System;

namespace Obsequy.Data
{
	/// <summary>
	/// The "Unit of Work"
	///     1) decouples the repos from the controllers
	///     2) decouples the DbContext and EF from the controllers
	///     3) manages the UoW
	/// </summary>
	/// <remarks>
	/// This class implements the "Unit of Work" pattern in which
	/// the "UoW" serves as a facade for querying and saving to the database.
	/// Querying is delegated to "repositories".
	/// Each repository serves as a container dedicated to a particular
	/// root entity type such as a <see cref="Person"/>.
	/// A repository typically exposes "Get" methods for querying and
	/// will offer add, update, and delete methods if those features are supported.
	/// The repositories rely on their parent UoW to provide the interface to the
	/// data layer (which is the EF DbContext).
	/// </remarks>
	public class ObsequyUow : IObsequyUow, IDisposable
	{
		public AccountSession AccountSession { get; set; }
		public Switchboard Switchboard { get; set; }

		public ObsequyUow(IRepositoryProvider repositoryProvider)
		{
			RepositoryProvider = repositoryProvider;
		}

		// Repositories
		public IAdministratorsRepository Administrators { get { return GetRepo<IAdministratorsRepository>(); } }
		public IConsumerPortfoliosRepository ConsumerPortfolios { get { return GetRepo<IConsumerPortfoliosRepository>(); } }
		public IConsumerMembersRepository ConsumerMembers { get { return GetRepo<IConsumerMembersRepository>(); } }
		public IProviderPortfoliosRepository ProviderPortfolios { get { return GetRepo<IProviderPortfoliosRepository>(); } }
		public IProviderMembersRepository ProviderMembers { get { return GetRepo<IProviderMembersRepository>(); } }
		public IResponsesRepository Responses { get { return GetRepo<IResponsesRepository>(); } }

		/// <summary>
		/// Save pending changes to the database
		/// </summary>
		public void Commit()
		{
			//System.Diagnostics.Debug.WriteLine("Committed");
			//DbContext.SaveChanges();
		}

		protected IRepositoryProvider RepositoryProvider { get; set; }

		private MongoRepository.IRepository<T> GetStandardRepo<T>() where T : MongoRepository.IEntity
		{
			return this.RepositoryProvider.GetRepositoryForEntityType<T>();
		}

		private T GetRepo<T>() where T : class
		{
			var repository = RepositoryProvider.GetRepository<T>();

			if (repository is IAccountSession)
				(repository as IAccountSession).AccountSession = this.AccountSession;

			if (repository is ISwitchboard)
				(repository as ISwitchboard).Switchboard = this.Switchboard;

			return repository;
		}

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
		}

		#endregion
	}
}