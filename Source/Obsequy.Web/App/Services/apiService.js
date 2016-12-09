
angular.module('api-service', [])
 .service('$api', ['_', '$http', '$window', '$q', '$busy', function (_, $http, $window, $q, $busy) {

 	if (!!$window.apiService) {
 		return $window.apiService;
 	}

 	//#region Private Methods
 	var toDTO = function (dto) {
 		return JSON.stringify(dto);
 	}

 	var onSuccess = function (json, status, headers, config, dfd) {
 		json = json || {};

 		$busy.hide();

 		if (json.invalid) {
 			var isStale = (json.results && json.results.isStale);
 			if (isStale) {
				// force refresh of the browser
 				window.location = '/';
 			}
 			else {
 				// reject with invalid results: these are validation errors
 				dfd.reject(json.results);
 			}
 		}
 		else {
			if (json.hasOwnProperty('results'))
				dfd.resolve(json.results);
			else 
				dfd.resolve(json);
 		}
 	}

 	var onError = function (json, status, headers, config, dfd) {
 		json = json || {};

 		$busy.hide();

 		if (status === 400 || status === 401) {
 			// Bad Request or Unuathorized: force refresh of the browser
 			window.location = '/';
 		}
		else if (json.status) {
 			dfd.reject(json.status);
 		}
 		else {
 			dfd.reject(json);
 		}
 	}
 	//#endregion

 	//#region HTTP Methods
 	var httpDelete = function (url, dto) {
 		var dfd = $q.defer();

 		$busy.show();

 		$http.delete(url, dto)
 			.success(function (data, status, headers, config) {
 				onSuccess(data, status, headers, config, dfd);
 			})
 			.error(function (data, status, headers, config) {
 				onError(data, status, headers, config, dfd);
 			});

 		return dfd.promise;
 	}

 	var httpGet = function (url, dto) {
 		var dfd = $q.defer();

 		$busy.show();

 		$http.get(url)
 			.success(function (data, status, headers, config) {
 				onSuccess(data, status, headers, config, dfd);
 			})
 			.error(function (data, status, headers, config) {
 				onError(data, status, headers, config, dfd);
 			});

 		return dfd.promise;
 	}

 	var httpPost = function (url, dto) {
 		var dfd = $q.defer();

 		$busy.show();

 		$http.post(url, toDTO(dto))
 			.success(function (data, status, headers, config) {
 				onSuccess(data, status, headers, config, dfd);
 			})
 			.error(function (data, status, headers, config) {
 				onError(data, status, headers, config, dfd);
 			});

 		return dfd.promise;
 	}

 	var httpPut = function (url, dto) {
 		var dfd = $q.defer();

 		$busy.show();

 		$http.put(url, toDTO(dto))
 			.success(function (data, status, headers, config) {
 				onSuccess(data, status, headers, config, dfd);
 			})
 			.error(function (data, status, headers, config) {
 				onError(data, status, headers, config, dfd);
 			});

 		return dfd.promise;
 	}
 	//#endregion

 	$window.apiService = {
 		//#region Account
 		account: {
 			getMember: function () {
 				return httpGet('/api/account/member', undefined);
 			},
 			registerConsumer: function (dto) {
 				return httpPost('/api/account/registerconsumer', dto);
 			},
 			registerProvider: function (dto) {
 				return httpPost('/api/account/registerprovider', dto);
 			},
 			login: function (dto) {
 				return httpPost('/api/account/login', dto);
 			},
 			passwordRecovery: function (dto) {
 				return httpPut('/api/account/passwordrecovery', dto);
 			},
 			recoverPassword: function (dto) {
 			    return httpPost('/api/account/recoverpassword', dto);
 			},
 			incomingToken: function (dto) {// param, password , param, password
 			    return httpPost('/api/account/incomingtoken', dto);
 	        }
         },
 		//#endregion

 		//#region Admin
 		admin: {
 			getAdministratorMember: function (id) {
 				return httpGet('/api/administrator/member/' + id);
 			},
 			getAdministratorMembers: function () {
 				return httpGet('/api/administrator/members', undefined);
 			},
 			createAdministratorMember: function (dto) {
 				return httpPost('/api/administrator/member', dto);
 			},
 			updateAdministratorMember: function (dto) {
 				return httpPut('/api/administrator/member', dto);
 			},
 			deleteAdministratorMember: function (dto) {
 				return httpPost('/api/administrator/deletemember', dto);
 			},
 			getProviders: function () {
 				return httpGet('/api/administrator/providers', undefined);
 			},
 			getPendingProviders: function () {
 				return httpGet('/api/administrator/pendingproviders', undefined);
 			},
 			getActiveProviders: function () {
 			    return httpGet('/api/administrator/activeproviders', undefined);
 			},
 			getSuspendedProviders: function () {
 			    return httpGet('/api/administrator/suspendedproviders', undefined);
 			},
 			getRejectedProviders: function () {
 			    return httpGet('/api/administrator/rejectedproviders', undefined);
 			},
 			updateProviderAccountStatus: function (dto) {
 				return httpPut('/api/administrator/updateprovideraccountstatus', dto);
 			},
 			getConsumerPortfolios: function () {
 				return httpGet('/api/administrator/consumerportfolios');
 			},
 			getConsumerPortfolio: function (id) {
 				return httpGet('/api/administrator/consumerportfolio/' + id);
 			},
 			getProviderPortfolio: function (id) {
 				return httpGet('/api/administrator/providerportfolio/' + id);
 			},
 			deleteConsumerPortfolio: function (id) {
 			    return httpPost('/api/administrator/deleteconsumerportfolio/' + id);
 			},
 			remindConsumerPortfolio: function (id) {
 			    return httpPost('/api/administrator/remindconsumerportfolio/' + id);
 			},
 			testPayment: function (dto) {
 				return httpPost('/api/administrator/testpayment', dto);
 			}
 		},
 		//#endregion

 		//#region Consumer
 		consumer: {
 			updateConsumer: function (dto) {
 				return httpPut('/api/consumer/consumer', dto);
 			},
 			createConsumer: function (dto) {
 				return httpPost('/api/consumer/consumer', dto);
 			},
 			deleteConsumer: function (dto) {
 				return httpPost('/api/consumer/consumer', dto);
 			},
 			createPortfolio: function (dto) {
 				return httpPost('/api/consumer/portfolio', dto);
 			},
 			updatePortfolio: function (dto) {
 				return httpPut('/api/consumer/portfolio', dto);
 			},
 			deletePortfolio: function (dto) {
 				return httpPost('/api/consumer/deleteportfolio', dto);
 			},
 			updateRequestAsDraft: function (dto) {
 				return httpPut('/api/consumer/updaterequestasdraft', dto);
 			},
 			updateRequestAsPending: function (dto) {
 				return httpPut('/api/consumer/updaterequestaspending', dto);
 			}
 		},
 		//#endregion

 		//#region Provider
 		provider: {
 			updateProvider: function (dto) {
 				return httpPut('/api/provider/provider', dto);
 			},
 			getProvider: function () {
 				return httpGet('/api/provider/provider', undefined);
 			},
 			createPortfolio: function (dto) {
 				return httpPost('/api/provider/portfolio', dto);
 			},
 			updatePortfolio: function (dto) {
 				return httpPut('/api/provider/portfolio', dto);
 			}
 		},
 		//#endregion

 		//#region Response
 		response: {
 			getPaymentConfiguration: function () {
 				return httpGet('/api/response/paymentconfiguration', undefined);
 			},
 			getConsumerResponses: function () {
 				return httpGet('/api/response/consumerresponses', undefined);
 			},
 			getProviderResponses: function () {
 				return httpGet('/api/response/providerresponses', undefined);
 			},
 			updateResponseAsAccepted: function (dto) {
 				return httpPut('/api/response/accepted', dto);
 			},
 			updateResponseAsAvailable: function (dto) {
 				return httpPut('/api/response/available', dto);
 			},
 			updateResponseAsDismissed: function (dto) {
 				return httpPut('/api/response/dismissed', dto);
 			},
 			updateResponseAsPending: function (dto) {
 				return httpPut('/api/response/pending', dto);
 			},
 			updateResponseAsRecalled: function (dto) {
 				return httpPut('/api/response/recalled', dto);
 			},
 			updateResponseAsRejected: function (dto) {
 				return httpPut('/api/response/rejected', dto);
 			}
 		},
 		//#endregion

 		//#region Validation
 		validation: {
 			email: function (dto) {
 				return httpPut('/api/validation/email', dto);
 			},
 			password: function (dto) {
 				return httpPut('/api/validation/password', dto);
 			}
 		}
 		//#endregion
 	};

 	return $window.apiService;
 }]);

