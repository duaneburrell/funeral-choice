using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using FluentValidation.Results;

namespace Obsequy.Model
{
	public class Payment
	{
		#region Data Properties

		[BsonRepresentation(BsonType.String)]
		public string CardholderName { get; set; }

		[BsonIgnore]
		public string CardNumber { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string CardLastFour { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string CardType { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string ExpirationMonth { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string ExpirationYear { get; set; }

		[BsonIgnore]
		public string SecurityCode { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string PostalCode { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double Amount { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string TransactionId { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string MerchantAccountId { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string ProcessorAuthorizationCode { get; set; }

		#endregion

		#region Computed Properties

		#endregion

		#region Constructors

		public Payment() 
        {
		}

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new PaymentValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
