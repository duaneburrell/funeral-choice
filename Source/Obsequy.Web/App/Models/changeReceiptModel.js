

angular.module('changeReceipt-model', [])
 .factory('ChangeReceipt', ['moment', function (moment) {
 	var ChangeReceipt = function (properties) {

 		angular.extend(this, {

 			by: undefined,
 			on: undefined,

 			update: function (properties) {
 				properties = properties || {};

				// update properties
 				this.by = properties.by;
 				this.on = properties.on;

 				// UI filtering
 				this._filter = moment(properties.on).format('L');
			},

			toJSON: function () {
				return {
					by: this.by,
					on: this.on
				};
			}
		});

		this.update(properties);
	};

 	return ChangeReceipt;
}]);