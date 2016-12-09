using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ProviderPrincipalValidator : ModelAbstractValidator<ProviderPrincipal>
	{
		public class ProviderPrincipalValidationResult : ModelValidationResult
        {
            // Summary:
            //     Is Valid is dependent on all child models
            public override bool IsValid
            {
                get 
                { 
                    if (!base.IsValid)
                        return false;
					if (!this.Address.IsValid)
						return false;
                        
                    return true;
                }
            }

            // Summary:
            //     The Address of the model.
            public ModelValidationResult Address { get; set; }

            // Summary:
            //     Creates a new model validation result.
            public ProviderPrincipalValidationResult(ValidationResult validationResult) 
                : base(validationResult)
            {
            }
        }

		public ProviderPrincipalValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Name
			RuleFor(i => i.Name)
				.NotEmpty()
				.When(i => validationMode == ValidationMode.Create || validationMode == ValidationMode.Update)
					.WithMessage("Name is required")
					.WithValidationContext(ValidationStatus.Required)
				.Length(1, 64)
				.When(i => validationMode == ValidationMode.Create || validationMode == ValidationMode.Update)
					.WithMessage("Name must be 64 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Phone
			RuleFor(i => i.Phone)
				.NotEmpty()
				.When(i => validationMode == ValidationMode.Create || validationMode == ValidationMode.Update)
					.WithMessage("Phone is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsurePhoneNumber()
				.When(i => validationMode == ValidationMode.Create || validationMode == ValidationMode.Update)
					.WithMessage("Phone is invalid")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Email
			RuleFor(i => i.Email)
				.NotEmpty()
				.When(i => validationMode == ValidationMode.Create || validationMode == ValidationMode.Update)
					.WithMessage("Email is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureEmailFormat()
				.When(i => validationMode == ValidationMode.Create || validationMode == ValidationMode.Update)
					.WithMessage("Email is invalid")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ProviderPrincipal instance)
		{
			var baseValidationResult = base.Validate(instance);

			var modelValidationResult = new ProviderPrincipalValidationResult(baseValidationResult)
			{
				Address = ((new AddressValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Address) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}
