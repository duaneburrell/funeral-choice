using FluentValidation;
using FluentValidation.Results;

namespace Obsequy.Model
{
    public class PropertyValidator : ModelAbstractValidator<PropertyDto>
    {
		public PropertyValidator(string propertyName, AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Email
			if (propertyName.ToLower() == "email")
			{
				RuleFor(i => i.Email)
					.NotEmpty()
						.WithMessage("Please specifiy an email address")
						.WithValidationContext(ValidationStatus.Required)
					.EmailAddress()
						.WithMessage("The email address format is invalid")
						.WithValidationContext(ValidationStatus.Invalid);

				RuleFor(i => i.Email)
					.EnsureUnusedEmail(this.AccountSession)
					.When(i => (validationMode == ValidationMode.Create))
						.WithMessage("The email address is currently in use")
						.WithValidationContext(ValidationStatus.Invalid);

				RuleFor(i => i.Email)
					.EnsureValidEmail()
					.When(i => (validationMode == ValidationMode.Update))
						.WithMessage("The email address does not exist in the system")
						.WithValidationContext(ValidationStatus.Invalid);
			}
			#endregion

			#region Password
			if (propertyName.ToLower() == "password")
			{
				RuleFor(i => i.Password)
					.NotEmpty()
						.WithMessage("A password is required")
						.WithValidationContext(ValidationStatus.Required)
					.EnsurePasswordStrength()
						.WithMessage("Passwords must have at least 6 characters")
						.WithValidationContext(ValidationStatus.Invalid);
			}
			#endregion
		}
    }
}
