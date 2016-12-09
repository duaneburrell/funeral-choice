using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ResponseAgreementValidator : ModelAbstractValidator<ResponseAgreement>
	{
		public class ResponseAgreementValidationResult : ModelValidationResult
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
			public ResponseAgreementValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ResponseAgreementValidator(AccountSession accountSession, ValidationMode validationMode, ProviderResponseScheme response)
			: base(accountSession, validationMode)
		{
			#region Wake Date
			RuleFor(i => i.WakeDate)
				.HasAgreementDate(response, "WakeDate")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Wake Date must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Ceremony Date
			RuleFor(i => i.CeremonyDate)
				.HasAgreementDate(response, "CeremonyDate")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Funeral Date must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Internment Type
			RuleFor(i => i.InternmentType)
				.HasAgreement(response, "InternmentType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Internment must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Funeral Type
			RuleFor(i => i.FuneralType)
				.HasAgreement(response, "FuneralType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Funeral Style must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Wake Type
			RuleFor(i => i.WakeType)
				.HasAgreement(response, "WakeType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Wake Style must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Religion Type
			RuleFor(i => i.ReligionType)
				.HasAgreement(response, "ReligionType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Religion must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Expected Attendance Type
			RuleFor(i => i.ExpectedAttendanceType)
				.HasAgreement(response, "ExpectedAttendanceType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Expected attendance must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Service Preferences
			RuleFor(i => i.ServicePreferences)
				.HasAgreementText(response, "ServicePreferences")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Service Preferences must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Material Type
			RuleFor(i => i.CasketMaterialType)
				.HasAgreement(response, "CasketMaterialType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Casket Material must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Size Type
			RuleFor(i => i.CasketSizeType)
				.HasAgreement(response, "CasketSizeType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Casket Size must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Color Type
			RuleFor(i => i.CasketColorType)
				.HasAgreement(response, "CasketColorType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Casket Color must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Manufacturer Type
			RuleFor(i => i.CasketManufacturerType)
				.HasAgreement(response, "CasketManufacturerType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Casket Manufacturer must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Burial Container Type
			RuleFor(i => i.BurialContainerType)
				.HasAgreement(response, "BurialContainerType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Burial Container must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Transportation Of Family Type
			RuleFor(i => i.TransportationOfFamilyType)
				.HasAgreement(response, "TransportationOfFamilyType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Transportation of family must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Flower Spray Type
			RuleFor(i => i.FlowerSprayType)
				.HasAgreement(response, "FlowerSprayType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Flower Spray must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Primary Flower Type
			RuleFor(i => i.PrimaryFlowerType)
				.HasAgreement(response, "PrimaryFlowerType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Primary Flower must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Secondary Flower Type
			RuleFor(i => i.SecondaryFlowerType)
				.HasAgreement(response, "SecondaryFlowerType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Secondary Flower must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Accent Flower Type
			RuleFor(i => i.AccentFlowerType)
				.HasAgreement(response, "AccentFlowerType")
				.When(i => (validationMode == ValidationMode.Pending))
					.WithMessage("Accent Flower must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion
		}
	}
}
