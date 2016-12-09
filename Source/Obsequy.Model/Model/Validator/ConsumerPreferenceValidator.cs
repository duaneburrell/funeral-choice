using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ConsumerPreferenceValidator : ModelAbstractValidator<ConsumerPreference>
	{
		public class ConsumerPreferenceValidationResult : ModelValidationResult
		{
			// Summary:
			//     Is Valid is dependent on all child models
			public override bool IsValid
			{
				get
				{
					if (!base.IsValid)
						return false;
					if (!this.Proximity.IsValid)
						return false;

					return true;
				}
			}

			// Summary:
			//     The Address of the model.
			public ModelValidationResult Proximity { get; set; }

			// Summary:
			//     Creates a new model validation result.
			public ConsumerPreferenceValidationResult(ValidationResult validationResult)
				: base(validationResult)
			{
			}
		}

		public ConsumerPreferenceValidator(AccountSession accountSession, ValidationMode validationMode, ConsumerPreference instance)
			: base(accountSession, validationMode)
		{
			#region Max Distance
			RuleFor(i => i.MaxDistance)
				.GreaterThan(0.0)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Distance is required")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Internment Type
			RuleFor(i => i.InternmentType)
				.HasPreference(instance, "InternmentType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Internment must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Funeral Type
			RuleFor(i => i.FuneralType)
				.HasPreference(instance, "FuneralType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Funeral Style must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Wake Type
			RuleFor(i => i.WakeType)
				.HasPreference(instance, "WakeType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Wake Style must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Religion Type
			RuleFor(i => i.ReligionType)
				.HasPreference(instance, "ReligionType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Religion Type must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Expected Attendance Type
			RuleFor(i => i.ExpectedAttendanceType)
				.HasPreference(instance, "ExpectedAttendanceType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Expected attendance must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Remains Choice Type
			RuleFor(i => i.RemainsChoiceType)
				.NotEqual(RemainsChoiceTypes.None)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Please specify one option")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Material Type
			RuleFor(i => i.CasketMaterialType)
				.HasPreference(instance, "CasketMaterialType")
				.When(i => (validationMode == ValidationMode.Update && i.RemainsChoiceType == RemainsChoiceTypes.CasketRequired))
					.WithMessage("Casket Material must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Size Type
			RuleFor(i => i.CasketSizeType)
				.HasPreference(instance, "CasketSizeType")
				.When(i => (validationMode == ValidationMode.Update && i.RemainsChoiceType == RemainsChoiceTypes.CasketRequired))
					.WithMessage("Casket Size must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Color Type
			RuleFor(i => i.CasketColorType)
				.HasPreference(instance, "CasketColorType")
				.When(i => (validationMode == ValidationMode.Update && i.RemainsChoiceType == RemainsChoiceTypes.CasketRequired))
					.WithMessage("Casket Color must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Casket Manufacturer Type
			RuleFor(i => i.CasketManufacturerType)
				.HasPreference(instance, "CasketManufacturerType")
				.When(i => (validationMode == ValidationMode.Update && i.RemainsChoiceType == RemainsChoiceTypes.CasketRequired))
					.WithMessage("Casket Manufacturer must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Burial Container Type
			RuleFor(i => i.BurialContainerType)
				.HasPreference(instance, "BurialContainerType")
				.When(i => (validationMode == ValidationMode.Update && i.RemainsChoiceType == RemainsChoiceTypes.CasketRequired))
					.WithMessage("Burial Container must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Transportation Choice Type
			RuleFor(i => i.TransportationChoiceType)
				.NotEqual(TransportationChoiceTypes.None)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Please specify one option")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Transportation Of Family Type
			RuleFor(i => i.TransportationOfFamilyType)
				.HasPreference(instance, "TransportationOfFamilyType")
				.When(i => (validationMode == ValidationMode.Update && i.TransportationChoiceType == TransportationChoiceTypes.Required))
					.WithMessage("Number of family members must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Flower Spray Type
			RuleFor(i => i.FlowerSprayType)
				.HasPreference(instance, "FlowerSprayType")
                .When(i => (validationMode == ValidationMode.Update && i.RemainsChoiceType == RemainsChoiceTypes.CasketRequired))
					.WithMessage("Flower Spray must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Primary Flower Type
			RuleFor(i => i.PrimaryFlowerType)
				.HasPreference(instance, "PrimaryFlowerType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Primary Flower must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Secondary Flower Type
			RuleFor(i => i.SecondaryFlowerType)
				.HasPreference(instance, "SecondaryFlowerType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Secondary Flower must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Accent Flower Type
			RuleFor(i => i.AccentFlowerType)
				.HasPreference(instance, "AccentFlowerType")
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Accent Flower must be specified")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Service Preferences
			RuleFor(i => i.ServicePreferences)
				.Length(0, 1000)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Service Preferences must be 1000 characters or less")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion
		}

		public override FluentValidation.Results.ValidationResult Validate(ConsumerPreference instance)
		{
			var baseValidationResult = base.Validate(instance);

			var modelValidationResult = new ConsumerPreferenceValidationResult(baseValidationResult)
			{
				Proximity = ((new ProximityValidator(this.AccountSession, this.ValidationMode)).Validate(instance.Proximity) as ModelValidationResult)
			};

			return modelValidationResult;
		}
	}
}

