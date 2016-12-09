using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ProviderPortfolioValidator : ModelAbstractValidator<ProviderPortfolio>
	{
		public class ProviderPortfolioValidationResult : ModelValidationResult
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
					if (!this.Profile.IsValid)
						return false;

					return true;
				}
			}

			// Summary:
			//     The Principal of the model.
			public ModelValidationResult Principal { get; set; }

			// Summary:
			//     The Preference of the model.
			public ModelValidationResult Profile { get; set; }

			// Summary:
			//     Creates a new portfolio validation result.
			public ProviderPortfolioValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ProviderPortfolioValidator(AccountSession accountSession, ValidationMode validationMode, ProviderPortfolio instance)
			: base(accountSession, validationMode)
		{
			#region Id
			RuleFor(i => i.Id)
				.IsCurrentProviderPortfolio(instance)
				.When(i => validationMode == ValidationMode.Update)
					.WithMessage("Your data is not current")
					.WithValidationContext(ValidationStatus.Stale);

			RuleFor(i => i.Id)
				.OwnsProviderPortfolioId(accountSession)
				.When(i => validationMode == ValidationMode.Update)
					.WithMessage("You do not have access to this portfolio")
					.WithValidationContext(ValidationStatus.Unauthorized);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ProviderPortfolio instance)
		{
			var baseValidationResult = base.Validate(instance);
			var modelValidationResult = new ProviderPortfolioValidationResult(baseValidationResult)
			{
				Principal = ((new ProviderPrincipalValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Principal) as ModelValidationResult),
				Profile = ((new ProviderProfileValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Profile) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}
