
angular.module('requestReceipt-model', [])
	.factory('RequestReceipt', ['moment', function (moment) {
 		var RequestReceipt = function (properties) {

 			angular.extend(this, {

 				by: undefined,
 				on: undefined,
 				state: undefined,
 				mnemonic: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.by = properties.by;
 					this.on = properties.on;
 					this.state = properties.state;
 					this.mnemonic = properties.mnemonic;

 					// UI filtering
 					this._filter = moment(properties.on).format('L');
				},

				toJSON: function () {
					return {
						by: this.by,
						on: this.on,
						state: this.state
					};
				}
			});

			this.update(properties);
		};

 		return RequestReceipt;
	}]);