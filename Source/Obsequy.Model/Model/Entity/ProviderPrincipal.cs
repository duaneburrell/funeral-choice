using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Obsequy.Model
{
	public class ProviderPrincipal
	{
		#region Data Properties

		[BsonRepresentation(BsonType.String)]
		public string Name { get; set; }

		public Address Address { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Phone { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Email { get; set; }

		#endregion

		#region Computed Properties

		#region Google Map Url
		[BsonIgnore]
		public string GoogleMapUrl
		{
			get
			{
				if (this.Address.HasLocation)
					return string.Format("http://www.google.com/maps?daddr={0},+{1},+{2}+{3}@{4},{5}", this.Name, this.Address.Line1, this.Address.City, this.Address.Zip, this.Address.Latitude, this.Address.Longitude);
				return string.Empty;
			}
		}
		#endregion

		#region Is Complete
		[BsonIgnore]
		public bool IsComplete
		{
			get 
			{ 
				return (
					!string.IsNullOrEmpty(this.Name) &&
					this.Address.HasLocation &&
					!string.IsNullOrEmpty(this.Phone) &&
					!string.IsNullOrEmpty(this.Email)
					); }
		}
		#endregion

		#endregion

		#region Constructors

		public ProviderPrincipal()
			: base()
		{
			// create members
			this.Address = new Address();
		}

		public ProviderPrincipal(ProviderPrincipal principal)
			: base()
		{
			// create members
			this.Address = new Address();

			// update
			Update(principal);
		}

		#endregion

		#region Methods

		#region Clone
		public ProviderPrincipal Clone()
		{
			return new ProviderPrincipal()
			{
				Name = this.Name,
				Address = this.Address.Clone(),
				Phone = this.Phone,
				Email = this.Email
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ProviderPrincipal principal)
		{
			if (this.Address.HasChanged(principal.Address))
				return true;

			if (this.Name != principal.Name.Scrub())
				return true;

			if (this.Phone != principal.Phone.Scrub())
				return true;

			if (this.Email != principal.Email.Scrub())
				return true;

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1}", Name, Address);
		}
		#endregion

		#region Update
		public void Update(ProviderPrincipal principal)
		{
			// update all values
			this.Name = principal.Name.Scrub();

			// update the address
			this.Address.Update(principal.Address);

			// update the phone
			this.Phone = principal.Phone.Scrub();

			//update the email
			this.Email = principal.Email.Scrub();
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ProviderPrincipalValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}