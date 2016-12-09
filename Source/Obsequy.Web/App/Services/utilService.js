
angular.module('util-service', [])
 .service('$util', ['_', '$window', '$repo', '$enum', function (_, $window, $repo, $enum) {

 	if (!!$window.utilService) {
 		return $window.utilService;
 	}

 	$window.utilService = {
 	};

 	return $window.utilService;
 }]);
