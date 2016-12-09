

angular.module('providerMember-model', ['underscore', 'address-model'])
 .factory('ProviderMember', ['_', 'Address', function (_, Address) {
 	var ProviderMember = function (properties) {

		angular.extend(this, {

			id: undefined,
			accountPrestige: undefined,
			accountStatus: undefined,
			accountType: undefined,

			email: undefined,
			firstName: undefined,
			lastName: undefined,
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

			    // ralph, we should discuss this.  what seems to be happening is that the ProviderMember and the ProviderPortfolio have somewhat independent 
			    // account status's.  The ProviderMember was defaulted to active, which was the initial source of trouble. defaulting it to pending keeps the user
			    // out by default, but the mechanism for enabling access in admin is ProviderPortfolio, which has it's own account access and setting that does not
			    // effect the ProviderMember, which will always have it's default state.  We should talk about what the intention and how to get the cleanest solution
                // because although this works it's hacky and probably not what you envisioned.

                // so based on our discussions this is how i'm leaving it, seem to work fine.
				if (_.size(properties.portfolios)) { 
				    this.accountStatus = _.first(properties.portfolios).accountStatus;
				}
				else {
				    this.accountStatus = 1; // $enum here? yes! tried, gives a not defined error...tbd later
				}

				this.accountType = properties.accountType;

				this.email = properties.email;
				this.firstName = properties.firstName;
				this.lastName = properties.lastName;
				this.fullName = properties.fullName;
				this.phone = properties.phone;

				this.portfolioId = properties.portfolioId;

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
					phone: this.phone
				};
			}
		});

		this.update(properties);
	};

 	return ProviderMember;
}]);

