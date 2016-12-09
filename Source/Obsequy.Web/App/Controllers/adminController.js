

angular.module('appAdmin', ANGULAR_REQUIREMENTS)
	.config(['$routeProvider',
		function ($routeProvider) {
			$routeProvider.
				when('/', {
					templateUrl: '/App/Views/Admin/home.html',
					controller: 'AdminHomeCtrl'
				}).
				when('/provider/:id', {
					templateUrl: '/App/Views/Admin/provider.html',
					controller: 'AdminProviderCtrl',
					resolve: {
						data: ['$route', '$api', function ($route, $api) {
							var id = $route.current.params.id;
							var promise = $api.admin.getProviderPortfolio(id)
								.then(function (results) {
									return results;
								});
							return promise;
						}]
					}
				}).
				when('/providers', {
					templateUrl: '/App/Views/Admin/providers.html',
					controller: 'AdminProvidersCtrl'
				}).
				when('/consumer/:id?', {
					templateUrl: '/App/Views/Admin/consumer.html',
					controller: 'AdminConsumerCtrl',
					resolve: {
						data: ['$route', '$api', function ($route, $api) {
							var id = $route.current.params.id;
							var promise = $api.admin.getConsumerPortfolio(id)
								.then(function (results) {
									return results;
								});
							return promise;
						}]
					}
				}).
				when('/consumers', {
					templateUrl: '/App/Views/Admin/consumers.html',
					controller: 'AdminConsumersCtrl'
				}).
				when('/administrators', {
					templateUrl: '/App/Views/Admin/administrators.html',
					controller: 'AdministratorsCtrl'
				}).
				when('/administrator/:id?', {
					templateUrl: '/App/Views/Admin/administrator-edit.html',
					controller: 'AdministratorEditCtrl'
				}).
				when('/log', {
					templateUrl: '/App/Views/Admin/log.html',
					controller: 'AdministratorLogCtrl'
				}).
				when('/test', {
					templateUrl: '/App/Views/Admin/test.html',
					controller: 'AdministratorTestCtrl'
				}).
				otherwise({
					redirectTo: '/'
				});
		}
	])
	.controller('AdminHomeCtrl', ['$scope', '$location', '_', '$api', '$repo', '$enum',
		function AdminHomeCtrl($scope, $location, _, $api, $repo, $enum) {

		}
	])
	.controller('AdminProviderCtrl', ['$scope', '_', '$api', '$enum', 'data',
		function AdminProviderCtrl($scope, _, $api, $enum, data) {

			$scope.selectedTab = 'available';
			$scope.enum = $enum;
			$scope.member = data.portfolio.members[0];
			$scope.portfolio = data.portfolio;
			$scope.responses = data.responses;
			$scope.visibleResponseId = '';

			var setPortfolioData = function () {

				$scope.portfolio.responsesAccepted = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Accepted.value; });
				$scope.portfolio.responsesAvailable = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Available.value; });
				$scope.portfolio.responsesDismissed = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Dismissed.value; });
				$scope.portfolio.responsesPending = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Pending.value; });
				$scope.portfolio.responsesRejected = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Rejected.value; });

				$scope.portfolio.totalResponsesGenerated = _.size($scope.responses);
				$scope.portfolio.totalResponsesAcknowledged = $scope.portfolio.responsesAccepted.length + $scope.portfolio.responsesPending.length + $scope.portfolio.responsesRejected.length;
				$scope.portfolio.totalResponsesAvailable = $scope.portfolio.responsesAvailable.length + $scope.portfolio.responsesDismissed.length;
			};

			$scope.toggleResponse = function (response) {
				if (!$scope.visibleResponseId || $scope.visibleResponseId !== response.id)
					$scope.visibleResponseId = response.id;
				else
					$scope.visibleResponseId = '';
			};

			setPortfolioData();
		}
	])
	.controller('AdminProvidersCtrl', ['$scope', '_', '$api', '$enum', 'ProviderPortfolio',
		function AdminProvidersCtrl($scope, _, $api, $enum, ProviderPortfolio) {

			$scope.selectedTab = 'pending';
			$scope.activeProviders = [];
			$scope.pendingProviders = [];
			$scope.rejectedProviders = [];
			$scope.suspendedProviders = [];

			getActiveProviders = function () {

				$api.admin.getActiveProviders()
					.then(function (results) {
						$scope.activeProviders = [];

						_.each(results, function (provider) {
							$scope.activeProviders.push(new ProviderPortfolio(provider));
						});
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			getPendingProviders = function () {

				$api.admin.getPendingProviders()
					.then(function (results) {
						$scope.pendingProviders = [];

						_.each(results, function (provider) {
							$scope.pendingProviders.push(new ProviderPortfolio(provider));
						});
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			getRejectedProviders = function () {

				$api.admin.getRejectedProviders()
					.then(function (results) {
						$scope.rejectedProviders = [];

						_.each(results, function (provider) {
							$scope.rejectedProviders.push(new ProviderPortfolio(provider));
						});
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			getSuspendedProviders = function () {

				$api.admin.getSuspendedProviders()
					.then(function (results) {
						$scope.suspendedProviders = [];

						_.each(results, function (provider) {
							$scope.suspendedProviders.push(new ProviderPortfolio(provider));
						});
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onActive = function (provider) {
				// set account status to active
				provider.accountStatus = $enum.accountStatus.Active.value;

				$api.admin.updateProviderAccountStatus(provider)
					.then(function (results) {
						refresh();
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onPending = function (provider) {
				// set account status to pending
				provider.accountStatus = $enum.accountStatus.Pending.value;

				$api.admin.updateProviderAccountStatus(provider)
					.then(function (results) {
						refresh();
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onReject = function (provider) {
				// set account status to rejected
				provider.accountStatus = $enum.accountStatus.Rejected.value;

				$api.admin.updateProviderAccountStatus(provider)
					.then(function (results) {
						refresh();
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onSuspend = function (provider) {
				// set account status to suspended
				provider.accountStatus = $enum.accountStatus.Suspended.value;

				$api.admin.updateProviderAccountStatus(provider)
					.then(function (results) {
						refresh();
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			var refresh = function () {
				getPendingProviders();
				getActiveProviders();
				getSuspendedProviders();
				getRejectedProviders();
			};
			
			refresh();
		}
	])
	.controller('AdminConsumerCtrl', ['$scope', '$location', '_', '$api', '$enum', 'data',
		function AdminConsumerCtrl($scope, $location, _, $api, $enum, data) {

			$scope.selectedTab = 'request';
			$scope.enum = $enum;
			$scope.member = data.portfolio.members[0];
			$scope.portfolio = data.portfolio;
			$scope.responses = data.responses;
			$scope.visibleResponseId = '';

			var setPortfolioData = function () {

				$scope.portfolio.responsesAccepted = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Accepted.value; });
				$scope.portfolio.responsesAvailable = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Available.value; });
				$scope.portfolio.responsesDismissed = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Dismissed.value; });
				$scope.portfolio.responsesPending = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Pending.value; });
				$scope.portfolio.responsesRejected = _.filter($scope.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Rejected.value; });

				$scope.portfolio.totalResponsesGenerated = _.size($scope.responses);
				$scope.portfolio.totalResponsesAcknowledged = $scope.portfolio.responsesAccepted.length + $scope.portfolio.responsesPending.length + $scope.portfolio.responsesRejected.length;
				$scope.portfolio.totalResponsesAvailable = $scope.portfolio.responsesAvailable.length + $scope.portfolio.responsesDismissed.length;
			};

			$scope.onDelete = function () {
			    var deleteOk = window.confirm("Are you sure you want to delete the portfolio for " + $scope.portfolio.principal.fullName + "?");

			    if (deleteOk) {
			        $api.admin.deleteConsumerPortfolio($scope.portfolio.id)
					    .then(function (results) {
					        // return back to consumers
					        $location.path('/consumers');
					    })
					    .catch(function (results) {
					        $scope.status = results || {};
					    });
			    }
			};

			$scope.onSendReminder = function () {
			    $api.admin.remindConsumerPortfolio($scope.portfolio.id)
					.then(function (results) {
					    // update the portfolio
					    $scope.portfolio = results;

					    setPortfolioData();
					})
					.catch(function (results) {
					    $scope.status = results || {};
					});
			};

			$scope.toggleResponse = function (response) {
				if (!$scope.visibleResponseId || $scope.visibleResponseId !== response.id)
					$scope.visibleResponseId = response.id;
				else
					$scope.visibleResponseId = '';
			};

			setPortfolioData();
		}
	])
	.controller('AdminConsumersCtrl', ['$scope', '_', '$api', '$enum', 'ConsumerPortfolio',
		function AdminConsumersCtrl($scope, _, $api, $enum, ConsumerPortfolio) {

			$scope.selectedTab = 'pending';
			$scope.enum = $enum;
			$scope.draftPortfolios = [];
			$scope.pendingPortfolios = [];
			$scope.completedPortfolios = [];

			var getConsumerPortfolios = function () {

				$api.admin.getConsumerPortfolios()
					.then(function (results) {
						_.each(results, function (portfolio) {
							if (portfolio.request.state === $enum.requestReceiptStates.Draft.value)
								$scope.draftPortfolios.push(new ConsumerPortfolio(portfolio));

							if (portfolio.request.state === $enum.requestReceiptStates.Pending.value)
								$scope.pendingPortfolios.push(new ConsumerPortfolio(portfolio));

							if (portfolio.request.state === $enum.requestReceiptStates.Completed.value)
								$scope.completedPortfolios.push(new ConsumerPortfolio(portfolio));
						});
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			getConsumerPortfolios();
		}
	])
	.controller('AdministratorsCtrl', ['$scope', '_', '$api', '$enum',
		function AdministratorsCtrl($scope, _, $api, $enum) {

			$scope.enum = $enum;
			$scope.administratorMembers = [];

			$scope.onDelete = function (member) {
				var deleteOk = window.confirm("Are you sure you want to delete " + member.fullName + "?");

				if (deleteOk) {
					$api.admin.deleteAdministratorMember(member)
						.then(function (results) {
						})
						.catch(function (results) {
							$scope.status = results || {};
						});
				}
			};

			$scope.getAdministratorMembers = function () {

				$api.admin.getAdministratorMembers()
					.then(function (results) {
						$scope.administratorMembers = [];

						_.each(results, function (member) {
							$scope.administratorMembers.push(member);
						});
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			// load data
			$scope.getAdministratorMembers();
		}
	])
	.controller('AdministratorEditCtrl', ['$scope', '$location', '$routeParams', '_', '$api', 'AdminMember', 'AdministratorRegistrationForm',
		function AdministratorEditCtrl($scope, $location, $routeParams, _, $api, AdminMember, AdministratorRegistrationForm) {

			$scope.isNew = true;
			$scope.registrationForm = new AdministratorRegistrationForm();

			$scope.getAdministratorMember = function () {

				$api.admin.getAdministratorMember($routeParams.id)
					.then(function (administratorMember) {
						if (administratorMember) {
							$scope.registrationForm.member.update(administratorMember);
							$scope.isNew = false;
						}
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onSubmit = function () {

				if ($scope.isNew) {
					$api.admin.createAdministratorMember($scope.registrationForm)
					.then(function (administratorMember) {
						$location.path('/administrators');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
				}
				else {
					$api.admin.updateAdministratorMember($scope.registrationForm.member)
					.then(function (administratorMember) {
						$location.path('/administrators');
					})
					.catch(function (results) {
						$scope.status = { member: results || {} };
					});
				}
			};

			$scope.onDelete = function () {
				var member = $scope.registrationForm.member;
				var deleteOk = window.confirm("Are you sure you want to delete " + member.fullName + "?");

				if (deleteOk) {
					$api.admin.deleteAdministratorMember(member)
						.then(function (results) {
							$location.path('/administrators');
						})
						.catch(function (results) {
							$scope.status = results || {};
						});
				}
			};

			// load data
			$scope.getAdministratorMember();
		}
	])
	.controller('AdministratorLogCtrl', ['$scope', '_',
		function AdministratorLogCtrl($scope, _) {

			$scope.lines = [];

			$.get('/Server.Log.txt', function (file) {
				var lines = file.split('\r');

				$.each(lines, function (i, line) {
				});

				// gotta have this here
				$scope.$apply();
			});
		}
	])
	.controller('AdministratorTestCtrl', ['$scope', 'moment', '_', 'braintree', '$api', '$enum', '$busy', 'Payment',
		function AdministratorTestCtrl($scope, moment, _, Braintree, $api, $enum, $busy, Payment) {

			// set the response
			$scope.enum = $enum;
			$scope.busy = $busy;
			$scope.payment = new Payment();

			$scope.errorMessage = '';
			$scope.statusMessage = '';

			var braintree = undefined;

			$scope.init = function () {
				$scope.busy.show();

				// initialize payment processing
				$api.response.getPaymentConfiguration()
					.then(function (marketplaceKey) {
						// create the braintree instance
						braintree = Braintree.create(marketplaceKey);
						$scope.busy.hide();
					})
					.catch(function (results) {
						$scope.status = results || {};
						$scope.busy.hide();
					});
			};

			$scope.onSendPayment = function () {
				// initialize status
				$scope.errors = {};
				$scope.errorMessage = '';
				$scope.statusMessage = '';

				// encrypt the payment information
				var payment = new Payment(
				{
					amount: $scope.payment.amount,
					cardholderName: encrypt($scope.payment.cardholderName),
					cardNumber: encrypt($scope.payment.cardNumber),
					expirationMonth: encrypt($scope.payment.expirationMonth),
					expirationYear: encrypt($scope.payment.expirationYear),
					securityCode: encrypt($scope.payment.securityCode),
					postalCode: encrypt($scope.payment.postalCode)
				});

				// send the encrypted card to the server for validation, payment processing, and update as accepted
				$api.admin.testPayment(payment)
					.then(function (results) {

						// set last payment message
						$scope.statusMessage = 'Last payment of ' + results.amount + ' accepted ' + moment().fromNow() + ' @ ' + moment().format('h:mm:ssa') + '. ID: ' + results.transactionId;

						// clear error message
						$scope.errorMessage = '';
						$scope.status = {};

						// clear fields to prevent another submit
						$scope.payment.amount = '';
						$scope.payment.cardholderName = '';
						$scope.payment.cardNumber = '';
						$scope.payment.expirationMonth = '';
						$scope.payment.expirationYear = '';
						$scope.payment.securityCode = '';
						$scope.payment.postalCode = '';

						if (!$scope.$$phase) {
							$scope.$apply();
						}
					})
					.catch(function (results) {
						$scope.status = results || {};
						$scope.errorMessage = (results.errors && results.errors.length > 0 ? results.errors[0].errorMessage : "Update Error Handling to Find out!");
						if (!$scope.$$phase) {
							$scope.$apply();
						}
					});
			};
			
			var encrypt = function (value) {
				if (value)
					return braintree.encrypt(value);
				return '';
			}

			// initialize payment processing
			$scope.init();
		}
	])
	.controller('filters', []).
		filter('truncate', function () {
			return function (text, length, end) {
				if (isNaN(length))
					length = 80;

				if (end === undefined)
					end = "...";

				if (text.length <= length || text.length - end.length <= length) {
					return text;
				}
				else {
					return String(text).substring(0, length - end.length) + end;
				}
			};
		});

