using FluentValidation;
using FluentValidation.Results;

namespace Obsequy.Model
{
	public class AdministratorRegistrationFormValidator : ModelAbstractValidator<AdministratorRegistrationForm>
	{
		public class AdministratorRegistrationFormValidationResult : ModelValidationResult
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
			public AdministratorRegistrationFormValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public AdministratorRegistrationFormValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Id
			RuleFor(i => i.Id)
				.EnsureAccountTypeAdministrator(accountSession)
					.WithMessage("You must be an administrator to create an account")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

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
        }

		public override FluentValidation.Results.ValidationResult Validate(AdministratorRegistrationForm instance)
		{
			var baseValidationResult = base.Validate(instance);

			var modelValidationResult = new AdministratorRegistrationFormValidationResult(baseValidationResult)
			{
				Member = ((new AdminMemberValidator(this.AccountSession, this.ValidationMode, instance.Member)).Validate(instance.Member) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}