using System.Collections.Generic;
using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ProviderPortfolioScheme
	{
		#region Properties

		public string Id { get; set; }
		public AccountStatus AccountStatus { get; set; }

		public ProviderPrincipal Principal { get; set; }

		public ProviderProfile Profile { get; set; }

		public List<ProviderMember> Members { get; set; }

		public ChangeReceipt Created { get; set; }
		public ChangeReceipt Modified { get; set; }

		#endregion

		#region Constructors
		public ProviderPortfolioScheme()
		{

		}

		public ProviderPortfolioScheme(ProviderPortfolio providerPortfolio)
		{
			this.Id = providerPortfolio.Id;
			this.AccountStatus = providerPortfolio.AccountStatus;
			
			this.Principal = providerPortfolio.Principal;
			this.Profile = providerPortfolio.Profile;

			this.Members = new List<ProviderMember>();

			this.Created = providerPortfolio.Created;
			this.Modified = providerPortfolio.Modified;
		}

		#endregion

		#region Methods

		#endregion
	}
}
