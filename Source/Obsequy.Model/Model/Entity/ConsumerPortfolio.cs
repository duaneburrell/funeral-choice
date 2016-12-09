using System;
using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;

namespace Obsequy.Model
{
	[CollectionName(Definitions.Database.ConsumerPortfolioDocuments)]
	public class ConsumerPortfolio : IEntity
	{
		#region Data Properties

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AccountStatus AccountStatus { get; set; }

		public ConsumerPrincipal Principal { get; set; }

		public ConsumerPreference Preference { get; set; }

		public ConsumerSchedule Schedule { get; set; }

		public ConsumerRequest Request { get; set; }

		public Payment Payment { get; set; }

		public ChangeReceipt Created { get; set; }

        public ChangeReceipt Modified { get; set; }
        
        public ChangeReceipt Reminded { get; set; }

		#endregion

		#region Computed Properties

		[BsonIgnore]
		public bool CanDelete
		{
			get { return this.Request.Current != null && this.Request.Current.IsDraft ? true : false; }
		}

		[BsonIgnore]
		public bool CanSave
		{
			get { return this.Request.Current != null && this.Request.Current.IsDraft ? true : false; }
		}

		[BsonIgnore]
		public bool CanSubmit
		{
			get { return (this.Request.Current == null || this.Request.Current.IsDraft) && this.Principal.IsCompleted && this.Preference.IsCompleted && this.Schedule.IsCompleted; }
		}

		[BsonIgnore]
		public int ChosenOptionsCount
		{
			get { return this.Principal.ChosenOptionsCount + this.Preference.ChosenOptionsCount + this.Schedule.ChosenOptionsCount; }
		}

		[BsonIgnore]
		public int PercentComplete
		{
			get { return Convert.ToInt32(((double)this.ChosenOptionsCount) / ((double)this.TotalOptionsCount) * 100); }
		}

		[BsonIgnore]
		public int TotalOptionsCount
		{
			get { return this.Principal.TotalOptionsCount + this.Preference.TotalOptionsCount + this.Schedule.TotalOptionsCount; }
		}

		#endregion

		#region Constructors

		public ConsumerPortfolio()
		{
			this.Principal = new ConsumerPrincipal();
			this.Preference = new ConsumerPreference();
			this.Schedule = new ConsumerSchedule();
			this.Request = new ConsumerRequest();

            this.Created = new ChangeReceipt();
            this.Modified = new ChangeReceipt();
            this.Reminded = new ChangeReceipt();
		}

		#endregion

		#region Methods

		#region Has Changed
		public bool HasChanged(ConsumerPortfolio portfolio)
		{
			if (this.Principal.HasChanged(portfolio.Principal))
				return true;

			if (this.Preference.HasChanged(portfolio.Preference))
				return true;

			if (this.Schedule.HasChanged(portfolio.Schedule))
				return true;

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0} - {1}", this.Request.State, this.Principal.FullName);
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerPortfolioValidator(accountSession, validationMode, this)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}