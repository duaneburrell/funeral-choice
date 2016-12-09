

angular.module('providerPortfolio-scheme', ['underscore'])
 .factory('ProviderPortfolio', ['_', 'moment', 'ProviderMember', 'ProviderPrincipal', 'ProviderProfile', function (_, moment, ProviderMember, ProviderPrincipal, ProviderProfile) {
 	var ProviderPortfolio = function (properties) {

		angular.extend(this, {

			id: undefined,
			accountStatus: undefined,
			principal: new ProviderPrincipal(),
			profile: new ProviderProfile(),
			members: [],

			created: {},
			modified: {},

			update: function (properties) {
				properties = properties || { address: {} };

				// update properties
				this.id = properties.id;
				this.accountStatus = properties.accountStatus;

				_.each(properties.members, function (member) {
					this.members.push(new ProviderMember(member));
				}, this);

				this.principal.update(properties.principal);
				this.profile.update(properties.profile);

				this.created = properties.created;
				this.modified = properties.modified;

				// UI filtering
				this._filterCreated = moment(properties.created).format('L');
				this._filterModified = moment(properties.modified).format('L');
			},

			toJSON: function () {
				return {
					id: this.id,
					accountStatus: this.accountStatus,
					principal: this.principal.toJSON(),
					profile: this.profile.toJSON(),
					members: this.members,
					created: this.created,
					modified: this.modified
				};
			}
		});

		this.update(properties);
	};

 	return ProviderPortfolio;
}]);