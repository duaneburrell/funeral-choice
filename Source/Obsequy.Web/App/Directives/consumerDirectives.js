
angular.module('requestSummary-directive', [])
	.directive('requestSummary', ['_', '$repo', '$enum', function (_, $repo, $enum) {
		return {
			restrict: 'EA',
			template: '<a href="#/quotes" style="margin-left: 20px; font-size: 1em;">{{ data.portfolio.requestSummary }}</a>',
		};
	}]);


