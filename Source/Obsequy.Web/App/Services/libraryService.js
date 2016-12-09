
angular.module('moment', [])
 .factory('moment', [function () {

 	// moment must already be loaded on the page 
 	return window.moment;
 }]);


angular.module('underscore', [])
 .factory('_', [function () {

 	// underscore must already be loaded on the page 
 	return window._;
 }]);


angular.module('braintree', [])
 .factory('braintree', [function () {

 	// braintree must already be loaded on the page 
 	return window.Braintree;
 }]);

