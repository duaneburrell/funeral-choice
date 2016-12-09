using System.Collections.Generic;
using System.Linq;
using Braintree;
using FluentValidation;
using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ResponseFormValidator : ModelAbstractValidator<ResponseForm>
	{
		public class ResponseFormValidationResult : ModelValidationResult
		{
			// Summary:
			//     Is Valid is dependent on all child models
			public override bool IsValid
			{
				get
				{
					if (!base.IsValid)
						return false;
					if (!this.Payment.IsValid)
						return false;

					return true;
				}
			}

			// Summary:
			//     The Payment of the model.
			public ModelValidationResult Payment { get; set; }

			// Summary:
			//     Creates a new model validation result.
			public ResponseFormValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ResponseFormValidator(AccountSession accountSession, ValidationMode validationMode, ResponseForm instance)
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
				.CanBecomeAccepted()
				.When(i => validationMode == ValidationMode.Accept)
					.WithMessage("The state can not be changed")
					.WithValidationContext(ValidationStatus.Stale);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ResponseForm instance)
		{
			var baseValidationResult = base.Validate(instance);
			var modelValidationResult = new ResponseFormValidationResult(baseValidationResult)
			{
				Payment = ((new PaymentValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Payment) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}