using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ConsumerPortfolioValidator : ModelAbstractValidator<ConsumerPortfolio>
	{
		public class ConsumerPortfolioValidationResult : ModelValidationResult
        {
            // Summary:
            //     Is Valid is dependent on all child models
            public override bool IsValid
            {
                get 
                { 
                    if (!base.IsValid)
                        return false;
					if (!this.Principal.IsValid)
						return false;
					if (!this.Preference.IsValid)
						return false;
					if (!this.Schedule.IsValid)
						return false;
                        
                    return true;
                }
            }

            // Summary:
			//     The Principal of the model.
            public ModelValidationResult Principal { get; set; }

			// Summary:
			//     The Preference of the model.
			public ModelValidationResult Preference { get; set; }

			// Summary:
			//     The Schedule of the model.
			public ModelValidationResult Schedule { get; set; }

            // Summary:
            //     Creates a new portfolio validation result.
			public ConsumerPortfolioValidationResult(ValidationResult validationResult) 
                : base(validationResult)
            {
            }
        }

		public ConsumerPortfolioValidator(AccountSession accountSession, ValidationMode validationMode, ConsumerPortfolio instance)
			: base(accountSession, validationMode)
		{
			#region Id
			RuleFor(i => i.Id)
				.IsCurrentConsumerPortfolio(instance)
				.When(i => validationMode == ValidationMode.Update)
					.WithMessage("Your data is not current")
					.WithValidationContext(ValidationStatus.Stale);
			
			RuleFor(i => i.Id)
				.OwnsConsumerPortfolioId(accountSession)
				.When(i => validationMode == ValidationMode.Update)
					.WithMessage("You do not have access to this portfolio")
					.WithValidationContext(ValidationStatus.Unauthorized);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ConsumerPortfolio instance)
		{
			var baseValidationResult = base.Validate(instance);
			var modelValidationResult = new ConsumerPortfolioValidationResult(baseValidationResult)
			{
				Principal = ((new ConsumerPrincipalValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Principal) as ModelValidationResult),
				Preference = ((new ConsumerPreferenceValidator(this.AccountSession, this.ValidationMode, instance.Preference)).Validate(instance.Preference) as ModelValidationResult),
				Schedule = ((new ConsumerScheduleValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Schedule) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}
