using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ResponseAlternateValidator : ModelAbstractValidator<ResponseAlternate>
	{
		public ResponseAlternateValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
		}
	}
}
