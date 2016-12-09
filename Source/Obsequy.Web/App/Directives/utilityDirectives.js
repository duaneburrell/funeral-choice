
angular.module('accountPrestige-directive', [])
	.directive('accountPrestige', ['$enum', function ($enum) {
		return {
			restrict: 'A',
			link: function (scope, element, attrs) {
				scope.$watch(attrs.accountPrestige, function (value) {
					var value = Number(attrs.accountPrestige);
					var text = "Unknown";

					// try to find a match
					text = (value === $enum.accountPrestige.None.value ? $enum.accountPrestige.None.text : text);
					text = (value === $enum.accountPrestige.Creator.value ? $enum.accountPrestige.Creator.text : text);
					text = (value === $enum.accountPrestige.Contributor.value ? $enum.accountPrestige.Contributor.text : text);

					// set the text value
					element.text(text);
				});
			}
		};
	}]);


angular.module('accountPrivilidge-directive', [])
	.directive('accountPrivilidge', ['$enum', function ($enum) {
		return {
			restrict: 'A',
			link: function (scope, element, attrs) {
				scope.$watch(attrs.accountPrivilidge, function (value) {
					var value = Number(attrs.accountPrivilidge);
					var text = "Unknown";

					// try to find a match
					text = (value === $enum.accountPrivilidge.None.value ? $enum.accountPrivilidge.None.text : text);
					text = (value === $enum.accountPrivilidge.Elevated.value ? $enum.accountPrivilidge.Elevated.text : text);
					text = (value === $enum.accountPrivilidge.Standard.value ? $enum.accountPrivilidge.Standard.text : text);

					// set the text value
					element.text(text);
				});
			}
		};
	}]);


angular.module('accountStatus-directive', [])
	.directive('accountStatus', ['_', '$enum', function (_, $enum) {
		return {
			restrict: 'EA',
			link: function (scope, element, attrs) {
				scope.$watch(attrs.accountStatus, function (value) {
					var value = Number(attrs.accountStatus);
					var text = "Unknown";

					// try to find a match
					text = (value === $enum.accountStatus.None.value ? $enum.accountStatus.None.text : text);
					text = (value === $enum.accountStatus.Pending.value ? $enum.accountStatus.Pending.text : text);
					text = (value === $enum.accountStatus.Active.value ? $enum.accountStatus.Active.text : text);
					text = (value === $enum.accountStatus.Disabled.value ? $enum.accountStatus.Disabled.text : text);
					text = (value === $enum.accountStatus.Suspended.value ? $enum.accountStatus.Suspended.text : text);
					text = (value === $enum.accountStatus.Terminated.value ? $enum.accountStatus.Terminated.text : text);
					text = (value === $enum.accountStatus.Rejected.value ? $enum.accountStatus.Rejected.text : text);

					// set the text value
					element.text(text);
				});
			}
		};
	}]);


angular.module('businessEstablished-directive', [])
	.directive('businessEstablished', ['_', '$enum', function (_, $enum) {
	    return {
	        restrict: 'EA',
	        link: function (scope, element, attrs) {
	            scope.$watch(attrs.businessEstablished, function (value) {
	                var value = Number(attrs.businessEstablished);
	                var text = "Unknown";

	                // try to find a match
	                text = _.find($enum.businessEstablishedTypes, function (businessEstablishedType) { return businessEstablishedType.value === value; } ).text;

	                // set the text value
	                element.text(text);
	            });
	        }
	    };
	}]);


angular.module('facilityAge-directive', [])
	.directive('facilityAge', ['_', '$enum', function (_, $enum) {
	    return {
	        restrict: 'EA',
	        link: function (scope, element, attrs) {
	            scope.$watch(attrs.facilityAge, function (value) {
	                var value = Number(attrs.facilityAge);
	                var text = "Unknown";

	                // try to find a match
	                text = _.find($enum.facilityAgeTypes, function (facilityAgeType) { return facilityAgeType.value === value; }).text;

	                // set the text value
	                element.text(text);
	            });
	        }
	    };
	}]);


angular.module('facilityStyle-directive', [])
	.directive('facilityStyle', ['_', '$enum', function (_, $enum) {
	    return {
	        restrict: 'EA',
	        link: function (scope, element, attrs) {
	            scope.$watch(attrs.facilityStyle, function (value) {
	                var value = Number(attrs.facilityStyle);
	                var text = "Unknown";

	                // try to find a match
	                text = _.find($enum.facilityStyleTypes, function (facilityStyleType) { return facilityStyleType.value === value; } ).text;

	                // set the text value
	                element.text(text);
	            });
	        }
	    };
	}]);


angular.module('funeralDirectorExperience-directive', [])
	.directive('funeralDirectorExperience', ['_', '$enum', function (_, $enum) {
	    return {
	        restrict: 'EA',
	        link: function (scope, element, attrs) {
	            scope.$watch(attrs.funeralDirectorExperience, function (value) {
	                var value = Number(attrs.funeralDirectorExperience);
	                var text = "Unknown";

	                // try to find a match
	                text = _.find($enum.funeralDirectorExperienceTypes, function (funeralDirectorExperienceType) { return funeralDirectorExperienceType.value === value; }).text;

	                // set the text value
	                element.text(text);
	            });
	        }
	    };
	}]);


angular.module('transportationFleetAge-directive', [])
	.directive('transportationFleetAge', ['_', '$enum', function (_, $enum) {
	    return {
	        restrict: 'EA',
	        link: function (scope, element, attrs) {
	            scope.$watch(attrs.transportationFleetAge, function (value) {
	                var value = Number(attrs.transportationFleetAge);
	                var text = "Unknown";

	                // try to find a match
	                text = _.find($enum.transportationFleetAgeTypes, function (transportationFleetAgeType) { return transportationFleetAgeType.value === value; }).text;

	                // set the text value
	                element.text(text);
	            });
	        }
	    };
	}]);


angular.module('requestReceiptState-directive', [])
	.directive('requestReceiptState', ['_', '$enum', function (_, $enum) {
	    return {
	        restrict: 'EA',
	        link: function (scope, element, attrs) {
	            scope.$watch(attrs.requestReceiptState, function (value) {
	                var value = Number(attrs.requestReceiptState);
	                var text = "Unknown";

	                // try to find a match
	                text = (value === $enum.requestReceiptStates.None.value ? $enum.requestReceiptStates.None.text : text);
	                text = (value === $enum.requestReceiptStates.Draft.value ? $enum.requestReceiptStates.Draft.text : text);
	                text = (value === $enum.requestReceiptStates.Pending.value ? $enum.requestReceiptStates.Pending.text : text);
	                text = (value === $enum.requestReceiptStates.Expired.value ? $enum.requestReceiptStates.Expired.text : text);
	                text = (value === $enum.requestReceiptStates.Completed.value ? $enum.requestReceiptStates.Completed.text : text);

	                // set the text value
	                element.text(text);
	            });
	        }
	    };
	}]);


angular.module('timeAgo-directive', [])
	.directive('timeAgo', ['_', 'moment', function (_, moment) {
		return {
			restrict: 'A',
			link: function (scope, element, attrs) {
				attrs.$observe("time-ago", function () {
					element.text(moment(attrs.timeAgo).fromNow());
				});
			}
		};
	}]);


angular.module('timeElapsed-directive', [])
	.directive('timeElapsed', ['_', 'moment', function (_, moment) {
		return {
			restrict: 'A',
			link: function (scope, element, attrs) {
				attrs.$observe("time-elapsed", function () {
					element.text(moment(attrs.timeElapsed).fromNow(true));
				});
			}
		};
	}]);


angular.module('timeDate-directive', [])
	.directive('timeDate', ['_', 'moment', function (_, moment) {
		return {
			restrict: 'A',
			link: function (scope, element, attrs) {
				attrs.$observe("time-date", function () {
					element.text(moment(attrs.timeDate).format("MMM Do YYYY"));
				});
			}
		};
	}]);



angular.module('profiler-directive', [])
	.directive('profile-selector', ['_', '$repo', '$enum', function (_, $repo, $enum) {
		return {
			restrict: 'EA',
			template:
				'<div class="row">' +
					'<div class="span3">' +
						'<select ng-model="profiler.key" ng-options="i for i in profiler.keys" ng-change="profiler.onKeyChanged()"></select>' +
					'</div>' +
				'</div>'
		};
	}]);


angular.module('invalidProperty-directive', [])
	.directive('invalidProperty', ['_', '$repo', '$enum', function (_, $repo, $enum) {
		return {
			restrict: 'EA',
			link: function (scope, element, attrs) {
				var property = attrs.property;
				var watched = (attrs.ngModel ? attrs.ngModel + '.errors' : 'status'); // this allows you to specify a model, like preference.errors or it defaults to the 'status' property on the controller

				scope.$watch(watched, function (results) {
					// clear the error text
					element.text('');


					if (results) {
						/*
						results are in the form:
						  results = {
						    errors: []
						    xxxxxx: {        // a nested property
						      errors: []
						    }
							id: 'abcdef123'
						  }
						*/
						var errors = _.clone(results);
						var propertyNames = property.split('.');

						if (property === 'id') {
							// ID is special because the status object has an 'id' property, but it is only the ID of the object sent to the server.
							// any ID errors will always be in the first errors array of the status object
							errors = _.clone(errors.errors);
						}
						else {
							// traverse the results data until we get to the desired property
							_.each(propertyNames, function (propertyName) {
								if (errors) {
									if (errors.hasOwnProperty(propertyName))
										errors = _.clone(errors[propertyName]);
									else
										errors = _.clone(errors.errors);
								}
							});
						}


						if (errors) {
							var searchProperty = _.last(propertyNames).toLowerCase();
							var error = _.find(errors, function (item) { return item.property.toLowerCase() === searchProperty; });
							if (error) {
								element.text(error.reason);
								element.attr('style', 'color:red;');
							}
						}
					}
				});
			}
		};
	}]);
