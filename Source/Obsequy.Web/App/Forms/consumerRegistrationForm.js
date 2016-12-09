

angular.module('consumerRegistration-form', [])
	.factory('ConsumerRegistrationForm', ['ConsumerMember', function (ConsumerMember) {
		var ConsumerRegistrationForm = function (properties) {

 			angular.extend(this, {

 				member: new ConsumerMember(),
 				password: undefined,
 				confirmPassword: undefined,
 				hasAcceptedEULA: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.member.update(properties.member);
 					this.password = properties.password;
 					this.confirmPassword = properties.confirmPassword;
 					this.hasAcceptedEULA = properties.hasAcceptedEULA;
				},

				toJSON: function () {
					return {
						member: this.member.toJSON(),
						password: this.password,
						confirmPassword: this.confirmPassword,
						hasAcceptedEULA: this.hasAcceptedEULA
					};
				}
			});

			this.update(properties);
		};

		return ConsumerRegistrationForm;
	}]);