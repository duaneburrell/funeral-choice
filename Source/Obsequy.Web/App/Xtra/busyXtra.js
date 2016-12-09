
angular.module('busy-xtra', [])
	.service('$busy', ['$window', function ($window) {

		if (!!$window.busyXtra) {
			return $window.busyXtra;
		}

		var spinner = new Spinner({
			lines: 13,		// the number of lines to draw
			length: 29,		// the length of each line
			width: 14,		// the line thickness
			radius: 19,		// the radius of the inner circle
			corners: 0.8,	// corner roundness (0..1)
			rotate: 0,		// the rotation offset
			color: '#444',  // #rgb or #rrggbb
			trail: 82,		// afterglow percentage
			speed: 1.0,		// rounds per second
			shadow: false,	// whether to render a shadow
			hwaccel: false, // wether to use hardware acceleration
			top: 'auto',	// top position relative to parent in px
			left: 'auto'	// left position relative to parent in px
		}).spin();

		var show = function () {
			var target = document.getElementById('spinner');
			if (this.spinner) {
				this.spinner.spin();
				$(target).append(spinner.el);
				this.isBusy = true;
			}
		};

		var hide = function () {
			target = document.getElementById('spinner');
			if (this.spinner) {
				$(target).empty();
				this.spinner.stop();
				this.isBusy = false;
			}
		};

		var isBusy = false;

		$window.busyXtra = {

			spinner: spinner,
			show: show,
			hide: hide,
			isBusy: isBusy
		};

		return $window.busyXtra;
	}]);
