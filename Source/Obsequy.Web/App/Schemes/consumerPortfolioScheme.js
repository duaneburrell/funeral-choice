
angular.module('consumerPortfolio-scheme', ['underscore', 'consumerMember-model'])
	.factory('ConsumerPortfolio', ['_', 'ConsumerMember', 'ConsumerPrincipal', 'ConsumerPreference', 'ConsumerRequest', 'ConsumerSchedule', function (_, ConsumerMember, ConsumerPrincipal, ConsumerPreference, ConsumerRequest, ConsumerSchedule) {
 		var ConsumerPortfolio = function (properties) {

			angular.extend(this, {

				id: undefined,
				creator: new ConsumerMember(),
				members: [],

				principal: new ConsumerPrincipal(),
				preference: new ConsumerPreference(),
				request: new ConsumerRequest(),
				schedule: new ConsumerSchedule(),

				canDelete: undefined,
				canSave: undefined,
				canSubmit: undefined,
			
				chosenOptionsCount: undefined,
				totalOptionsCount: undefined,
				percentComplete: undefined,

				// these are set by the repository
				responseIdsAccepted: [],
				responseIdsPending: [],
				responseIdsRejected: [],
				totalResponsesGenerated: undefined,
				totalResponsesAcknowledged: undefined,

				created: {},
				modified: {},
				reminded: {},

				update: function (properties) {
					properties = properties || { address: {} };

					// update properties
					this.id = properties.id;
					this.creator.update(properties.creator);

					_.each(properties.members, function (member) {
						this.members.push(new ConsumerMember(member));
					}, this);

					this.principal.update(properties.principal);
					this.preference.update(properties.preference);
					this.request.update(properties.request);
					this.schedule.update(properties.schedule);

					this.canDelete = properties.canDelete;
					this.canSave = properties.canSave;
					this.canSubmit = properties.canSubmit;

					this.chosenOptionsCount = properties.chosenOptionsCount;
					this.totalOptionsCount = properties.totalOptionsCount;
					this.percentComplete = properties.percentComplete;

					this.created = properties.created;
					this.modified = properties.modified;
					this.reminded = properties.reminded;
				},

				toJSON: function () {
					return {
						id: this.id,
						principal: this.principal.toJSON(),
						preference: this.preference.toJSON(),
						schedule: this.schedule.toJSON(),
                        request: this.request.toJSON(),
						canDelete: this.canDelete,
						canSave: this.canSave,
						canSubmit: this.canSubmit,
						chosenOptionsCount: this.chosenOptionsCount,
						totalOptionsCount: this.totalOptionsCount,
						percentComplete: this.percentComplete,
						responseIdsAccepted: this.responseIdsAccepted,
						responseIdsPending: this.responseIdsPending,
						responseIdsRejected: this.responseIdsRejected,
						totalResponsesGenerated: this.totalResponsesGenerated,
						totalResponsesAcknowledged: this.totalResponsesAcknowledged,
						created: this.created,
						modified: this.modified,
						reminded: this.reminded
					};
				}
			});

			this.update(properties);
		};

 		return ConsumerPortfolio;
	}]);
