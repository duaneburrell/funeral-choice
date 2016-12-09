using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ConsumerPrincipal
	{
		#region Data Properties

		[BsonRepresentation(BsonType.String)]
		public string FirstName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string LastName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Phone { get; set; }

		public Address Address { get; set; }

		#endregion

		#region Computed Properties

		#region Chosen Options Count
		[BsonIgnore]
		public int ChosenOptionsCount
		{
			get
			{
				var count = 0;

				count += (!string.IsNullOrEmpty(this.FirstName) ? 1 : 0);
				count += (!string.IsNullOrEmpty(this.LastName) ? 1 : 0);

				return count;
			}
		}
		#endregion

		#region Full Name
		[BsonIgnore]
		public string FullName
		{
			get 
			{
				if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
					return string.Format("{0} {1}", FirstName, LastName);
				return string.Empty;
			}
		}
		#endregion

		#region Is Completed
		[BsonIgnore]
		public bool IsCompleted
		{
			get { return (this.ChosenOptionsCount == this.TotalOptionsCount); }
		}
		#endregion

		#region Percent Complete
		[BsonIgnore]
		public double PercentComplete
		{
			get { return (((double)this.ChosenOptionsCount) / ((double)this.TotalOptionsCount)); }
		}
		#endregion

		#region Total Options Count
		[BsonIgnore]
		public int TotalOptionsCount
		{
			get
			{
				return 2;
			}
		}
		#endregion

		#endregion

		#region Constructors

		public ConsumerPrincipal()
			: base()
		{
		}

		public ConsumerPrincipal(ConsumerPrincipal principal)
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
		public ConsumerPrincipal Clone()
		{
			return new ConsumerPrincipal()
			{
				FirstName = this.FirstName,
				LastName = this.LastName,
				Phone = this.Phone,
				Address = this.Address.Clone()
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ConsumerPrincipal principal)
		{
			if (this.FirstName != principal.FirstName.Scrub() ||
				this.LastName != principal.LastName.Scrub())
			{
				return true;
			}

			if (this.Phone != principal.Phone.Scrub())
				return true;

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0} {1}", FirstName, LastName);
		}
		#endregion

		#region Update
		public void Update(ConsumerPrincipal principal)
		{
			// update all values
			this.FirstName = principal.FirstName.Scrub();
			this.LastName = principal.LastName.Scrub();

			// update the address
			this.Address.Update(principal.Address);

			// update the phone
			this.Phone = principal.Phone.Scrub();
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerPrincipalValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}