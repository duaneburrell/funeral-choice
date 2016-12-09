using FluentValidation;
using Obsequy.Model;

namespace Obsequy.Model
{
	public class ConsumerRequestValidator : ModelAbstractValidator<ConsumerRequest>
	{
		public ConsumerRequestValidator(AccountSession accountSession, ValidationMode validationMode)
			: base(accountSession, validationMode)
		{
		}
	}
}
