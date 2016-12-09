using FluentValidation;
using FluentValidation.Results;

namespace Obsequy.Model
{
    public class PropertyDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
		public ValidationMode ValidationMode { get; set; }

        public PropertyDto()
        {
        }

		public ValidationResult ValidateEmail(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new PropertyValidator("Email", accountSession, validationMode)).Validate(this) as ValidationResult);
		}

		public ValidationResult ValidatePassword(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new PropertyValidator("Password", accountSession, validationMode)).Validate(this) as ValidationResult);
		}
    }
}
