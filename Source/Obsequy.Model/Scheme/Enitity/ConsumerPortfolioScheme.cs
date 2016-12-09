using System.Collections.Generic;
using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ConsumerPortfolioScheme
	{
		#region Properties

		public string Id { get; set; }
		public AccountStatus AccountStatus { get; set; }

		public ConsumerPrincipal Principal { get; set; }

		public ConsumerPreference Preference { get; set; }
		public ConsumerSchedule Schedule { get; set; }

		public ConsumerRequest Request { get; set; }

		public Payment Payment { get; set; }

		public List<ConsumerMember> Members { get; set; }

		public ChangeReceipt Created { get; set; }
		public ChangeReceipt Modified { get; set; }
        public ChangeReceipt Reminded { get; set; }

		public bool CanDelete { get; set; }
		public bool CanSave { get; set; }
		public bool CanSubmit { get; set; }

		public int ChosenOptionsCount { get; set; }
		public int PercentComplete { get; set; }
		public int TotalOptionsCount { get; set; }

		#endregion

		#region Constructors
		public ConsumerPortfolioScheme()
		{

		}

		public ConsumerPortfolioScheme(ConsumerPortfolio consumerPortfolio)
		{
			this.Id = consumerPortfolio.Id;
			this.AccountStatus = consumerPortfolio.AccountStatus;
			
			this.Principal = consumerPortfolio.Principal;
			this.Preference = consumerPortfolio.Preference;
			this.Schedule = consumerPortfolio.Schedule;

			this.Request = consumerPortfolio.Request;

			this.Payment = consumerPortfolio.Payment;

			this.Members = new List<ConsumerMember>();

			this.Created = consumerPortfolio.Created;
            this.Modified = consumerPortfolio.Modified;
            this.Reminded = consumerPortfolio.Reminded;

			this.CanDelete = consumerPortfolio.CanDelete;
			this.CanSave = consumerPortfolio.CanSave;
			this.CanSubmit = consumerPortfolio.CanSubmit;

			this.ChosenOptionsCount = consumerPortfolio.ChosenOptionsCount;
			this.PercentComplete = consumerPortfolio.PercentComplete;
			this.TotalOptionsCount = consumerPortfolio.TotalOptionsCount;
		}

		#endregion

		#region Methods

		#region To String
		public override string ToString()
		{
			return string.Format("{0} - {1}", this.Request.State, this.Principal.FullName);
		}
		#endregion

		#endregion
	}
}
