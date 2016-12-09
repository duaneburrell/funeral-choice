

angular.module('agreementResponseStateIcon-directive', [])
	.directive('agreementResponseStateIcon', ['_', '$repo', '$enum', '$compile', '$http', function (_, $repo, $enum, $compile, $http) {
		return {
			restrict: 'EA',
			replace: true,
			scope: true,
			link: function (scope, element, attrs) {
				var response = scope.$eval(attrs.ngModel);
				var state = response.current.state;
				var iconSrc = '';

				if (state === $enum.responseReceiptStates.Available.value)
					iconSrc = 'alert-info-ico.png';

				if (state === $enum.responseReceiptStates.Pending.value)
					iconSrc = 'alert-notice-ico.png';

				if (state === $enum.responseReceiptStates.Accepted.value)
					iconSrc = 'alert-success-ico.png';

				if (state === $enum.responseReceiptStates.Rejected.value)
					iconSrc = 'alert-error-ico.png';

				if (state === $enum.responseReceiptStates.Dismissed.value)
					iconSrc = 'alert-error-ico.png';

				if (state === $enum.responseReceiptStates.Expired.value)
					iconSrc = 'alert-error-ico.png';


				// set icon and compile
				element.css({ 'height': '16px', 'width': '16px' });
				element.attr('src', "/Content/images/" + iconSrc);
				$compile(element.contents())(scope);
			},
		};
	}]);
