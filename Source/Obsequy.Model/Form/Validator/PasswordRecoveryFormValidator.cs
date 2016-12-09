using FluentValidation;
using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class PasswordRecoveryFormValidator : ModelAbstractValidator<PasswordRecoveryForm>
	{
		public PasswordRecoveryFormValidator()
			: base(null, ValidationMode.None)
		{
			#region Email
			RuleFor(i => i.Email)
				.NotEmpty()
					.WithMessage("Please specifiy your email address")
					.WithValidationContext(ValidationStatus.Required)
				.EmailAddress()
					.WithMessage("The email address format is invalid")
					.WithValidationContext(ValidationStatus.Invalid)
				.EnsureValidEmail()
					.WithMessage("The email address is not in the system")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Password
			//RuleFor(i => i.Password)
			//	.NotEmpty()
			//		.WithMessage("Please specify your password")
			//		.WithValidationContext(ValidationStatus.Required);
			#endregion
		}
	}
}