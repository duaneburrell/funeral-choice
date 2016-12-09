

angular.module('consumerPrincipal-model', ['underscore', 'address-model'])
 .factory('ConsumerPrincipal', ['_', 'Address', function (_, Address) {
 	var ConsumerPrincipal = function (properties) {

		angular.extend(this, {

			firstName: undefined,
			lastName: undefined,
			phone: undefined,
			address: new Address(),
			
			update: function (properties) {
				properties = properties || { address: {} };

				// update properties
				this.firstName = properties.firstName;
				this.lastName = properties.lastName;
				this.fullName = properties.fullName;
				this.phone = properties.phone;

				// update address
				this.address.update(properties.address);

				// update receipts
				this.modified = properties.modified;
			},

			toJSON: function () {
				return {
					firstName: this.firstName,
					lastName: this.lastName,
					fullName: this.fullName,
					phone: this.phone,
					address: this.address.toJSON()
				};
			}
		});

		this.update(properties);
	};

 	return ConsumerPrincipal;
}]);

