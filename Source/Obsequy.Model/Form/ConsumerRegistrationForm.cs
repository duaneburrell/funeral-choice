using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ConsumerRegistrationForm
	{
		#region Properties

		public ConsumerMember Member { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public bool? HasAcceptedEULA { get; set; }

		#endregion

		public ConsumerRegistrationForm()
		{
		}

		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerRegistrationFormValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
	}
}
