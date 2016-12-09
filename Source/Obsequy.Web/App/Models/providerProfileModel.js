

angular.module('providerProfile-model', [])
	.factory('ProviderProfile', [function () {
 		var ProviderProfile = function (properties) {

 			angular.extend(this, {

 				description: undefined,
 				website: undefined,
 				businessEstablished: undefined,
 				facilityAge: undefined,
 				facilityStyle: undefined,
 				funeralDirectorExperience: undefined,
 				transportationFleetAge: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.description = properties.description;
 					this.website = properties.website;
 					this.businessEstablished = properties.businessEstablished;
 					this.facilityAge = properties.facilityAge;
 					this.facilityStyle = properties.facilityStyle;
 					this.funeralDirectorExperience = properties.funeralDirectorExperience;
 					this.transportationFleetAge = properties.transportationFleetAge;
				},

				toJSON: function () {
					return {
						description: this.description,
						website: this.website,
						businessEstablished: this.businessEstablished,
						facilityAge: this.facilityAge,
						facilityStyle: this.facilityStyle,
						funeralDirectorExperience: this.funeralDirectorExperience,
						transportationFleetAge: this.transportationFleetAge
					};
				}
			});

			this.update(properties);
		};

 		return ProviderProfile;
	}]);