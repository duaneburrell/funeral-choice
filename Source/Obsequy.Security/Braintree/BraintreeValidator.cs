using System;
using System.Collections.Generic;
using System.Linq;
using Braintree;
using FluentValidation;
using FluentValidation.Results;
using Obsequy.Model;
using Obsequy.Utility;

namespace Obsequy.Security
{
	public static class BraintreeValidator
	{
		#region Properties
		private static Dictionary<string, string> Attributes
		{
			get
			{
				var attributes = new Dictionary<string, string>();

				attributes.Add("???.1", "CardholderName");
				attributes.Add("number", "CardNumber");
				attributes.Add("expiration_month", "ExpirationMonth");
				attributes.Add("expiration_year", "ExpirationYear");
				attributes.Add("cvv", "SecurityCode");
				attributes.Add("???.2", "PostalCode");

				return attributes;
			}
		}
		#endregion

		#region As Invalid Payment
		public static FluentValidation.Results.ValidationResult AsInvalidPayment(Braintree.Result<Transaction> transactionResult)
		{
			// generate an 'invalid ' fluent error
			var baseValidationResult = new ValidationResult();
			var modelValidationResult = new Obsequy.Model.ResponseFormValidator.ResponseFormValidationResult(baseValidationResult);

			// get braintree validation errors
			var validationErrors = transactionResult.Errors.DeepAll();

			if (validationErrors.Any())
			{
				modelValidationResult.Payment = new ModelValidationResult();

				foreach (var attribute in Attributes)
				{
					// no real reason why I'm using last instead of first, just initial testing showed these messages make more sense to the user?
					var validationError = validationErrors.LastOrDefault(item => item.Attribute == attribute.Key);
					if (validationError != null)
					{
						// only set the general message if the property errors are not set
						modelValidationResult.Payment.Errors.Add(new PropertyValidationFailure(attribute.Value, validationError.Message)
						{
							Status = FormatHelper.ToCamlCase("Invalid"),
							Severity = FormatHelper.ToCamlCase("Error")
						});
					}
				}
			}
			
			if (!string.IsNullOrEmpty(transactionResult.Message))
			{
				// only set the general message if the property errors are not set
				modelValidationResult.Errors.Add(new PropertyValidationFailure("Id", transactionResult.Message)
				{
					Status = FormatHelper.ToCamlCase("Invalid"),
					Severity = FormatHelper.ToCamlCase("Error")
				});
			}

			return modelValidationResult;
		}
		#endregion
	}
}
