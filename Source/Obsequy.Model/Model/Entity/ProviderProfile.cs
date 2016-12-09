using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Obsequy.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ProviderProfile
	{
		#region Data Properties

		[BsonRepresentation(BsonType.String)]
		public string Description { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Website { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public BusinessEstablishedTypes BusinessEstablished { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public FacilityAgeTypes FacilityAge { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public FacilityStyleTypes FacilityStyle { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public FuneralDirectorExperienceTypes FuneralDirectorExperience { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public TransportationFleetAgeTypes TransportationFleetAge { get; set; }

		#endregion

		#region Computed Properties

		#region Is Complete
		[BsonIgnore]
		public bool IsComplete
		{
			get 
			{
				return (!string.IsNullOrEmpty(this.Description) &&
					this.BusinessEstablished != BusinessEstablishedTypes.NA &&
					this.FacilityAge != FacilityAgeTypes.NA &&
					this.FacilityStyle != FacilityStyleTypes.NA &&
					this.FuneralDirectorExperience != FuneralDirectorExperienceTypes.NA &&
					this.TransportationFleetAge != TransportationFleetAgeTypes.NA);
			}
		}
		#endregion

		#endregion

		#region Constructors

		public ProviderProfile()
			: base()
		{
		}

		#endregion

		#region Methods

		#region Clone
		public ProviderProfile Clone()
		{
			return new ProviderProfile()
			{
				Description = this.Description,
				Website = this.Website,
				BusinessEstablished = this.BusinessEstablished,
				FacilityAge = this.FacilityAge,
				FacilityStyle = this.FacilityStyle,
				FuneralDirectorExperience = this.FuneralDirectorExperience,
				TransportationFleetAge = this.TransportationFleetAge
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ProviderProfile profile)
		{
			if (this.Description != profile.Description.Scrub())
				return true;

			if (this.Website != profile.Website.Scrub())
				return true;

			if (this.BusinessEstablished != profile.BusinessEstablished ||
				this.FacilityAge != profile.FacilityAge ||
				this.FacilityStyle != profile.FacilityStyle ||
				this.FuneralDirectorExperience != profile.FuneralDirectorExperience ||
				this.TransportationFleetAge != profile.TransportationFleetAge)
				return true;

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}", Website);
		}
		#endregion

		#region Update
		public void Update(ProviderProfile profile)
		{
			this.Description = profile.Description.Scrub();
			string website = profile.Website.Scrub();

			// won't link properly in HTML without http
			if (!string.IsNullOrWhiteSpace(website))
				website = website.StartsWith("http://") ? website : string.Concat("http://", website);

			this.Website = website;

			this.BusinessEstablished = profile.BusinessEstablished;
			this.FacilityAge = profile.FacilityAge;
			this.FacilityStyle = profile.FacilityStyle;
			this.FuneralDirectorExperience = profile.FuneralDirectorExperience;
			this.TransportationFleetAge = profile.TransportationFleetAge;
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ProviderProfileValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}