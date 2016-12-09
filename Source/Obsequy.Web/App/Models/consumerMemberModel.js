

angular.module('consumerMember-model', ['underscore', 'address-model'])
 .factory('ConsumerMember', ['_', 'Address', function (_, Address) {
	var ConsumerMember = function (properties) {

		angular.extend(this, {

			id: undefined,
			accountPrestige: undefined,
			accountStatus: undefined,
			accountType: undefined,

			email: undefined,
			firstName: undefined,
			lastName: undefined,
			fullName: undefined,
			address: new Address(),
			phone: undefined,

			portfolioId: undefined,
			
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
				this.phone = properties.phone;

				this.portfolioId = properties.portfolioId;

				// update address
				this.address.update(properties.address);

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
					phone: this.phone,
					address: this.address.toJSON()
				};
			}
		});

		this.update(properties);
	};

	return ConsumerMember;
}]);

