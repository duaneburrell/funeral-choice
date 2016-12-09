
angular.module('repository-service', [])
	.service('$repo', ['$window', '$rootScope', '$q', '$api', '$enum', 'ConsumerMember', 'ConsumerPortfolio', 'ConsumerResponse', 'ProviderMember', 'ProviderPortfolio', 'ProviderResponse', 
		function ($window, $rootScope, $q, $api, $enum, ConsumerMember, ConsumerPortfolio, ConsumerResponse, ProviderMember, ProviderPortfolio, ProviderResponse) {

		if (!!$window.repositoryService) {
			return $window.repositoryService;
		}

		//#region Consumer Hub

		var runConsumerHub = function () {
		    if (!$.connection)
		        return;
			if ($.connection.hub.state !== $.signalR.connectionState.disconnected)
				return;

			var repo = $window.repositoryService;
			var hub = $.connection.consumerHub;

			// watch for consumer response updates
			hub.client.responseUpdated = function (results) {
				repo.consumer.updateResponse(results);
			};

			$.connection.hub.logging = true;
			$.connection.hub.start();
		};

		//#endregion

		//#region Provider Hub

		var runProviderHub = function () {
		    if (!$.connection)
		        return;
			if ($.connection.hub.state !== $.signalR.connectionState.disconnected)
				return;

			var repo = $window.repositoryService;
			var hub = $.connection.providerHub;

			// watch for consumer response updates
			hub.client.responseUpdated = function (results) {
				repo.provider.updateResponse(results);
			};

			$.connection.hub.logging = true;
			$.connection.hub.start();
		};

		//#endregion

		//#region Data

		// member: the current member
		// portfolio: the selected portfolio
		// portfolios: the other portfolios for this member not selected
		// portfoliosAll: all portfolios for this member
		// responses: responses for the selected portfolio
		// responsesAll: all responses for all portfolios

		var data = {
			consumer: {
				member: new ConsumerMember(),
				portfoliosAll: [],
				responsesPending: [],
				responsesAccepted: [],
				responsesRejected: [],
				responsesAll: [],

				hasMember: false,
				anyPortfolios: false,
				anyResponses: false
			},

			provider: {
				member: new ProviderMember(),
				portfolio: new ProviderPortfolio(),
				portfolios: [],
				portfoliosAll: [],
				responses: [],
				responsesAvailable: [],
				responsesAccepted: [],
				responsesPending: [],
				responsesRejected: [],
				responsesDismissed: [],
				responsesLast24Hours: [],
				responsesLast7Days: [],
				responsesLast30Days: [],
				responsesAll: [],

				hasMember: false,
				hasPortfolio: false,
				hasPortfolios: false,
				anyPortfolios: false,
				hasResponses: false,
				anyResponses: false
			},

			initialized: false
		};

		//#endregion

		//#region Consumer Methods

		var findConsumerPortfolio = function (id) {
			// find the portfolio for this id
			var portfolio = _.find(this.data.portfoliosAll, function (item) { return item.id === id; });
			if (portfolio)
				return portfolio.toJSON();
			return {};
		};

		var updateConsumerMember = function (member) {

		    this.data.portfoliosAll = [];

			// update the consumer member
			this.data.member.update(member);

			if (_.size(member.portfolios) > 0) {

				// do we have any items?
				if (_.size(this.data.portfoliosAll) === 0) {

					_.each(member.portfolios, function (item) {
						// add a new consumer portfolio
						this.data.portfoliosAll.push(new ConsumerPortfolio(item));
					}, this);
				}
				else {
					// update existing items
					_.each(member.portfolios, function (item) {
						// find the portfolio
						var portfolio = _.find(this.data.portfoliosAll, function (portfolioAll) { return portfolioAll.id === item.id; });
						if (portfolio) {
							// update the existing consumer portfolio
							portfolio.update(item);
						}
						else {
							// add a new consumer portfolio
							this.data.portfoliosAll.push(new ConsumerPortfolio(item));
						}
					}, this);
				}
			}
			else if (!member.portfolioId) {
				this.data.portfoliosAll = [];
			}

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var updateConsumerPortfolio = function (portfolio) {

			var member = _.find(portfolio.members, function (item) { return item.id === this.data.member.id; }, this);

			// update the current member
			this.data.member.update(member);

			// update all portfolios
			_.each(this.data.portfoliosAll, function (item) {
				if (item.id === portfolio.id)
					item.update(portfolio);
			});

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var updateConsumerResponse = function (response) {

			// find the response
			var repoResponse = _.find(this.data.responsesAll, function (item) { return item.id === response.id; });

			if (repoResponse)
				repoResponse.update(response);
			else
				this.data.responsesAll.push(new ConsumerResponse(response));

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var updateConsumerResponses = function (responses) {

			if (_.size(responses) > 0) {

				// do we have any items?
				if (_.size(this.data.responsesAll) === 0) {

					_.each(responses, function (item) {
						// add a new response
						this.data.responsesAll.push(new ConsumerResponse(item));
					}, this);
				}
				else {
					// update existing responses
					_.each(member.portfolios, function (item) {
						// find the response
						var response = _.find(this.data.responsesAll, function (responseAll) { return responseAll.id === item.id; });
						if (response) {
							// update the existing response
							response.update(item);
						}
						else {
							// add a new response
							this.data.responsesAll.push(new ConsumerResponse(item));
						}
					}, this);
				}
			}
			else if (_.size(this.data.responsesAll) !== 0) {

				// clear all responses
				this.data.responsesAll = [];
			}

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var refreshConsumerData = function () {

			// refresh all computed arrays
			this.data.responsesPending = _.filter(this.data.responsesAll, function (item) { return item.current.state === $enum.responseReceiptStates.Pending.value; });
			this.data.responsesAccepted = _.filter(this.data.responsesAll, function (item) { return item.current.state === $enum.responseReceiptStates.Accepted.value; });
			this.data.responsesRejected = _.filter(this.data.responsesAll, function (item) { return item.current.state === $enum.responseReceiptStates.Rejected.value; });

			// refresh computed properties
			this.data.hasMember = (this.data.member.id ? true : false);
			this.data.anyPortfolios = (_.size(this.data.portfoliosAll) > 0);
			this.data.anyResponses = (_.size(this.data.responsesAll) > 0);

			// iterate over all consumer portfolios and set their response ids
			_.each(this.data.portfoliosAll, function (portfolio) {
				portfolio.responseIdsAccepted = _.chain(this.data.responsesAccepted).filter(function (item) { return item.consumerPortfolioId === portfolio.id; }).pluck('id').value();
				portfolio.responseIdsPending = _.chain(this.data.responsesPending).filter(function (item) { return item.consumerPortfolioId === portfolio.id; }).pluck('id').value();
				portfolio.responseIdsRejected = _.chain(this.data.responsesRejected).filter(function (item) { return item.consumerPortfolioId === portfolio.id; }).pluck('id').value();

				portfolio.totalResponsesGenerated = _.filter(this.data.responsesAll, function (item) { return item.consumerPortfolioId === portfolio.id }).length;
				portfolio.totalResponsesAcknowledged = portfolio.responseIdsAccepted.length + portfolio.responseIdsPending.length + portfolio.responseIdsRejected.length;

			}, this);

			// broadcast update event
			$rootScope.$broadcast('dataChanged', this.data);
		};

		//#endregion

		//#region Provider Methods

		var findProviderPortfolio = function (id) {
			// find the portfolio for this id
			var portfolio = _.find(this.data.portfoliosAll, function (item) { return item.id === id; });
			if (portfolio)
				return portfolio.toJSON();
			return {};
		};

		var updateProviderMember = function (member) {

			// update the provier member
			this.data.member.update(member);
            
			if (_.size(member.portfolios) > 0) {

				// do we have any items?
				if (_.size(this.data.portfoliosAll) === 0) {

					_.each(member.portfolios, function (item) {
						// add a new portfolio
						this.data.portfoliosAll.push(new ProviderPortfolio(item));
					}, this);
				}
				else {

					// update existing items
					_.each(member.portfolios, function (item) {
						// find the portfolio
						var portfolio = _.find(this.data.portfoliosAll, function (portfolioAll) { return portfolioAll.id === item.id; });
						if (portfolio) {
							// update the existing portfolio
							portfolio.update(item);
						}
						else {
							// add a new portfolio
							this.data.portfoliosAll.push(new ProviderPortfolio(item));
						}
					}, this);
				}

				// update the current portfolio
				this.data.portfolio.update(_.find(member.portfolios, function (item) { return item.id === this.data.member.portfolioId; }, this));
			}
			else if (!member.portfolioId) {
				// clear all portfolios
				this.data.portfoliosAll = [];
			}

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var updateProviderPortfolio = function (portfolio) {
			var member = _.find(portfolio.members, function (item) { return item.id === this.data.member.id; }, this);

			// update the curent instance
			this.data.portfolio.update(portfolio);

			// update the current member
			this.data.member.update(member);

			// update all portfolios
			_.each(this.data.portfoliosAll, function (item) {
				if (item.id === portfolio.id)
					item.update(portfolio);
			});

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var updateProviderResponse = function (response) {

			// find the response
			var repoResponse = _.find(this.data.responsesAll, function (item) { return item.id === response.id; });
			
			if (repoResponse)
				repoResponse.update(response);
			else
				this.data.responsesAll.push(new ProviderResponse(response));

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var updateProviderResponses = function (responses) {
			if (_.size(responses) > 0) {

				// do we have any items?
				if (_.size(this.data.responsesAll) === 0) {

					_.each(responses, function (item) {
						// add a new response
						this.data.responsesAll.push(new ProviderResponse(item));
					}, this);
				}
				else {
					// update existing responses
					_.each(member.portfolios, function (item) {
						// find the response
						var response = _.find(this.data.responsesAll, function (responseAll) { return responseAll.id === item.id; });
						if (response) {
							// update the existing response
							response.update(item);
						}
						else {
							// add a new response
							this.data.responsesAll.push(new ProviderResponse(item));
						}
					}, this);
				}
			}
			else if (_.size(this.data.responsesAll) !== 0) {
				// remove all responses
				this.data.responsesAll = [];
			}

			// refresh computed values (this will broadcast changes)
			this.refresh();
		};

		var refreshProviderData = function () {

			// refresh all computed arrays
			this.data.portfolios = _.filter(this.data.portfoliosAll, function (item) { return item.id !== this.data.member.portfolioId; }, this);
			this.data.responses = _.filter(this.data.responsesAll, function (item) { return item.providerPortfolioId === this.data.member.portfolioId; }, this);
			this.data.responsesAvailable = _.filter(this.data.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Available.value; });
			this.data.responsesAccepted = _.filter(this.data.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Accepted.value; });
			this.data.responsesPending = _.filter(this.data.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Pending.value; });
			this.data.responsesRejected = _.filter(this.data.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Rejected.value; });
			this.data.responsesDismissed = _.filter(this.data.responses, function (item) { return item.current.state === $enum.responseReceiptStates.Dismissed.value; });

			// get the current time
			var now = moment();

			// build responses within past 24 hours
			this.data.responsesLast24Hours = _.filter(this.data.responses, function (item) {
				return (moment.duration(now.diff(moment(item.lastActivity)))).days() < 1;
			});

			// build responses within past 7 days
			this.data.responsesLast7Days = _.filter(this.data.responses, function (item) {
				return (moment.duration(now.diff(moment(item.lastActivity)))).days() <= 7;
			});
			this.data.responsesLast7Days = _.difference(this.data.responsesLast7Days, this.data.responsesLast24Hours);

			// build responses within past 30 days
			this.data.responsesLast30Days = _.filter(this.data.responses, function (item) {
				return (moment.duration(now.diff(moment(item.lastActivity)))).days() <= 30;
			});
			this.data.responsesLast30Days = _.difference(this.data.responsesLast30Days, this.data.responsesLast24Hours);
			this.data.responsesLast30Days = _.difference(this.data.responsesLast30Days, this.data.responsesLast7Days);

			// refresh computed properties
			this.data.hasMember = (this.data.member.id ? true : false);
			this.data.hasPortfolio = (this.data.portfolio.id ? true : false);
			this.data.hasPortfolios = (_.size(this.data.portfolios) > 0);
			this.data.anyPortfolios = (_.size(this.data.portfoliosAll) > 0);
			this.data.hasResponses = (_.size(this.data.responsesPending) > 0 || _.size(this.data.responsesAccepted) > 0 || _.size(this.data.responsesRejected) > 0);
			this.data.anyResponses = (_.size(this.data.responsesAll) > 0);

			// broadcast update event
			$rootScope.$broadcast('dataChanged', this.data);
		};

		//#endregion

		//#region Load Member

		var loadMember = function () {
			var dfd = $q.defer();
			var repo = $window.repositoryService;

			$api.account.getMember()
				.then(function (results) {
					if (results) {

						if (results.accountType == $enum.accountType.Consumer.value) {
							// update the consumer member (this will also update the consumer portfolios)
							repo.consumer.updateMember(results);

							// start the hub (if the member has a portfolio)
							repo.runConsumerHub();

							// load the responses for this member's current portfolio
							$api.response.getConsumerResponses()
								.then(function (results) {
									repo.consumer.updateResponses(results);

									dfd.resolve();
								})
								.catch(function (results) {
									dfd.reject(results);
								});
						}

						if (results.accountType == $enum.accountType.Provider.value) {
							// update the provider member (this will also update the provider portfolios)
							repo.provider.updateMember(results);

							// start the hub (if the member has a portfolio)
							repo.runProviderHub();

							// load the responses for this member's current portfolio
							$api.response.getProviderResponses()
								.then(function (results) {
									repo.provider.updateResponses(results);

									dfd.resolve();
								})
								.catch(function (results) {
									dfd.reject(results);
								});
						}
					}
					else {
						dfd.resolve();
					}

					// set as initialized
					// note: this isn't 100% initialized, but it's primarily for UI initialization, so it works here
					repo.initialized = true;
				})
				.catch(function (results) {
					dfd.reject(results);
				});

			return dfd.promise;
		}

		//#endregion

		$window.repositoryService = {

			consumer: {
				data: data.consumer,

				getPortfolio: findConsumerPortfolio,
				updateMember: updateConsumerMember,
				updatePortfolio: updateConsumerPortfolio,
				updateResponse: updateConsumerResponse,
				updateResponses: updateConsumerResponses,
				refresh: refreshConsumerData,
			},

			provider: {
				data: data.provider,

				getPortfolio: findProviderPortfolio,
				updateMember: updateProviderMember,
				updatePortfolio: updateProviderPortfolio,
				updateResponse: updateProviderResponse,
				updateResponses: updateProviderResponses,
				refresh: refreshProviderData
			},

			runConsumerHub: runConsumerHub,
			runProviderHub: runProviderHub,
			initialized: data.initialized,
			run: loadMember
		};

		return $window.repositoryService;
	}]);
