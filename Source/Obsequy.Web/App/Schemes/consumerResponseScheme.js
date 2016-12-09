
angular.module('consumerResponse-scheme', ['underscore'])
	.factory('ConsumerResponse', ['_', 'ProviderPrincipal', 'ProviderProfile', 'ConsumerPreference', 'ConsumerSchedule', 'ResponseAgreement', 'ResponseAlternate', 'ResponseReceipt', function (_, ProviderPrincipal, ProviderProfile, ConsumerPreference, ConsumerSchedule, ResponseAgreement, ResponseAlternate, ResponseReceipt) {
 		var ConsumerResponse = function (properties) {

			angular.extend(this, {

				id: undefined,
				consumerPortfolioId: undefined,
				providerPortfolioId: undefined,

				distance: undefined,
				quote: undefined,
				depositDue: undefined,
				depositPaid: undefined,
				balanceDue: undefined,
				balancePaid: undefined,

				principal: new ProviderPrincipal(),
				profile: new ProviderProfile(),
			
				agreement: new ResponseAgreement(),
				alternate: new ResponseAlternate(),

				preference: new ConsumerPreference(),
				schedule: new ConsumerSchedule(),

				created: new ResponseReceipt(),
				current: new ResponseReceipt(),
				pending: new ResponseReceipt(),

				lastActivity: undefined,

				canAccept: undefined,
				canReject: undefined,

				update: function (properties) {
					properties = properties || { principal: {}, profile: {}, agreement: {}, alternate: {}, current: {}, pending: {} };

					// update properties
					this.id = properties.id;
					this.consumerPortfolioId = properties.consumerPortfolioId;
					this.providerPortfolioId = properties.providerPortfolioId;

					this.distance = properties.distance;
					this.quote = properties.quote;
					this.depositDue = properties.depositDue;
					this.depositPaid = properties.depositPaid;
					this.balanceDue = properties.balanceDue;
					this.balancePaid = properties.balancePaid;

					this.principal.update(properties.principal);
					this.profile.update(properties.profile);
			
					this.agreement.update(properties.agreement);
					this.alternate.update(properties.alternate);

					this.preference.update(properties.preference);
					this.schedule.update(properties.schedule);

					this.created.update(properties.created);
					this.current.update(properties.current);
					this.pending.update(properties.pending);

					this.lastActivity = properties.lastActivity;

					this.canAccept = properties.canAccept;
					this.canReject = properties.canReject;
				},

				toJSON: function () {
					return {
						id: this.id,
						current: this.current.toJSON()
					};
				}
			});

			this.update(properties);
		};

 		return ConsumerResponse;
	}]);
