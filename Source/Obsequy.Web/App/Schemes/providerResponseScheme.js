

angular.module('providerResponse-scheme', ['underscore'])
 .factory('ProviderResponse', ['_', 'ResponseAgreement', 'ResponseAlternate', 'ResponseReceipt', 'ConsumerPrincipal', 'ConsumerMember', 'ConsumerPreference', 'ConsumerSchedule', 'ConsumerRequest', function (_, ResponseAgreement, ResponseAlternate, ResponseReceipt, ConsumerPrincipal, ConsumerMember, ConsumerPreference, ConsumerSchedule, ConsumerRequest) {
 	var ProviderResponse = function (properties) {

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

			agreement: new ResponseAgreement(),
			alternate: new ResponseAlternate(),

			created: new ResponseReceipt(),
			current: new ResponseReceipt(),
			pending: new ResponseReceipt(),

			member: new ConsumerMember(),
			principal: new ConsumerPrincipal(),
			preference: new ConsumerPreference(),
			schedule: new ConsumerSchedule(),
			request: new ConsumerRequest(),

			lastActivity: undefined,

			canBecomeAvailable: undefined,
			canBecomePending: undefined,
			canBecomeDismissed: undefined,

			update: function (properties) {
				properties = properties || { };

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

				this.agreement.update(properties.agreement);
				this.alternate.update(properties.alternate);

				this.created.update(properties.created);
				this.current.update(properties.current);
				this.pending.update(properties.pending);

				this.member.update(properties.member);
				this.principal.update(properties.principal);
				this.preference.update(properties.preference);
				this.schedule.update(properties.schedule);
				this.request.update(properties.request);

				this.lastActivity = properties.lastActivity;

				this.canBecomeAvailable = properties.canBecomeAvailable;
				this.canBecomePending = properties.canBecomePending;
				this.canBecomeDismissed = properties.canBecomeDismissed;
			},

			toJSON: function () {
				return {
					id: this.id,
					consumerPortfolioId: this.consumerPortfolioId,
					providerPortfolioId: this.providerPortfolioId,
					distance: this.distance,
					quote: this.quote,
					depositDue: this.depositDue,
					depositPaid: this.depositPaid,
					balanceDue: this.balanceDue,
					balancePaid: this.balancePaid,
					agreement: this.agreement.toJSON(),
					alternate: this.alternate.toJSON(),
					current: this.current.toJSON()
				};
			}
		});

		this.update(properties);
	};

 	return ProviderResponse;
}]);