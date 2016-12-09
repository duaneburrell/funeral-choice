using FluentValidation;
using FluentValidation.Results;

namespace Obsequy.Model
{
	public class ProviderRegistrationFormValidator : ModelAbstractValidator<ProviderRegistrationForm>
	{
		public class ProviderRegistrationFormValidationResult : ModelValidationResult
        {
            // Summary:
            //     Is Valid is dependent on all child models
            public override bool IsValid
            {
                get 
                { 
                    if (!base.IsValid)
                        return false;
					if (!this.Member.IsValid)
                        return false;
                        
                    return true;
                }
            }

			// Summary:
			//     The Member of the model.
			public ModelValidationResult Member { get; set; }

            // Summary:
            //     Creates a new model validation result.
            public ProviderRegistrationFormValidationResult(ValidationResult validationResult) 
                : base(validationResult)
            {
            }
        }

		public ProviderRegistrationFormValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Password
			RuleFor(i => i.Password)
				.NotEmpty()
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("Please specify a password")
					.WithValidationContext(ValidationStatus.Invalid);

			RuleFor(i => i.Password)
				.EnsurePasswordStrength()
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("Passwords must have at least 6 characters")
					.WithValidationContext(ValidationStatus.Invalid);
            #endregion

            #region Confirm Password
			RuleFor(i => i.ConfirmPassword)
				.NotEmpty()
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("A password confirmation is required")
                    .WithValidationContext(ValidationStatus.Required);

			RuleFor(i => i.ConfirmPassword)
				.Equal(i => i.Password)
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("The passwords do not match")
                    .WithValidationContext(ValidationStatus.Invalid);
            #endregion

			#region Has Accepted EULA
			RuleFor(i => i.HasAcceptedEULA)
				.EnsureHasAcceptedEULA()
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("The end user license agreement must be accepted")
					.WithValidationContext(ValidationStatus.Required);
			#endregion
        }

		public override FluentValidation.Results.ValidationResult Validate(ProviderRegistrationForm instance)
		{
			var baseValidationResult = base.Validate(instance);

			var modelValidationResult = new ProviderRegistrationFormValidationResult(baseValidationResult)
			{
				Member = ((new ProviderMemberValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Member) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}
