using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class AddressValidator : ModelAbstractValidator<Address>
	{
		public AddressValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Line 1
			RuleFor(i => i.Line1)
				.NotEmpty()
					.WithMessage("Line 1 is required")
					.WithValidationContext(ValidationStatus.Required)
				.Length(1, 64)
					.WithMessage("Line 1 must be 64 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Line 2
			RuleFor(i => i.Line2)
				.Length(1, 64)
					.WithMessage("Line 2 must be 64 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region City
			RuleFor(i => i.City)
				.NotEmpty()
					.WithMessage("City is required")
					.WithValidationContext(ValidationStatus.Required)
				.Length(1, 64)
					.WithMessage("City must be 64 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region State
			RuleFor(i => i.State)
				.NotEmpty()
					.WithMessage("State is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureState(false)
					.WithMessage("The selected state is invalid")
					.WithValidationContext(ValidationStatus.Invalid)
				.Length(1, 32)
					.WithMessage("State must be 32 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Zip
			RuleFor(i => i.Zip)
				.NotEmpty()
					.WithMessage("Zip is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureZip()
					.WithMessage("The zip code is invalid")
					.WithValidationContext(ValidationStatus.Invalid)
				.Length(1, 32)
					.WithMessage("Zip must be 32 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion
		}
	}
}
