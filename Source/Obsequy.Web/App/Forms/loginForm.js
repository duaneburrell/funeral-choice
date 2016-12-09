

angular.module('login-form', [])
	.factory('LoginForm', [function () {
 		var LoginForm = function (properties) {

 			angular.extend(this, {

 				email: undefined,
 				password: undefined,
 				rememberMe: undefined,

 				errors: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.email = properties.email;
 					this.password = properties.password;
 					this.rememberMe = properties.rememberMe;

 					this.errors = undefined;
 				},

 				invalid: function (results) {
 					this.errors = (results.errors || {} );
 				},

				toJSON: function () {
					return {
						email: this.email,
						password: this.password,
						rememberMe: this.rememberMe
					};
				}
			});

			this.update(properties);
		};

 		return LoginForm;
	}]);