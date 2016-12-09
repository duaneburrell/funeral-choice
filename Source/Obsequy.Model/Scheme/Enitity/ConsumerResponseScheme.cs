using FluentValidation.Results;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Obsequy.Model
{
	public class ConsumerResponseScheme
	{
		#region Data Properties

		public string Id { get; set; }
		public string ConsumerPortfolioId { get; set; }
		public string ProviderPortfolioId { get; set; }

		public double? Distance { get; set; }
		public double? Quote { get; set; }
		public double? DepositDue { get; set; }
		public double? DepositPaid { get; set; }
		public double? BalanceDue { get; set; }
		public double? BalancePaid { get; set; }

		public ResponseAgreement Agreement { get; set; }
		public ResponseAlternate Alternate { get; set; }

		public ProviderPrincipal Principal { get; set; }
		public ProviderProfile Profile { get; set; }

		public ConsumerPreference Preference { get; protected set; }
		public ConsumerSchedule Schedule { get; protected set; }

		public ResponseReceipt Created { get; set; }
		public ResponseReceipt Current { get; set; }
		public ResponseReceipt Pending { get; set; }

		public bool CanAccept { get; protected set; }
		public bool CanReject { get; protected set; }

		#endregion

		#region Computed Properties

		[BsonIgnore]
		public DateTime? LastActivity
		{
			get
			{
				// set the last activity to the current date
				var lastActivity = Current != null ? Current.On : null;

				if (Pending != null && lastActivity < Pending.On)
					lastActivity = Pending.On;

				return lastActivity;
			}
		}

		#endregion

		#region Constructors

		public ConsumerResponseScheme()
		{

		}

		public ConsumerResponseScheme(Response response, ConsumerPortfolio consumerPortfolio, ProviderPortfolio providerPortfolio)
		{
			this.Id = response.Id;
			this.ConsumerPortfolioId = response.ConsumerPortfolioId;
			this.ProviderPortfolioId = response.ProviderPortfolioId;
			this.Distance = response.Distance;
			this.Quote = response.Quote;
			this.DepositDue = response.DepositDue;
			this.DepositPaid = response.DepositPaid;
			this.BalanceDue = response.BalanceDue;
			this.BalancePaid = response.BalancePaid;

			this.Agreement = response.Agreement;
			this.Alternate = response.Alternate;

			this.Principal = (response.Current.IsAccepted ? providerPortfolio.Principal : new ProviderPrincipal());
			this.Profile = (response.Current.IsAccepted ? providerPortfolio.Profile : new ProviderProfile()
				{
					BusinessEstablished = providerPortfolio.Profile.BusinessEstablished,
					FacilityAge = providerPortfolio.Profile.FacilityAge,
					FacilityStyle = providerPortfolio.Profile.FacilityStyle,
					FuneralDirectorExperience = providerPortfolio.Profile.FuneralDirectorExperience,
					TransportationFleetAge = providerPortfolio.Profile.TransportationFleetAge
				});

			this.Preference = consumerPortfolio.Preference;
			this.Schedule = consumerPortfolio.Schedule;

			this.Created = response.Created;
			this.Current = response.Current;
			this.Pending = response.Pending;

			this.CanAccept = (consumerPortfolio.Request.IsAcceptAllowed && response.CanBecomeAccepted);
			this.CanReject = (consumerPortfolio.Request.IsRejectAllowed && response.CanBecomeRejected);
		}

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerResponseSchemeValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
