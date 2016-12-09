

angular.module('appConsumer', ANGULAR_REQUIREMENTS)
	.config(['$routeProvider',
		function ($routeProvider) {
			$routeProvider.
				when('/', {
					templateUrl: '/App/Views/Consumer/home.html',
					controller: 'ConsumerHomeCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/portfolio/:id?', {
					templateUrl: '/App/Views/Consumer/portfolio.html',
					controller: 'ConsumerPortfolioCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/review/:id', {
					templateUrl: '/App/Views/Consumer/review.html',
					controller: 'ConsumerReviewCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/quotes/', {
					templateUrl: '/App/Views/Consumer/quotes.html',
					controller: 'ConsumerQuotesCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/quotes/:id', {
					templateUrl: '/App/Views/Consumer/quotes.html',
					controller: 'ConsumerQuotesCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/quote/:id', {
					templateUrl: '/App/Views/Consumer/quote.html',
					controller: 'ConsumerQuoteCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/payment/:id', {
					templateUrl: '/App/Views/Consumer/payment.html',
					controller: 'ConsumerPayment',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/how/consumer', {
					templateUrl: '/App/Views/All/How/how-consumer.html',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/documents', {
					templateUrl: '/App/Views/All/Support/documents.html',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/resources', {
					templateUrl: '/App/Views/All/Support/resources.html',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/support', {
					templateUrl: '/App/Views/All/Support/support.html',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				when('/account', {
					templateUrl: '/App/Views/Consumer/account.html',
					controller: 'ConsumerAccountCtrl',
					resolve: {
						init: ['$q', '$repo', function ($q, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();
									});
								return deferred.promise;
							}
						}]
					}
				}).
				otherwise({
					redirectTo: '/'
				});
		}
	])
	.controller('ConsumerHomeCtrl', ['$scope', '$location', '_', '$api', '$repo', '$enum', '$util', 'ConsumerMember',
		function ConsumerHomeCtrl($scope, $location, _, $api, $repo, $enum, $util, ConsumerMember) {

			// set the data with the consumer from the repository
			$scope.data = $repo.consumer.data
			$scope.enum = $enum;
			$scope.anyResponses = false;

			$scope.onPortfolioReview = function (portfolio) {
				$location.path('/review/' + portfolio.id);
			};

			$scope.onPortfolioEdit = function (portfolio) {
				$location.path('/portfolio/' + portfolio.id);
			};

			$scope.onPortfolioQuote = function (portfolio) {
				$location.path('/quote/' + portfolio.responseIdsAccepted[0]);
			};

			$scope.onPortfolioQuotes = function (portfolio) {
				$location.path('/quotes/' + portfolio.id);
			};

			$scope.onDeletePortfolio = function (portfolio) {
				$api.consumer.deletePortfolio(portfolio.toJSON())
					.then(function (results) {
						// update the consumer members (and all portfolios since this is a full scheme)
						$repo.consumer.updateMember(results);
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('ConsumerAccountCtrl', ['$scope', '$location', '$api', '$repo', 'ConsumerMember',
		function ConsumerAccountCtrl($scope, $location, $api, $repo, ConsumerMember) {

			// set the data with the consumer from the repository
			$scope.data = $repo.consumer.data;

			$scope.member = new ConsumerMember();
			$scope.member.update($scope.data.member.toJSON());

			$scope.onSubmit = function () {
				var member = $scope.member.toJSON();

				$api.consumer.updateConsumer(member)
					.then(function (results) {
						$repo.consumer.updateConsumerMember(results);

						// move to dashboard
						$location.path('/');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onCancel = function () {
				$location.path('/');
			};
		}
	])
	.controller('ConsumerPortfolioCtrl', ['$scope', '$routeParams', '$location', '_', '$api', '$repo', '$enum', 'ConsumerPortfolio', 'Profiler', '$filter',
		function ConsumerPortfolioCtrl($scope, $routeParams, $location, _, $api, $repo, $enum, ConsumerPortfolio, Profiler, $filter) {

			// set the data with the consumer from the repository
			$scope.data = $repo.consumer.data;
			$scope.enum = $enum;

			// update with current repo data (if possible)
			$scope.portfolio = new ConsumerPortfolio($repo.consumer.getPortfolio($routeParams.id));
			$scope.isNew = $scope.portfolio.id === undefined;

			// the profiler (for development/demo only)
			$scope.profilerPrincipal = new Profiler($scope, 'consumer.principal', $scope.portfolio.principal);
			$scope.profilerPreference = new Profiler($scope, 'consumer.preference', $scope.portfolio.preference);
			$scope.profilerSchedule = new Profiler($scope, 'consumer.schedule', $scope.portfolio.schedule);
			$scope.onProfiler = true;

			$scope.$on('profileChanged', function () {
				// the profiler has changed profiles so update the UI
				$scope.onProfiler = undefined;
				var t = setInterval(function () { $scope.onProfiler = true; clearInterval(t); $scope.$apply(); }, 1);
			});

			$scope.onCreatePortfolio = function () {
				var portfolio = $scope.portfolio.toJSON();

				$api.consumer.createPortfolio(portfolio)
					.then(function (results) {
						$repo.consumer.updateMember(results);

						$location.path('/portfolio/' + results.portfolioId);
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onUpdatePortfolio = function () {
				var portfolio = $scope.portfolio.toJSON();

				$api.consumer.updatePortfolio(portfolio)
					.then(function (results) {
						$repo.consumer.updatePortfolio(results);

						// where to go? is this a finish / exit?
						$location.path('/review/' + results.id);
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			// the dates have to be converted for display in the datepickers, convert to local var on scope then pass back on watch
			// should we do this locally in the factory ($filter on update) and avoid the watch?
			$scope.wakeDate = $filter("date")($scope.portfolio.schedule.wakeDate, 'yyyy-MM-dd');
			$scope.ceremonyDate = $filter("date")($scope.portfolio.schedule.ceremonyDate, 'yyyy-MM-dd');

			$scope.$watch('wakeDate', function (newValue, oldValue) {
				var value = document.getElementById("wakeDate").value;
				$scope.portfolio.schedule.wakeDate = $scope.wakeDate;
			});

			$scope.$watch('ceremonyDate', function (newValue, oldValue) {
				$scope.portfolio.schedule.ceremonyDate = $scope.ceremonyDate;
			});
		}
	])
	.controller('ConsumerReviewCtrl', ['$scope', '$routeParams', '$location', '_', '$api', '$repo', '$enum', 'ConsumerPortfolio',
		function ConsumerReviewCtrl($scope, $routeParams, $location, _, $api, $repo, $enum, ConsumerPortfolio) {

			// set the data with the consumer from the repository
			$scope.enum = $enum;
			$scope.data = $repo.consumer.data;

			// update with current repo data (if possible)
			$scope.portfolio = new ConsumerPortfolio($repo.consumer.getPortfolio($routeParams.id));

			$scope.onPortfolioEdit = function (portfolio) {
				$location.path('/portfolio/' + portfolio.id);
			};

			$scope.onSubmit = function () {
				var portfolio = $scope.portfolio.toJSON();

				$api.consumer.updateRequestAsPending(portfolio)
					.then(function (results) {
						$repo.consumer.updatePortfolio(results);

						// move to the specified URL
						$location.path('/home');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('ConsumerQuotesCtrl', ['$scope', '$location', '$routeParams', '_', '$api', '$repo', '$enum',
		function ConsumerQuotesCtrl($scope, $location, $routeParams, _, $api, $repo, $enum) {

			$scope.data = $repo.consumer.data;
			$scope.enum = $enum;
			$scope.portfolio = $repo.consumer.getPortfolio($routeParams.id);

			var responses = _.filter($scope.data.responsesAll, function (item) { return item.consumerPortfolioId === $routeParams.id; });
			$scope.responsesPending = _.filter(responses, function (item) { return item.current.state === $enum.responseReceiptStates.Pending.value; });
			$scope.responsesAccepted = _.filter(responses, function (item) { return item.current.state === $enum.responseReceiptStates.Accepted.value; });
			$scope.responsesRejected = _.filter(responses, function (item) { return item.current.state === $enum.responseReceiptStates.Rejected.value; });

			$scope.hasResponses = (_.size($scope.responsesPending) > 0 || _.size($scope.responsesAccepted) > 0 || _.size($scope.responsesRejected) > 0);
			$scope.anyResponses = (_.size(responses) > 0);

			$scope.responseUrl = function (response) {
				$location.path('/quote/' + response.id);
			}
		}
	])
	.controller('ConsumerQuoteCtrl', ['$scope', '$location', '$routeParams', '$modal', '$q', '_', '$api', '$repo', '$enum',
		function ConsumerQuoteCtrl($scope, $location, $routeParams, $modal, $q, _, $api, $repo, $enum) {

			// set the data with the consumer from the repository
			$scope.enum = $enum;
			$scope.data = $repo.consumer.data;
			$scope.response = _.find($scope.data.responsesAll, function (item) { return item.id === $routeParams.id; });
			$scope.portfolio = _.find($scope.data.portfoliosAll, function (item) { return item.id === $scope.response.consumerPortfolioId; });

			$scope.onAcceptResponse = function () {
				$location.path('/payment/' + $scope.response.id);
			};

			$scope.onRejectResponse = function () {
				var response = $scope.response.toJSON();

				$api.response.updateResponseAsRejected(response)
					.then(function (results) {
						$repo.consumer.updateResponse(results);

						// move to the specified URL
						$location.path('/quotes/' + $scope.response.consumerPortfolioId);
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.responseUrl = function (response) {
				return '#/quote/' + response.id;
			}
		}
	])
	.controller('ConsumerPayment', ['$scope', '$location', '$routeParams', '$modal', '$q', '_', 'braintree', '$api', '$repo', '$enum', '$busy', 'ResponseForm', 'Payment', 'Profiler',
		function ConsumerPayment($scope, $location, $routeParams, $modal, $q, _, Braintree, $api, $repo, $enum, $busy, ResponseForm, Payment, Profiler) {

			// set the response
			$scope.enum = $enum;
			$scope.busy = $busy;
			$scope.response = _.find($repo.consumer.data.responsesAll, function (item) { return item.id === $routeParams.id; });
			$scope.payment = new Payment();
			$scope.profiler = new Profiler($scope, 'payment', $scope.payment);

			$scope.isAccepted = $scope.response.current.state === $scope.enum.responseReceiptStates.Accepted.value;
			$scope.errorMessage = '';

			var braintree = undefined;

			$scope.init = function () {
				$scope.busy.show();

				// initialize payment processing
				$api.response.getPaymentConfiguration($scope.response)
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
				$scope.isAccepted = false;
				$scope.status = {};
				$scope.errorMessage = '';

				// encrypt the payment information
				var payment = new Payment(
				{
					cardholderName: encrypt($scope.payment.cardholderName),
					cardNumber: encrypt($scope.payment.cardNumber),
					expirationMonth: encrypt($scope.payment.expirationMonth),
					expirationYear: encrypt($scope.payment.expirationYear),
					securityCode: encrypt($scope.payment.securityCode),
					postalCode: encrypt($scope.payment.postalCode)
				});

				// create a response form with the encrypted data
				var responseForm = new ResponseForm({ id: $scope.response.id, payment: payment, current: $scope.response.current });

				// send the encrypted card to the server for validation, payment processing, and update as accepted
				$api.response.updateResponseAsAccepted(responseForm)
					.then(function (results) {
						$repo.consumer.updateResponse(results);

						$scope.isAccepted = true;
						if (!$scope.$$phase) {
							$scope.$apply();
						}
					})
					.catch(function (results) {
						$scope.status = results || {};
						$scope.errorMessage = (results.errors && results.errors.length > 0 ? results.errors[0].errorMessage : "Unknown Error");
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

			$scope.onClose = function () {
				// navigate to the accepted quote
				$location.path('/quote/' + $scope.response.id);
			};

			$scope.onCancel = function () {
				$location.path('/quote/' + $scope.response.id);
			};

			// initialize payment processing
			$scope.init();
		}
	]);


