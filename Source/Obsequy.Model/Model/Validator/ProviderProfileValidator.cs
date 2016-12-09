using FluentValidation;
using Obsequy.Model;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ProviderProfileValidator : ModelAbstractValidator<ProviderProfile>
	{
		public ProviderProfileValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Description
			RuleFor(i => i.Description)
				.NotEmpty()
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Description must be specified")
					.WithValidationContext(ValidationStatus.Invalid)
				.Length(0, 3000)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Description must be 3000 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Website
			RuleFor(i => i.Website)
				.EnsureUrl()
					.WithMessage("Website URL invalid")
					.WithValidationContext(ValidationStatus.Invalid)
				.Length(0, 100)
					.WithMessage("Website URL must be 100 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Business Established
			RuleFor(i => i.BusinessEstablished)
				.NotEqual(BusinessEstablishedTypes.NA)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Business established length must be specified")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region FacilityAge
			RuleFor(i => i.FacilityAge)
				.NotEqual(FacilityAgeTypes.NA)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Facility age must be specified")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Facility Style
			RuleFor(i => i.FacilityStyle)
				.NotEqual(FacilityStyleTypes.NA)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Facility style must be specified")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Funeral Director Experience
			RuleFor(i => i.FuneralDirectorExperience)
				.NotEqual(FuneralDirectorExperienceTypes.NA)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Funeral directors' experience must be specified")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Transportation Fleet Age
			RuleFor(i => i.TransportationFleetAge)
				.NotEqual(TransportationFleetAgeTypes.NA)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Transportation fleet age must be specified")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion
		}
	}
}
