using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ConsumerResponseSchemeValidator : ModelAbstractValidator<ConsumerResponseScheme>
	{
		public ConsumerResponseSchemeValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
		}
	}
}
