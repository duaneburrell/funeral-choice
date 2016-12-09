using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;
using Obsequy.Utility;
using System.Collections.Generic;

namespace Obsequy.Model
{
	[CollectionName(Definitions.Database.ConsumerMemberDocuments)]
	public class ConsumerMember : IEntity
	{
		#region Data Properties

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AccountType AccountType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AccountStatus AccountStatus { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AccountPrestige AccountPrestige { get; set; }

		// [BsonRepresentation(BsonType.ObjectId)]
		// note: the provider ID is set to a GUID. For now, this is OK, but I wonder if it will have any affect later 
		// see: https://github.com/freshlogic/MongoDB.Web/blob/master/MongoDB.Web/Providers/MongoDBMembershipProvider.cs line 177
		[JsonIgnore]
		[BsonRepresentation(BsonType.String)]
		public string MembershipId { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Email { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string FirstName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string LastName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Phone { get; set; }

		public Address Address { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string PortfolioId { get; set; }

		[JsonIgnore]
		public List<string> PortfolioIds { get; set; }
		
		public ChangeReceipt Created { get; set; }

		public ChangeReceipt Modified { get; set; }

		#endregion

		#region Computed Properties

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

		#region Portfolio Schemes

		[BsonIgnore()]
		public List<ConsumerPortfolioScheme> Portfolios { get; set; }
		
		#endregion

		#region Empty
		[BsonIgnore]
		public static ConsumerMember Empty
		{
			get { return null; }
		}
		#endregion

		#endregion

		#region Constructors

		public ConsumerMember()
			: base()
		{
			this.Address = new Address();

			this.Portfolios = new List<ConsumerPortfolioScheme>();
		}

		public ConsumerMember(ConsumerRegistrationForm form, object membershipId)
			: base()
		{
			// account information
			this.AccountType = AccountType.Consumer;
			this.AccountStatus = AccountStatus.Active;
			this.AccountPrestige = AccountPrestige.Creator;

			// membership
			this.MembershipId = membershipId.ToString();
			this.Address = new Address();

			// portfolio
			this.PortfolioIds = new List<string>();
			this.PortfolioId = string.Empty;

			this.Portfolios = new List<ConsumerPortfolioScheme>();

			// update the properties
			Update(form.Member);
		}
	
		#endregion

		#region Methods

		#region Has Changed
		public bool HasChanged(ConsumerMember member)
		{
			if (this.Address.HasChanged(member.Address))
				return true;

			if (this.Email != member.Email.Scrub() ||
				this.FirstName != member.FirstName.Scrub() ||
				this.LastName != member.LastName.Scrub() ||
				this.Phone != member.Phone.Scrub())
				return true;

			return false;
		}
		#endregion

		#region Update
		public void Update(ConsumerMember member)
		{
			// update all values
			this.Email = member.Email.Scrub();
			this.FirstName = member.FirstName.Scrub();
			this.LastName = member.LastName.Scrub();
			this.Phone = member.Phone.Scrub();

			// update the address
			this.Address.Update(member.Address);
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1} {2}", Email, FirstName, LastName);
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerMemberValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
