using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class AdministratorRegistrationForm
	{
		#region Properties

		public string Id { get; set; }

		public AdminMember Member { get; set; }

		public string Password { get; set; }
		public string ConfirmPassword { get; set; }

		#endregion

		public AdministratorRegistrationForm()
		{
			Member = new AdminMember();
		}

		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new AdministratorRegistrationFormValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
	}
}
