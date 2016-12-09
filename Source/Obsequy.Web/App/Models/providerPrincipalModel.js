

angular.module('providerPrincipal-model', [])
 .factory('ProviderPrincipal', ['Address', function (Address) {
 	var ProviderPrincipal = function (properties) {

 		angular.extend(this, {

 			name: undefined,
 			phone: undefined,
 			email: undefined,
 			address: new Address(),
 			googleMapUrl: undefined,

 			update: function (properties) {
 				properties = properties || { address: {} };

				// update properties
 				this.name = properties.name;
 				this.phone = properties.phone;
 				this.email = properties.email;
 				this.address.update(properties.address);
 				this.googleMapUrl = properties.googleMapUrl;
 			},

			toJSON: function () {
				return {
					name: this.name,
					phone: this.phone,
					email: this.email,
					address: this.address.toJSON(),
					googleMapUrl: this.googleMapUrl
				};
			}
		});

		this.update(properties);
	};

 	return ProviderPrincipal;
}]);