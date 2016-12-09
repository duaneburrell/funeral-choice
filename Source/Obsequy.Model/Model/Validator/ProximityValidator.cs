using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ProximityValidator : ModelAbstractValidator<Proximity>
	{
		public ProximityValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region City
			RuleFor(i => i.City)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("City is required")
					.WithValidationContext(ValidationStatus.Required)
				.Length(1, 64)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("City must be 64 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region State
			RuleFor(i => i.State)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("State is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureState(true)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("The selected state is invalid")
					.WithValidationContext(ValidationStatus.Invalid)
				.Length(1, 32)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("State must be 32 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion
		}
	}
}
