using FluentValidation;
using Obsequy.Model;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ConsumerScheduleValidator : ModelAbstractValidator<ConsumerSchedule>
	{
		public ConsumerScheduleValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
			#region Remains Choice Type
			RuleFor(i => i.ScheduleChoiceType)
				.NotEqual(ScheduleChoiceTypes.None)
				.When(i => (validationMode == ValidationMode.Update))
					.WithMessage("Please specify one option")
					.WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Wake Date
            RuleFor(i => i.WakeDate)
                .EnsureDateIsAfterToday()
                .When(i => (validationMode == ValidationMode.Update && i.ScheduleChoiceType == ScheduleChoiceTypes.Scheduled && i.WakeDate != null))
                    .WithMessage("Wake date must be after {0:MM/dd/yyyy}", DateTimeHelper.Today)
                    .WithValidationContext(ValidationStatus.Required);
			#endregion

			#region Ceremony Date
			RuleFor(i => i.CeremonyDate)
                .NotNull()
				.When(i => (validationMode == ValidationMode.Update && i.ScheduleChoiceType == ScheduleChoiceTypes.Scheduled))
					.WithMessage("Funeral date must be specified")
					.WithValidationContext(ValidationStatus.Required);

            RuleFor(i => i.CeremonyDate)
                .EnsureDateIsAfterToday()
				.When(i => (validationMode == ValidationMode.Update && i.ScheduleChoiceType == ScheduleChoiceTypes.Scheduled))
                    .WithMessage("Funeral date must be after {0:MM/dd/yyyy}", DateTimeHelper.Today)
					.WithValidationContext(ValidationStatus.Required);
			#endregion
		}
	}
}
