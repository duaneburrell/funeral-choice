using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class AdminMemberValidator : ModelAbstractValidator<AdminMember>
	{
		public AdminMemberValidator(AccountSession accountSession, ValidationMode validationMode, AdminMember currentMember)
			: base(accountSession, validationMode)
		{
			#region Id
			RuleFor(i => i.Id)
				.EnsureAccountTypeAdministrator(accountSession)
				.When(i => validationMode == ValidationMode.Update || validationMode == Model.ValidationMode.Delete)
					.WithMessage("You must be an administrator to modify this account")
					.WithValidationContext(ValidationStatus.Invalid);

			RuleFor(i => i.Id)
				.CanDeleteAdministratorMember(accountSession)
				.When(i => validationMode == Model.ValidationMode.Delete)
					.WithMessage("This administrator can not be deleted.")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region Email
			RuleFor(i => i.Email)
				.NotEmpty()
					.WithMessage("Please specifiy an email address")
					.WithValidationContext(ValidationStatus.Required)
				.EmailAddress()
					.WithMessage("The email address format is invalid")
					.WithValidationContext(ValidationStatus.Invalid);

			RuleFor(i => i.Email)
				.EnsureUnusedEmail(this.AccountSession)
				.When(i => validationMode == ValidationMode.Create)
					.WithMessage("The email address is currently in use")
					.WithValidationContext(ValidationStatus.Invalid);

			RuleFor(i => i.Email)
				.EnsureMemberEmail(currentMember.Id)
				.When(i => validationMode == ValidationMode.Update)
					.WithMessage("The email address is currently in use")
					.WithValidationContext(ValidationStatus.Invalid);
			#endregion

			#region First Name
			RuleFor(i => i.FirstName)
				.NotEmpty()
					.WithMessage("Your first name is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureCharacterString()
					.WithMessage("Your first name should only contain characters")
					.WithValidationContext(ValidationStatus.Invalid, ValidationSeverity.Info);
			#endregion

			#region Last Name
			RuleFor(i => i.LastName)
				.NotEmpty()
					.WithMessage("Your last name is required")
					.WithValidationContext(ValidationStatus.Required)
				.EnsureCharacterString()
					.WithMessage("Your last name should only contain characters")
					.WithValidationContext(ValidationStatus.Invalid, ValidationSeverity.Info);
			#endregion
		}
	}
}
