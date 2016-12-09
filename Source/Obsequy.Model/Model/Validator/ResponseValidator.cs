using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ResponseValidator : ModelAbstractValidator<Response>
	{
		public ResponseValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
		}
	}
}
