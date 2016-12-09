using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	[CollectionName(Definitions.Database.ProviderPortfolioDocuments)]
	public class ProviderPortfolio : IEntity
	{
		#region Data Properties

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AccountStatus AccountStatus { get; set; }

		public ProviderPrincipal Principal { get; set; }

		public ProviderProfile Profile { get; set; }

		public ChangeReceipt Created { get; set; }

		public ChangeReceipt Modified { get; set; }

		#endregion

		#region Computed Properties

		#region Is Complete
		[BsonIgnore]
		public bool IsComplete
		{
			get { return (this.Principal.IsComplete && this.Profile.IsComplete); }
		}
		#endregion

		#endregion

		#region Constructors

		public ProviderPortfolio()
		{
			this.Principal = new ProviderPrincipal();
			this.Profile = new ProviderProfile();
			this.Created = new ChangeReceipt();
			this.Modified = new ChangeReceipt();
		}

		#endregion

		#region Methods

		#region Has Changed
		public bool HasChanged(ProviderPortfolio portfolio)
		{
			if (this.Principal.HasChanged(portfolio.Principal))
				return true;

			if (this.Profile.HasChanged(portfolio.Profile))
				return true;

			return false;
		}
		#endregion

		#region Is Geo-Coding Required
		public bool IsGeoCodingRequired()
		{
			//if (this.Preferences != null && this.Preferences.Proximity.IsGeoCodingRequired(dto.Proximity))
			//	return true;

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1}", Principal.Name, AccountStatus);
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ProviderPortfolioValidator(accountSession, validationMode, this)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}