using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class PaymentValidator : ModelAbstractValidator<Payment>
	{
		// Note: The client is designed to not encypt empty strings, so this is really handling those checks. Any data type checks are done on the credit card processing site.
		public PaymentValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Cardholder Name
			RuleFor(i => i.CardholderName)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Accept))
					.WithMessage("Cardholder name is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Card Number
			RuleFor(i => i.CardNumber)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Accept))
					.WithMessage("Card number is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Expiration Month
			RuleFor(i => i.ExpirationMonth)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Accept))
					.WithMessage("Expiration month is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Expiration Year
			RuleFor(i => i.ExpirationYear)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Accept))
					.WithMessage("Expiration year is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Security Code
			RuleFor(i => i.SecurityCode)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Accept))
					.WithMessage("Security code is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Postal Code
			RuleFor(i => i.PostalCode)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Accept))
					.WithMessage("Postal code is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion
		}
	}
}
