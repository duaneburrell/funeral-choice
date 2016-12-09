using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class LoginForm
	{
		#region Properties

		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }

		#endregion

		public LoginForm()
		{
		}

		public ValidationResult Validate()
		{
			return ((new LoginFormValidator()).Validate(this) as ValidationResult);
		}

		public ValidationResult AsInvalidPassword()
		{
			return (new LoginFormValidator()).AsInvalidPassword(this);
		}
	}
}
