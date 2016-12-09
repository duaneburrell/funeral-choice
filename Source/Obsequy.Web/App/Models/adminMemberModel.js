

angular.module('adminMember-model', ['underscore', 'address-model'])
 .factory('AdminMember', ['_', 'Address', function (_, Address) {
 	var AdminMember = function (properties) {

		angular.extend(this, {

			id: undefined,
			accountPrestige: undefined,
			accountStatus: undefined,
			accountType: undefined,

			email: undefined,
			firstName: undefined,
			lastName: undefined,
			fullName: undefined,

			isNotifiedOnConsumerRegistrations: undefined,
			isNotifiedOnProviderRegistrations: undefined,
			isNotifiedOnAcceptedResponses: undefined,
			isNotifiedOnExceptions: undefined,

			created: {},
			modified: {},

			update: function (properties) {
				properties = properties || { address: {} };

				// update properties
				this.id = properties.id;
				this.accountPrestige = properties.accountPrestige;
				this.accountStatus = properties.accountStatus;
				this.accountType = properties.accountType;

				this.email = properties.email;
				this.firstName = properties.firstName;
				this.lastName = properties.lastName;
				this.fullName = properties.fullName;

				this.isNotifiedOnConsumerRegistrations = properties.isNotifiedOnConsumerRegistrations;
				this.isNotifiedOnProviderRegistrations = properties.isNotifiedOnProviderRegistrations;
				this.isNotifiedOnAcceptedResponses = properties.isNotifiedOnAcceptedResponses;
				this.isNotifiedOnExceptions = properties.isNotifiedOnExceptions;

				// update receipts
				this.created = properties.created;
				this.modified = properties.modified;
			},

			toJSON: function () {
				return {
					id: this.id,
					email: this.email,
					firstName: this.firstName,
					lastName: this.lastName,
					isNotifiedOnConsumerRegistrations: this.isNotifiedOnConsumerRegistrations,
					isNotifiedOnProviderRegistrations: this.isNotifiedOnProviderRegistrations,
					isNotifiedOnAcceptedResponses: this.isNotifiedOnAcceptedResponses,
					isNotifiedOnExceptions: this.isNotifiedOnExceptions
				};
			}
		});

		this.update(properties);
	};

 	return AdminMember;
}]);

