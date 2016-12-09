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
	public class ProviderResponseScheme
	{
		#region Properties

		public string Id { get; set; }
		public string ConsumerPortfolioId { get; set; }
		public string ProviderPortfolioId { get; set; }

		public double? Distance { get; protected set; }
		public double? Quote { get; set; }
		public double? DepositDue { get; set; }
		public double? DepositPaid { get; set; }
		public double? BalanceDue { get; set; }
		public double? BalancePaid { get; set; }

		public ResponseAgreement Agreement { get; set; }
		public ResponseAlternate Alternate { get; set; }

		public ResponseReceipt Created { get; set; }
		public ResponseReceipt Current { get; set; }
		public ResponseReceipt Pending { get; set; }

		public ConsumerMember Member { get; set; }
		public ConsumerPrincipal Principal { get; set; }

		public ConsumerPreference Preference { get; protected set; }
		public ConsumerSchedule Schedule { get; protected set; }
		public ConsumerRequest Request { get; protected set; }

		public bool CanBecomeAvailable { get; protected set; }
		public bool CanBecomeDismissed { get; protected set; }
		public bool CanBecomePending { get; protected set; }

		#endregion

		#region Computed Properties

		[BsonIgnore]
		public DateTime? LastActivity
		{
			get
			{
				if (Request != null ) {
					// set the last activity to the requests date
					var lastActivity = Request.Current.On;

					if (Current != null && lastActivity < Current.On)
						lastActivity = Current.On;

					if (Pending != null && lastActivity < Pending.On)
						lastActivity = Pending.On;

					return lastActivity;
				}

				return null;
			}
		}

		#endregion

		#region Constructors

		public ProviderResponseScheme()
		{

		}

		public ProviderResponseScheme(Response response, ProviderPortfolio providerPortfolio, ConsumerPortfolio consumerPortfolio, ConsumerMember consumerMember = null)
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

			this.Created = response.Created;
			this.Current = response.Current;
			this.Pending = response.Pending;

			this.Member = GetConsumerMember(response, consumerMember); // only show if winning bidder
			this.Principal = (response.Current.IsAccepted ? consumerPortfolio.Principal : new ConsumerPrincipal()); // only show if winning bidder
			this.Preference = consumerPortfolio.Preference.Clone();
			this.Schedule = consumerPortfolio.Schedule.Clone();
			this.Request = consumerPortfolio.Request;

			this.CanBecomeAvailable = response.CanBecomeAvailable;
			this.CanBecomePending = response.CanBecomePending;
			this.CanBecomeDismissed = response.CanBecomeDismissed;
		}

		#endregion

		#region Methods
		private ConsumerMember GetConsumerMember(Response response, ConsumerMember consumerMember)
		{
			var member = new ConsumerMember();

			if (response.Current.IsAccepted && consumerMember != null)
			{
				// only return specific properties
				member.FirstName = consumerMember.FirstName;
				member.LastName = consumerMember.LastName;
				member.Email = consumerMember.Email;
				member.Phone = consumerMember.Phone;
			}

			return member;
		}
		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ProviderResponseSchemeValidator(accountSession, validationMode, this)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
