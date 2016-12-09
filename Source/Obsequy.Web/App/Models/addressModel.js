

angular.module('address-model', [])
 .factory('Address', [function () {
 	var Address = function (properties) {

 		angular.extend(this, {

			line1: undefined,
			line2: undefined,
			city: undefined,
			state: undefined,
			zip: undefined,

			hasLocation: undefined,
			latitude: undefined,
			longitude: undefined,

 			update: function (properties) {
 				properties = properties || {};

				// update properties
				this.line1 = properties.line1;
				this.line2 = properties.line2;
				this.city = properties.city;
				this.state = properties.state;
				this.zip = properties.zip;
				this.hasLocation = properties.hasLocation;
				this.latitude = properties.latitude;
				this.longitude = properties.longitude;

				this.location = (this.line1 + ", " + this.city);

 				// UI filtering
				this.filter = (this.line1 + ", " + this.city + ", " + this.state + " " + this.zip);
			},

			toJSON: function () {
				return {
					line1: this.line1,
					line2: this.line2,
					city: this.city,
					state: this.state,
					zip: this.zip
				};
			}
		});

		this.update(properties);
	};

 	return Address;
}]);