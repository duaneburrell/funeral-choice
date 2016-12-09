using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ProviderRegistrationForm
	{
		#region Properties

		public ProviderMember Member { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public bool? HasAcceptedEULA { get; set; }

		#endregion

		public ProviderRegistrationForm()
		{
			this.Member = new ProviderMember();
		}

		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ProviderRegistrationFormValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
	}
}
