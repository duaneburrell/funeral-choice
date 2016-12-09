

angular.module('payment-model', [])
 .factory('Payment', [function () {
 	var Payment = function (properties) {

 		angular.extend(this, {

 			amount: undefined,
 			cardholderName: undefined,
 			cardNumber: undefined,
 			expirationMonth: undefined,
 			expirationYear: undefined,
 			securityCode: undefined,
 			postalCode: undefined,
 			countryCode: undefined,
 			transactionId: undefined,

 			update: function (properties) {
 				properties = properties || {};

				// update properties
 				this.amount = properties.amount;
 				this.cardholderName = properties.cardholderName;
 				this.cardNumber = properties.cardNumber;
 				this.expirationMonth = properties.expirationMonth;
 				this.expirationYear = properties.expirationYear;
 				this.securityCode = properties.securityCode;
				this.postalCode = properties.postalCode;
				this.countryCode = properties.countryCode;
				this.transactionId = properties.transactionId;
			},

			toJSON: function () {
				return {
					amount: this.amount,
					cardholderName: this.cardholderName,
					cardNumber: this.cardNumber,
					expirationMonth: this.expirationMonth,
					expirationYear: this.expirationYear,
					securityCode: this.securityCode,
					postalCode: this.postalCode,
					countryCode: this.countryCode,
					transactionId: this.transactionId
				};
			}
		});

		this.update(properties);
	};

 	return Payment;
}]);