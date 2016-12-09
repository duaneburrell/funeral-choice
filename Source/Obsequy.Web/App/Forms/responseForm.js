

angular.module('response-form', [])
	.factory('ResponseForm', ['Payment', 'ResponseReceipt', function (Payment, ResponseReceipt) {
		var ResponseForm = function (properties) {

 			angular.extend(this, {

 				id: undefined,
 				payment: new Payment(),
 				current: new ResponseReceipt(),

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.id = properties.id
 					this.payment.update(properties.payment);
 					this.current.update(properties.current);
				},

				toJSON: function () {
					return {
						id: this.id,
						payment: this.payment.toJSON(),
						current: this.current.toJSON()
					};
				}
			});

			this.update(properties);
		};

		return ResponseForm;
	}]);