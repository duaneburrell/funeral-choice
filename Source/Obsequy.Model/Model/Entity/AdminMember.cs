using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;

namespace Obsequy.Model
{
	[CollectionName(Definitions.Database.AdminMemberDocuments)]
	public class AdminMember : IEntity
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
		[BsonRepresentation(BsonType.String)]
		public string MembershipId { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Email { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string FirstName { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string LastName { get; set; }

		[BsonRepresentation(BsonType.Boolean)]
		public bool IsNotifiedOnConsumerRegistrations { get; set; }

		[BsonRepresentation(BsonType.Boolean)]
		public bool IsNotifiedOnProviderRegistrations { get; set; }
	
		[BsonRepresentation(BsonType.Boolean)]
		public bool IsNotifiedOnAcceptedResponses { get; set; }

		[BsonRepresentation(BsonType.Boolean)]
		public bool IsNotifiedOnExceptions { get; set; }

		public ChangeReceipt Created { get; set; }

		public ChangeReceipt Modified { get; set; }

		#endregion

		#region Computed Properties

		#region Full Name
		[BsonIgnore]
		public string FullName
		{
			get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
		}
		#endregion

		#region Empty
		[BsonIgnore]
		public static AdminMember Empty
		{
			get { return null; }
		}
		#endregion

		#endregion

		#region Constructors

		public AdminMember()
			: base()
		{
		}
	
		#endregion

		#region Methods

		#region Has Changed
		public bool HasChanged(AdminMember member)
		{
			if (this.AccountStatus != member.AccountStatus ||
				this.Email != member.Email.Scrub() ||
				this.FirstName != member.FirstName.Scrub() ||
				this.LastName != member.LastName.Scrub() ||
				this.IsNotifiedOnConsumerRegistrations != member.IsNotifiedOnConsumerRegistrations ||
				this.IsNotifiedOnProviderRegistrations != member.IsNotifiedOnProviderRegistrations ||
				this.IsNotifiedOnAcceptedResponses != member.IsNotifiedOnAcceptedResponses ||
				this.IsNotifiedOnExceptions != member.IsNotifiedOnExceptions)
				return true;

			return false;
		}

		#region Update
		public void Update(AdminMember member)
		{
			// update all values
			this.AccountStatus = member.AccountStatus;
			this.Email = member.Email.Scrub();
			this.FirstName = member.FirstName.Scrub();
			this.LastName = member.LastName.Scrub();

			this.IsNotifiedOnConsumerRegistrations = member.IsNotifiedOnConsumerRegistrations;
			this.IsNotifiedOnProviderRegistrations = member.IsNotifiedOnProviderRegistrations;
			this.IsNotifiedOnAcceptedResponses = member.IsNotifiedOnAcceptedResponses;
			this.IsNotifiedOnExceptions = member.IsNotifiedOnExceptions;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1} {2}", Email, FirstName, LastName);
		}
		#endregion

		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new AdminMemberValidator(accountSession, validationMode, this)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
