

angular.module('passwordRecovery-form', [])
	.factory('PasswordRecoveryForm', [function () {
		var PasswordRecoveryForm = function (properties) {

 			angular.extend(this, {

 				email: undefined,

 				securityQuestion: undefined,
 				securityAnswer: undefined,

 				canAnswerSecurityQuestion: undefined,
 				canResetPassword: undefined,
 				canSendSmSCode: undefined,

 				isAnsweringSecurityQuestion: undefined,
 				isResettingPassword: undefined,
 				isSendingSmSCode: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.email = properties.email;
 				
 					this.securityQuestion = properties.securityQuestion;
 					this.securityAnswer = properties.securityAnswer;

 					this.canAnswerSecurityQuestion = properties.canAnswerSecurityQuestion;
 					this.canResetPassword = properties.canResetPassword;
 					this.canSendSmSCode = properties.canSendSmSCode;

 					this.isAnsweringSecurityQuestion = properties.isAnsweringSecurityQuestion;
 					this.isResettingPassword = properties.isResettingPassword;
 					this.isSendingSmSCode = properties.isSendingSmSCode;
				},

				toJSON: function () {
					return {
						email: this.email,
						securityQuestion: this.securityQuestion,
						securityAnswer: this.securityAnswer,
						canAnswerSecurityQuestion: this.canAnswerSecurityQuestion,
						canResetPassword: this.canResetPassword,
						canSendSmSCode: this.canSendSmSCode,
						isAnsweringSecurityQuestion: this.isAnsweringSecurityQuestion,
						isResettingPassword: this.isResettingPassword,
						isSendingSmSCode: this.isSendingSmSCode
					};
				}
			});

			this.update(properties);
		};

		return PasswordRecoveryForm;
	}]);