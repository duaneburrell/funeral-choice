using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ConsumerMemberValidator : ModelAbstractValidator<ConsumerMember>
	{
		public class ConsumerMemberValidationResult : ModelValidationResult
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
			public ConsumerMemberValidationResult(ValidationResult validationResult) 
                : base(validationResult)
            {
            }
        }

		public ConsumerMemberValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Email
			RuleFor(i => i.Email)
				.NotEmpty()
					.WithMessage("Please specifiy an email address")
					.WithValidationContext(ValidationStatus.Required)
				.EmailAddress()
					.WithMessage("The email address format is invalid")
					.WithValidationContext(ValidationStatus.Invalid);

			RuleFor(i => i.Email)
				.EnsureUnusedEmail(this.AccountSession)
					.WithMessage("The email address is currently in use")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region First Name
			RuleFor(i => i.FirstName)
				.NotEmpty()
					.WithMessage("Your first name is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureCharacterString()
					.WithMessage("Your first name should only contain characters")
					.WithValidationContext(ValidationStatus.Invalid, ValidationSeverity.Info);
			#endregion

			#region Last Name
			RuleFor(i => i.LastName)
				.NotEmpty()
					.WithMessage("Your last name is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureCharacterString()
					.WithMessage("Your last name should only contain characters")
					.WithValidationContext(ValidationStatus.Invalid, ValidationSeverity.Info);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ConsumerMember instance)
		{
			var baseValidationResult = base.Validate(instance);

			return baseValidationResult;
		}
	}
}
