using FluentValidation;
using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class LoginFormValidator : ModelAbstractValidator<LoginForm>
	{
		public class LoginFormValidationResult : ModelValidationResult
		{
			// Summary:
			//     Creates a new model validation result.
			public LoginFormValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public LoginFormValidator()
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
			RuleFor(i => i.Password)
				.NotEmpty()
					.WithMessage("Please specify your password")
					.WithValidationContext(ValidationStatus.Required);
			#endregion
		}

		public FluentValidation.Results.ValidationResult AsInvalidPassword(LoginForm instance)
		{
			// generate an 'invalid password' fluent error
			var baseValidationResult = new ValidationResult();
			var modelValidationResult = new ModelValidationResult(baseValidationResult);

			modelValidationResult.Errors.Add(new PropertyValidationFailure("Password", "The password is invalid")
			{
				Status = FormatHelper.ToCamlCase("Invalid"),
				Severity = FormatHelper.ToCamlCase("Error")
			});

			return modelValidationResult;
		}
	}
}