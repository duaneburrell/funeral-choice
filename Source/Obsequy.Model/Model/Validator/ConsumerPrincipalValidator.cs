using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ConsumerPrincipalValidator : ModelAbstractValidator<ConsumerPrincipal>
	{
		public class ConsumerPrincipalValidationResult : ModelValidationResult
		{
			// Summary:
			//     Is Valid is dependent on all child models
			public override bool IsValid
			{
				get
				{
					if (!base.IsValid)
						return false;

					return true;
				}
			}

			// Summary:
			//     Creates a new model validation result.
			public ConsumerPrincipalValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ConsumerPrincipalValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region First Name
			RuleFor(i => i.FirstName)
				.NotEmpty()
					.WithMessage("First name is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureCharacterString()
					.WithMessage("First name must only contain characters")
					.WithValidationContext(ValidationStatus.Invalid, ValidationSeverity.Info);
			#endregion

			#region Last Name
			RuleFor(i => i.LastName)
				.NotEmpty()
					.WithMessage("Last name is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureCharacterString()
					.WithMessage("Last name must only contain characters")
					.WithValidationContext(ValidationStatus.Invalid, ValidationSeverity.Info);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ConsumerPrincipal instance)
		{
			var baseValidationResult = base.Validate(instance);

			return baseValidationResult;
		}
	}
}




