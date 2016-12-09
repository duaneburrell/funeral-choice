using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ProviderResponseSchemeValidator : ModelAbstractValidator<ProviderResponseScheme>
	{
		public class ProviderResponseSchemeValidationResult : ModelValidationResult
		{
			// Summary:
			//     Is Valid is dependent on all child models
			public override bool IsValid
			{
				get
				{
					if (!base.IsValid)
						return false;
					if (!this.Agreement.IsValid)
						return false;

					return true;
				}
			}

			// Summary:
			//     The Agreement of the model.
			public ModelValidationResult Agreement { get; set; }

			// Summary:
			//     Creates a new model validation result.
			public ProviderResponseSchemeValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ProviderResponseSchemeValidator(AccountSession accountSession, ValidationMode validationMode, ProviderResponseScheme instance)
			: base(accountSession, validationMode)
		{
			#region Id
			RuleFor(i => i.Id)
				.IsCurrentResponse(instance)
					.WithMessage("Your data is not current")
					.WithValidationContext(ValidationStatus.Stale);
			
			RuleFor(i => i.Id)
				.OwnsResponseId(accountSession, validationMode)
					.WithMessage("You do not have ownership of this response")
					.WithValidationContext(ValidationStatus.Unauthorized);
			
			RuleFor(i => i.Id)
				.CanBecomeAvailable()
				.When(i => validationMode == ValidationMode.Available)
					.WithMessage("The state can not be changed")
					.WithValidationContext(ValidationStatus.Stale);

			RuleFor(i => i.Id)
				.CanBecomeDismissed()
				.When(i => validationMode == ValidationMode.Dismiss)
					.WithMessage("The state can not be changed")
					.WithValidationContext(ValidationStatus.Stale);

			RuleFor(i => i.Id)
				.CanBecomePending()
				.When(i => validationMode == ValidationMode.Pending)
					.WithMessage("The state can not be changed")
					.WithValidationContext(ValidationStatus.Stale);
			#endregion

			#region Quote
			RuleFor(i => i.Quote)
				.EnsureCurrency()
                .EnsureQuote()
				.When(i => (validationMode == ValidationMode.Pending))
                    .WithMessage(string.Format("{0} ${1}", "Please specify a valid amount, minimum bid is", FluentValidationExtensions.MIN_QUOTE_VALUE))
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ProviderResponseScheme instance)
		{
			var baseValidationResult = base.Validate(instance);
			var modelValidationResult = new ProviderResponseSchemeValidationResult(baseValidationResult)
			{
				Agreement = ((new ResponseAgreementValidator(this.AccountSession, this.ValidationMode, instance)).Validate(instance.Agreement) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}
