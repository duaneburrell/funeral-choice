

angular.module('appProvider', ANGULAR_REQUIREMENTS)
	.config(['$routeProvider',
		function ($routeProvider) {
			$routeProvider.
				when('/', {
					templateUrl: '/App/Views/Provider/home.html',
					controller: 'ProviderHomeCtrl',
					resolve: {
						init: ['$q', '$location', '$repo', function ($q, $location, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();

										if (!$repo.provider.data.hasPortfolio)
											$location.path('/portfolio');
									});
								return deferred.promise;
							}
							else if (!$repo.provider.data.hasPortfolio)
								$location.path('/portfolio');
						}]
					}
				}).
				when('/portfolio/:id?', {
					templateUrl: '/App/Views/Provider/portfolio.html',
					controller: 'ProviderPortfolioCtrl',
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
				when('/quotes', {
					templateUrl: '/App/Views/Provider/quotes.html',
					controller: 'ProviderQuotesCtrl',
					resolve: {
						init: ['$q', '$location', '$repo', function ($q, $location, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();

										if (!$repo.provider.data.hasPortfolio)
											$location.path('/portfolio');
									});
								return deferred.promise;
							}
							else if (!$repo.provider.data.hasPortfolio)
								$location.path('/portfolio');
						}]
					}
				}).
				when('/quote/:id', {
					templateUrl: '/App/Views/Provider/quote.html',
					controller: 'ProviderQuoteCtrl',
					resolve: {
						init: ['$q', '$location', '$repo', function ($q, $location, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();

										if (!$repo.provider.data.hasPortfolio)
											$location.path('/portfolio');
									});
								return deferred.promise;
							}
							else if (!$repo.provider.data.hasPortfolio)
								$location.path('/portfolio');
						}]
					}
				}).
				when('/account', {
					templateUrl: '/App/Views/Provider/account.html',
					controller: 'ProviderAccountCtrl',
					resolve: {
						init: ['$q', '$location', '$repo', function ($q, $location, $repo) {
							if (!$repo.initialized) {
								var deferred = $q.defer();
								$repo.run()
									.then(function () {
										deferred.resolve();

										if (!$repo.provider.data.hasPortfolio)
											$location.path('/portfolio');
									});
								return deferred.promise;
							}
							else if (!$repo.provider.data.hasPortfolio)
								$location.path('/portfolio');
						}]
					}
				}).
				when('/how/provider', {
					templateUrl: '/App/Views/All/How/how-provider.html',
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
	.controller('ProviderHomeCtrl', ['$scope', '$location', '_', '$api', '$repo', '$enum',
		function ProviderHomeCtrl($scope, $location, _, $api, $repo, $enum) {

			// set the data with the provider from the repository
			$scope.data = $repo.provider.data;
			$scope.enum = $enum;

			$scope.go = function (path) {
				$location.path(path);
			};
		}
	])
	.controller('ProviderPortfolioCtrl', ['$scope', '$routeParams', '$location', '_', '$api', '$repo', 'ProviderPortfolio', 'Profiler', '$enum',
		function ProviderPortfolioCtrl($scope, $routeParams, $location, _, $api, $repo, ProviderPortfolio, Profiler, $enum) {

			// set the data with the provider from the repository
			$scope.data = $repo.provider.data;
			$scope.enum = $enum;

			// update with current repo data (if possible)
			$scope.portfolio = new ProviderPortfolio($scope.data.portfolio.toJSON());
			$scope.isNew = $scope.portfolio.id === undefined;

			// the profiler (for development/demo only)
			$scope.profilerPrincipal = new Profiler($scope, 'provider.principal', $scope.portfolio.principal);
			$scope.profilerProfile = new Profiler($scope, 'provider.profile', $scope.portfolio.profile);
			$scope.onProfiler = true;

			$scope.$on('profileChanged', function () {
				// the profiler has changed profiles so update the UI
				$scope.onProfiler = undefined;
				var t = setInterval(function () { $scope.onProfiler = true; clearInterval(t); $scope.$apply(); }, 1);
			});


			$scope.onCreatePortfolio = function () {
				var portfolio = $scope.portfolio.toJSON();

				$api.provider.createPortfolio(portfolio)
					.then(function (results) {
						$repo.provider.updateMember(results);

						$location.path('/portfolio/' + results.portfolioId);
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};


			$scope.onUpdatePortfolio = function () {
				var portfolio = $scope.portfolio.toJSON();

				$api.provider.updatePortfolio(portfolio)
					.then(function (results) {
						$repo.provider.updatePortfolio(results);

						$location.path('/');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('ProviderQuotesCtrl', ['$scope', '$location', '$routeParams', '_', '$api', '$repo', '$enum', 'ProviderResponse',
		function ProviderQuotesCtrl($scope, $location, $routeParams, _, $api, $repo, $enum, ProviderResponse) {

			// set the data with the provider from the repository
			$scope.enum = $enum;
			$scope.data = $repo.provider.data;
			$scope.response = _.find($scope.data.responses, function (item) { return item.id === $routeParams.id; });
			$scope.selectedTab = 'available';

			$scope.onSetDismissedResponse = function () {
				var response = $scope.response.toJSON();

				$api.response.updateResponseAsDismissed(response)
					.then(function (results) {
						$repo.provider.updateResponse(results);
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.directive('showtab',
		function () {
			return {
				link: function (scope, element, attrs) {
					element.click(function (e) {
						e.preventDefault();
						$(element).tab('show');
					});
				}
			};
		})
	.controller('ProviderQuoteCtrl', ['$scope', '$location', '$rootScope', '$routeParams', '_', '$api', '$repo', '$enum', 'ProviderResponse',
		function ProviderQuoteCtrl($scope, $location, $rootScope, $routeParams, _, $api, $repo, $enum, ProviderResponse) {

			// set the data with the provider from the repository
			$scope.enum = $enum;
			$scope.data = $repo.provider.data;
			$scope.response = _.find($scope.data.responses, function (item) { return item.id === $routeParams.id; });

			// watch for broadcast events on the rootscope
			$scope.$on('dataChanged', function () {
				$scope.response = _.find($scope.data.responses, function (item) { return item.id === $routeParams.id; });
			});

			$scope.onSetPendingResponse = function () {
				var response = $scope.response.toJSON();

				$api.response.updateResponseAsPending(response)
					.then(function (results) {
						$repo.provider.updateResponse(results);
						$location.path('/');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onSetDismissedResponse = function () {
				var response = $scope.response.toJSON();

				$api.response.updateResponseAsDismissed(response)
					.then(function (results) {
					    $repo.provider.updateResponse(results);
					    $location.path('/');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onSetAvailableResponse = function () {
				var response = $scope.response.toJSON();

				$api.response.updateResponseAsAvailable(response)
					.then(function (results) {
						$repo.provider.updateResponse(results);
						$location.path('/');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};
		}
	])
	.controller('ProviderAccountCtrl', ['$scope', '$location', '_', '$api', '$repo', 'ProviderMember',
		function ProviderAccountCtrl($scope, $location, _, $api, $repo, ProviderMember) {

			// set the data with the provider from the repository
			$scope.data = $repo.provider.data;

			// update with current repo data (if possible)
			$scope.member = new ProviderMember();
			$scope.member.update($scope.data.member.toJSON());

			$scope.onSubmit = function () {
				var member = $scope.member.toJSON();

				$api.provider.updateProvider(member)
					.then(function (results) {
						$repo.provider.updateProviderMember(results);

						// move to dashboard
						$location.path('/');
					})
					.catch(function (results) {
						$scope.status = results || {};
					});
			};

			$scope.onCancel = function (action) {
				$location.path('/');
			};
		}
	]);

