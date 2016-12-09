using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class PasswordRecoveryForm
	{
		#region Properties

		public string Email { get; set; }

		public string SecurityQuestion { get; set; }
		public string SecurityAnswer { get; set; }

		public bool CanAnswerSecurityQuestion { get; set; }
		public bool CanResetPassword { get; set; }
		public bool CanSendSmSCode { get; set; }

		public bool IsAnsweringSecurityQuestion { get; set; } // they've elected to answer the security question
		public bool IsResettingPassword { get; set; }  // they just want a reset link to follow
		public bool IsSendingSmSCode { get; set; }    // they want us to send them an SMS code

		#endregion

		public PasswordRecoveryForm()
		{
		}

		public ValidationResult Validate()
		{
			return ((new PasswordRecoveryFormValidator()).Validate(this) as ValidationResult);
		}
	}
}
