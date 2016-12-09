

angular.module('requestSetPreference-directive', [])
	.directive('requestSetPreference', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var enumerations = $enum[attrs.enumKey ? attrs.enumKey : attrs.key];
				var preferenceKey = attrs.preferenceKey ? attrs.preferenceKey : attrs.key;
				var preference = scope.portfolio ? scope.portfolio.preference[preferenceKey] : scope.preference[preferenceKey];

				if (typeof enumerations === 'undefined' || typeof preference === 'undefined')
					return;

				// set description & explanation
				scope.description = attrs.description;
				scope.explanation = attrs.explanation;

				// set input type
				scope.inputType = attrs.inputType;

				// set enumerations (all except zero for radio, remove user specified for combo)
				if (scope.inputType === 'radio')
					scope.enumerations = _.filter(enumerations, function (item) { return item.value !== 0; });
				if (scope.inputType === 'combo')
					scope.enumerations = _.filter(enumerations, function (item) { return item.value !== $enum.USER_SPECIFIED_VALUE; });

				scope.isUserSpecified = false;

				scope.preferenceText = (preference.value === $enum.USER_SPECIFIED_VALUE ? preference.specified : enumerations[preference.value].text);

				// radio button selection
				scope.preferenceValue = preference.value;
				scope.$watch('preferenceValue', function (value) {
					if (scope.portfolio)
						scope.portfolio.preference[preferenceKey].value = value;
					else
						scope.preference[preferenceKey].value = value;

					scope.isUserSpecified = (value === $enum.USER_SPECIFIED_VALUE);
					if (scope.isUserSpecified) {
						if (scope.portfolio)
							scope.portfolio.preference[preferenceKey].specified = '';
						else
							scope.preference[preferenceKey].specified = '';
					}
				});

				// alternate specified text value
				scope.specifiedText = preference.specified;
				scope.$watch('specifiedText', function (text) {
					if (scope.portfolio)
						scope.portfolio.preference[preferenceKey].specified = text;
					else
						scope.preference[preferenceKey].specified = text;
				});

				// load HTML template and compile
				$http.get('/App/Templates/Shared/request-set-preference.html')
					.success(function (template) {
						// set the group name of each radio button to the key name
						var group = 'name=\"' + attrs.key + '\"';
						var html = template.replace(/data-group-name-replace-me/g, group);

						var validationProperty = 'property=\"' + attrs.key + '\"';
						var html = template.replace(/data-validation-property-replace-me/g, validationProperty);

						// load the html template
						element.html(html).show();

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('requestViewPreference-directive', [])
	.directive('trRequestViewPreference', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var enumKey = attrs.enumKey ? attrs.enumKey : attrs.key;
				var preferenceKey = attrs.preferenceKey ? attrs.preferenceKey : attrs.key;

				// get the request data. if this is a provider, the response will be set. if this is a consumer, the response will not be set.
				var portfolio = scope.$eval('portfolio');
				var response = scope.$eval('response');

				if (typeof portfolio === 'undefined' && typeof response === 'undefined')
					return;

				// build a request object from either the portfolio (consumer) or the response (provider)
				var request = (portfolio && portfolio.preference ? { preference: portfolio.preference, schedule: portfolio.schedule } : { preference: response.preference, schedule: response.schedule });
				if (typeof request === 'undefined')
					return;

				var enumerations = $enum[enumKey];
				if (typeof enumerations === 'undefined')
					return;

				// get the preferences value via the preference key
				var preference = request.preference[preferenceKey];
				if (!request.preference.hasOwnProperty(preferenceKey))
					return;

				// set description 
				scope.description = attrs.description;

				// set preferred choice
				if (preference.value === $enum.USER_SPECIFIED_VALUE)
					scope.preferredChoice = preference.specified;
				else
					scope.preferredChoice = _.find(enumerations, function (enumeration) { return enumeration.value === preference.value; }).text;

				// load HTML template and compile
				$http.get('/App/Templates/Shared/request-view-preference.html')
					.success(function (template) {

						// load the template
						element.html(template).show();

						// if the selected preference value is 0 (not chosen) or 1 (None value selected) then hide this element and return
						if (preference.value === 0) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('responseViewAgreementDate-directive', [])
	.directive('responseViewAgreementDate', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var agreementKey = attrs.agreementKey ? attrs.agreementKey : attrs.key;
				var alternateKey = attrs.alternateKey ? attrs.alternateKey : attrs.key;
				var scheduleKey = attrs.scheduleKey ? attrs.scheduleKey : attrs.key;

				var response = scope.$eval(attrs.ngModel);

				if (typeof response === 'undefined')
					return;

				var schedule = response.schedule[scheduleKey];
				var agreement = response.agreement[agreementKey];
				var alternate = response.alternate[alternateKey];

				if (typeof agreement === 'undefined' || typeof alternate === 'undefined')
					return;

				// set agreement data
				scope.noAgreement = (agreement === $enum.agreementTypes.NA.value);
				scope.isAgreed = (agreement === $enum.agreementTypes.Agreed.value);
				scope.isNotAgreed = (agreement === $enum.agreementTypes.NotAgreed.value);
				scope.isAlternate = (agreement === $enum.agreementTypes.Alternate.value);

				// set description 
				scope.description = attrs.description;

				// set preferred choice
				scope.scheduledChoice = schedule;

				// set alternate choice
				if (scope.isAlternate) {
					if (alternate.value === $enum.USER_SPECIFIED_VALUE)
						scope.alternateChoice = alternate.specified;
					else
						scope.alternateChoice = "Marty! Tell someone quick!!";
				}

				// load HTML template and compile
				$http.get('/App/Templates/Shared/response-view-agreement-date.html')
					.success(function (template) {

						// load the template
						element.html(template).show();

						// if the selected preference value is 0 (not chosen) or 1 (None value selected) then hide this element and return
						if (!schedule) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('responseViewAgreementList-directive', [])
	.directive('responseViewAgreementList', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var agreementKey = attrs.agreementKey ? attrs.agreementKey : attrs.key;
				var alternateKey = attrs.alternateKey ? attrs.alternateKey : attrs.key;
				var enumKey = attrs.enumKey ? attrs.enumKey : attrs.key;
				var preferenceKey = attrs.preferenceKey ? attrs.preferenceKey : attrs.key;

				var response = scope.$eval(attrs.ngModel);

				if (typeof response === 'undefined')
					return;

				// get the preferences. if this is a provider, it will be a property of the response. otherwise,  get from the consumer repo
				var preference = undefined;
				if (response.hasOwnProperty('preference'))
					preference = response.preference[preferenceKey];
				else
					preference = $repo.consumer.data.portfolio.preference[preferenceKey];


				var agreementValue = response.agreement[agreementKey];
				var enumerations = $enum[enumKey];
				var alternate = response.alternate[alternateKey];

				if (typeof agreementValue === 'undefined' || typeof alternate === 'undefined' || typeof enumerations === 'undefined' || typeof preference === 'undefined')
					return;

				// set agreement data
				scope.noAgreement = (agreementValue === $enum.agreementTypes.NA.value);
				scope.isAgreed = (agreementValue === $enum.agreementTypes.Agreed.value);
				scope.isNotAgreed = (agreementValue === $enum.agreementTypes.NotAgreed.value);
				scope.isAlternate = (agreementValue === $enum.agreementTypes.Alternate.value);

				// set description 
				scope.description = attrs.description;

				// set preferred choice
				if (preference.value === $enum.USER_SPECIFIED_VALUE)
					scope.preferredChoice = preference.specified;
				else
					scope.preferredChoice = _.find(enumerations, function (enumeration) { return enumeration.value === preference.value; }).text;

				// set alternate choice
				if (scope.isAlternate) {
					if (!alternate)
						scope.alternateChoice = "Marty! Tell someone quick!!";
					else if (alternate.value === $enum.USER_SPECIFIED_VALUE)
						scope.alternateChoice = alternate.specified;
					else
						scope.alternateChoice = _.find(enumerations, function (enumeration) { return enumeration.value === alternate.value; }).text;
				}

				// load HTML template and compile
				$http.get('/App/Templates/Shared/response-view-agreement-list.html')
					.success(function (template) {

						// load the template
						element.html(template).show();

						// if the selected preference value is 0 (not chosen) or 1 (None value selected) then hide this element and return
						if (preference.value === 0 || preference.value === 1) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('responseViewAgreementText-directive', [])
	.directive('responseViewAgreementText', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var agreementKey = attrs.agreementKey ? attrs.agreementKey : attrs.key;
				var alternateKey = attrs.alternateKey ? attrs.alternateKey : attrs.key;
				var preferenceKey = attrs.preferenceKey ? attrs.preferenceKey : attrs.key;

				var response = scope.$eval(attrs.ngModel);

				if (typeof response === 'undefined')
					return;

				var preference = response.preference[preferenceKey];
				var agreement = response.agreement[agreementKey];
				var alternate = response.alternate[alternateKey];

				if (typeof agreement === 'undefined' || typeof alternate === 'undefined')
					return;

				// set agreement data
				scope.noAgreement = (agreement === $enum.agreementTypes.NA.value);
				scope.isAgreed = (agreement === $enum.agreementTypes.Agreed.value);
				scope.isNotAgreed = (agreement === $enum.agreementTypes.NotAgreed.value);
				scope.isAlternate = (agreement === $enum.agreementTypes.Alternate.value);

				// set description 
				scope.description = attrs.description;

				// set preferred choice
				scope.preferredChoice = preference;

				// set alternate choice
				if (scope.isAlternate) {
					if (alternate.value === $enum.USER_SPECIFIED_VALUE)
						scope.alternateChoice = alternate.specified;
					else 
						scope.alternateChoice = "Marty! Tell someone quick!!";
				}

				// load HTML template and compile
				$http.get('/App/Templates/Shared/response-view-agreement-text.html')
					.success(function (template) {

						// load the template
						element.html(template).show();

						// if the selected preference value is 0 (not chosen) or 1 (None value selected) then hide this element and return
						if (!preference) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('responseSetAgreementDate-directive', [])
	.directive('responseSetAgreementDate', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var agreementKey = attrs.agreementKey ? attrs.agreementKey : attrs.key;
				var alternateKey = attrs.alternateKey ? attrs.alternateKey : attrs.key;
				var scheduleKey = attrs.scheduleKey ? attrs.scheduleKey : attrs.key;

				scope.response = scope.$eval(attrs.ngModel);

				if (typeof scope.response === 'undefined')
					return;

				var schedule = scope.response.schedule[scheduleKey];
				if (typeof schedule === 'undefined')
					return;

				// set description 
				scope.description = attrs.description;

				// set the text value
				scope.scheduleText = schedule;

				// initialize agreement selections
				scope.isAgreed = false;
				scope.isNotAgreed = false;
				scope.isAlternate = false;

				scope.setAgreement = function (agreement) {
					scope.isAgreed = false;
					scope.isNotAgreed = false;
					scope.isAlternate = false;

					if (agreement === 'isAgreed') {
						scope.isAgreed = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.Agreed.value;
					}
					if (agreement === 'isNotAgreed') {
						scope.isNotAgreed = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.NotAgreed.value;
					}
					if (agreement === 'isAlternate') {
						scope.isAlternate = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.Alternate.value;
					}
				};


				// alternate specified text value
				scope.specifiedText = '';
				scope.$watch('specifiedText', function (text) {
					if (scope.isAlternate) {
						scope.response.alternate[alternateKey].specified = text;
						scope.response.alternate[alternateKey].value = $enum.USER_SPECIFIED_VALUE;
					}
				});


				// load HTML template and compile
				$http.get('/App/Templates/Shared/response-set-agreement-date.html')
					.success(function (template) {
						// load the html template
						element.html(template).show();

						// if the selected schedule is null then hide this element and return
						if (!schedule) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('responseSetAgreementList-directive', [])
	.directive('responseSetAgreementList', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var agreementKey = attrs.agreementKey ? attrs.agreementKey : attrs.key;
				var alternateKey = attrs.alternateKey ? attrs.alternateKey : attrs.key;
				var preferenceKey = attrs.preferenceKey ? attrs.preferenceKey : attrs.key;

				scope.response = scope.$eval(attrs.ngModel);

				if (typeof scope.response === 'undefined')
					return;

				var enumerations = $enum[attrs.enumKey ? attrs.enumKey : attrs.key];
				var preference = scope.response.preference[preferenceKey];

				if (typeof enumerations === 'undefined' || typeof preference === 'undefined')
					return;

				// set description 
				scope.description = attrs.description;

				// set enumerations (except 0 value and the current one the user has selected) (note: if preference.value is the user specified, don't filter it out)
				scope.enumerations = _.filter(enumerations, function (item) { return item.value !== 0 && item.value !== (preference.value === $enum.USER_SPECIFIED_VALUE ? 0 : preference.value); });
				scope.isUserSpecified = false;

				scope.preferenceText = (preference.value === $enum.USER_SPECIFIED_VALUE ? preference.specified : enumerations[preference.value].text);
				scope.hasSelectedPreference = preference.value !== 0;

				// initialize agreement selections
				scope.isAgreed = false;
				scope.isNotAgreed = false;
				scope.isAlternate = false;

				scope.setAgreement = function (agreement) {
					scope.isAgreed = false;
					scope.isNotAgreed = false;
					scope.isAlternate = false;

					if (agreement === 'isAgreed') {
						scope.isAgreed = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.Agreed.value;
					}
					if (agreement === 'isNotAgreed') {
						scope.isNotAgreed = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.NotAgreed.value;
					}
					if (agreement === 'isAlternate') {
						scope.isAlternate = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.Alternate.value;
					}
				};


				// radio button selection
				if (!scope.response.alternate[alternateKey]) {
					alert();
				}
				scope.alternateValue = scope.response.alternate[alternateKey].value;
				scope.$watch('alternateValue', function (value) {
					scope.response.alternate[alternateKey].value = value;
					scope.isUserSpecified = (value === $enum.USER_SPECIFIED_VALUE);
					if (scope.isUserSpecified)
						scope.response.alternate[alternateKey].specified = '';
				});

				// alternate specified text value
				scope.specifiedText = '';
				scope.$watch('specifiedText', function (text) {
					scope.response.alternate[alternateKey].specified = text;
				});


				// load HTML template and compile
				$http.get('/App/Templates/Shared/response-set-agreement-list.html')
					.success(function (template) {
						// set the group name of each radio button to the key name
						var group = 'name=\"' + attrs.key + '\"';
						var html = template.replace(/data-group-name-replace-me/g, group);

						// load the html template
						element.html(html).show();

						// if the selected preference value is 0 (not chosen) or 1 (None value selected) then hide this element and return
						if (preference.value === 0 || preference.value === 1) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);


angular.module('responseSetAgreementText-directive', [])
	.directive('responseSetAgreementText', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var agreementKey = attrs.agreementKey ? attrs.agreementKey : attrs.key;
				var alternateKey = attrs.alternateKey ? attrs.alternateKey : attrs.key;
				var preferenceKey = attrs.preferenceKey ? attrs.preferenceKey : attrs.key;

				scope.response = scope.$eval(attrs.ngModel);

				if (typeof scope.response === 'undefined')
					return;

				var preference = scope.response.preference[preferenceKey];
				if (typeof preference === 'undefined')
					return;

				// set description 
				scope.description = attrs.description;

				scope.preferenceText = preference;
				scope.hasSelectedPreference = true;
				scope.isUserSpecified = true;

				// initialize agreement selections
				scope.isAgreed = false;
				scope.isNotAgreed = false;
				scope.isAlternate = false;

				scope.setAgreement = function (agreement) {
					scope.isAgreed = false;
					scope.isNotAgreed = false;
					scope.isAlternate = false;

					if (agreement === 'isAgreed') {
						scope.isAgreed = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.Agreed.value;
					}
					if (agreement === 'isNotAgreed') {
						scope.isNotAgreed = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.NotAgreed.value;
					}
					if (agreement === 'isAlternate') {
						scope.isAlternate = true;
						scope.response.agreement[agreementKey] = $enum.agreementTypes.Alternate.value;
					}
				};


				// alternate specified text value
				scope.specifiedText = '';
				scope.$watch('specifiedText', function (text) {
					if (scope.isAlternate) {
						scope.response.alternate[alternateKey].specified = text;
						scope.response.alternate[alternateKey].value = $enum.USER_SPECIFIED_VALUE;
					}
				});


				// load HTML template and compile
				$http.get('/App/Templates/Shared/response-set-agreement-text.html')
					.success(function (template) {
						// load the html template
						element.html(template).show();

						// if the selected preference value is 0 (not chosen) or 1 (None value selected) then hide this element and return
						if (!scope.preferenceText) {
							element.css('display', 'none');
							element.attr({ display: 'none' });
						}

						// compile
						$compile(element.contents())(scope);
					});
			}
		};
	}]);