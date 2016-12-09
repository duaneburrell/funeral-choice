

angular.module('appHome', ANGULAR_REQUIREMENTS)
	.config(['$routeProvider',
		function ($routeProvider) {
			$routeProvider.
				when('/', {
					templateUrl: '/App/Views/All/welcome.html',
					controller: 'HomeWelcomeCtrl'
				}).
				when('/register', {
					templateUrl: '/App/Views/Home/register.html'
				}).
				when('/register/consumer', {
					templateUrl: '/App/Views/Home/register-consumer.html',
					controller: 'RegisterConsumerCtrl'
				}).
				when('/register/provider', {
					templateUrl: '/App/Views/Home/register-provider.html',
					controller: 'RegisterProviderCtrl'
				}).
				when('/how/consumer', {
					templateUrl: '/App/Views/All/How/how-consumer.html'
				}).
				when('/how/provider', {
					templateUrl: '/App/Views/All/How/how-provider.html'
				}).
				when('/login', {
					templateUrl: '/App/Views/Home/login.html',
					controller: 'LoginCtrl'
				}).
				when('/recovery', {
					templateUrl: '/App/Views/Home/password-recovery.html',
					controller: 'PasswordRecoveryCtrl'
				}).
				when('/reset', {
					templateUrl: '/App/Views/Home/password-reset.html',
					controller: 'PasswordResetCtrl'
				}).
				when('/documents', {
					templateUrl: '/App/Views/All/Support/documents.html'
				}).
				when('/resources', {
					templateUrl: '/App/Views/All/Support/resources.html'
				}).
				when('/support', {
					templateUrl: '/App/Views/All/Support/support.html'
				}).
				when('/eula', {
					templateUrl: '/App/Views/Home/EULA.html'
				}).
				otherwise({
					redirectTo: '/'
				});
		}
	])
	.controller('HomeWelcomeCtrl', ['$scope', '$location', '_', '$api',
		function HomeWelcomeCtrl($scope, $location, _, $api) {

			$scope.isWelcomePage = true;
			$scope.pageTitle = "Welcome to Funeral-Choice";

			// check pages as the route and show/hide landing graphics
			$scope.$on('$routeChangeSuccess', function () {
				var items = $scope.items;
				var path = $location.path();

				$scope.isWelcomePage = path === '/';
			});
		}

	])
	.controller('LoginCtrl', ['$scope', '$location', '_', '$api', 'LoginForm', 'Profiler',
		function LoginCtrl($scope, $location, _, $api, LoginForm, Profiler) {

			// form data
			$scope.loginForm = new LoginForm();

			// the profiler (for development/demo only)
			$scope.profiler = new Profiler($scope, 'login', $scope.loginForm);

			$scope.onSubmit = function () {
				var loginForm = $scope.loginForm.toJSON();

				$api.account.login(loginForm)
					.then(function (redirect) {
						window.location = redirect;
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('PasswordRecoveryCtrl', ['$scope', '$location', '_', '$api', 'PasswordRecoveryForm',
		function PasswordRecoveryCtrl($scope, $location, _, $api, PasswordRecoveryForm) {

			// form data
			$scope.recoveryForm = new PasswordRecoveryForm({ isResettingPassword: true });
			$scope.recoveryInProgress = false;

			$scope.onSubmit = function () {
				var recoveryForm = $scope.recoveryForm.toJSON();

				$api.account.passwordRecovery(recoveryForm)
					.then(function (results) {
						if (results.canResetPassword) {

							$api.account.recoverPassword(recoveryForm)
								.then(function (results) {
									$scope.recoveryInProgress = true;
								})
								.catch(function (results) {
									$scope.status = results || {};
								});
						}
						else {
							// error mesage is already shown
						}
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('PasswordResetCtrl', ['$scope', '$location', '_', '$api', '$routeParams',
		function PasswordResetCtrl($scope, $location, _, $api, $routeParams) {

			var token = $routeParams.param;

			$scope.error = false;
			$scope.errorMsg = "";
			$scope.success = false;
			$scope.failure = false;
			$scope.password;
			$scope.confirmPassword;

			$scope.onSubmit = function () {

				if ($scope.password != $scope.confirmPassword) {
					$scope.error = true;
					$scope.errorMsg = "Passwords do not match";
				}
				else if (_.size($scope.password) < 6) {
					$scope.error = true;
					$scope.errorMsg = "Password must be at least 6 characters in length";
				}
				else {
					$scope.error = false;

					var j = { "token": token, "password": $scope.password, "confirmPassword": $scope.confirmPassword };
					JSON.stringify(j);

					$api.account.incomingToken(j)
						.then(function (results) {
							if (results.success) {
								$scope.success = true;
							}
							else {
								$scope.failure = true;
							}
						})
						.catch(function (results) {
							$scope.status = results || {};
						});
				}
			};
		}
	])
	.controller('RegisterConsumerCtrl', ['$scope', '$location', '_', '$api', 'ConsumerRegistrationForm', 'Profiler',
		function RegisterConsumerCtrl($scope, $location, _, $api, ConsumerRegistrationForm, Profiler) {

			// form data
			$scope.registrationForm = new ConsumerRegistrationForm();

			// the profiler (for development/demo only)
			$scope.profiler = new Profiler($scope, 'consumer.registration', $scope.registrationForm);

			$scope.onSubmit = function () {
				var registrationForm = $scope.registrationForm.toJSON();

				$api.account.registerConsumer(registrationForm)
					.then(function (results) {
						window.location = results.redirect;
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('RegisterProviderCtrl', ['$scope', '$location', '_', '$api', 'ProviderRegistrationForm', 'Profiler',
		function RegisterProviderCtrl($scope, $location, _, $api, ProviderRegistrationForm, Profiler) {

			// form data
			$scope.registrationForm = new ProviderRegistrationForm();

			// the profiler (for development/demo only)
			$scope.profiler = new Profiler($scope, 'provider.registration', $scope.registrationForm);

			$scope.onSubmit = function () {
				var registrationForm = $scope.registrationForm.toJSON();

				$api.account.registerProvider(registrationForm)
					.then(function (results) {
						window.location = results.redirect;
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	]);

