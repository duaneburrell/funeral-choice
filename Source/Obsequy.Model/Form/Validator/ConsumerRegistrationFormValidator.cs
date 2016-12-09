using FluentValidation;
using FluentValidation.Results;

namespace Obsequy.Model
{
	public class ConsumerRegistrationFormValidator : ModelAbstractValidator<ConsumerRegistrationForm>
	{
		public class ConsumerRegistrationFormValidationResult : ModelValidationResult
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
			public ConsumerRegistrationFormValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ConsumerRegistrationFormValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Password
			RuleFor(i => i.Password)
				.NotEmpty()
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("Please specifiy a password")
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

		public override FluentValidation.Results.ValidationResult Validate(ConsumerRegistrationForm instance)
		{
			var baseValidationResult = base.Validate(instance);

			var modelValidationResult = new ConsumerRegistrationFormValidationResult(baseValidationResult)
			{
				Member = ((new ConsumerMemberValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Member) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}