using System.Collections.Generic;
using Braintree;
using FluentValidation.Results;
using Obsequy.Utility;

namespace Obsequy.Model
{
	public class ResponseForm
	{
		#region Properties

		public ResponseReceipt Current { get; set; }

		public string Id { get; set; }

		public Payment Payment { get; set; }

		#endregion

		public ResponseForm()
		{
			this.Payment = new Payment();
		}

		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode, ResponseForm instance)
		{
			return ((new ResponseFormValidator(accountSession, validationMode, instance)).Validate(this) as ValidationResult);
		}
	}
}
