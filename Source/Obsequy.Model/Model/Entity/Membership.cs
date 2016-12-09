using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;

namespace Obsequy.Model
{
	[CollectionName("Memberships")]
	public class Membership
	{
		#region Data Properties

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string ApplicationName { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime CreationDate { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Email { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int FailedPasswordAnswerAttemptCount { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int FailedPasswordAttemptCount { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime FailedPasswordAttemptWindowStart { get; set; }

		[BsonRepresentation(BsonType.Boolean)]
		public bool IsApproved { get; set; }

		[BsonRepresentation(BsonType.Boolean)]
		public bool IsLockedOut { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime LastActivityDate { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime LastLockoutDate { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime LastLoginDate { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime LastPasswordChangedDate { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string PasswordQuestion { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Username { get; set; }

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public Membership() 
        {
		}

		#endregion
	}
}
