
angular.module('responseReceipt-model', [])
	.factory('ResponseReceipt', ['moment', function (moment) {
 		var ResponseReceipt = function (properties) {

 			angular.extend(this, {

 				by: undefined,
 				on: undefined,
 				state: undefined,
 				value: undefined,
 				mnemonic: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.by = properties.by;
 					this.on = properties.on;
 					this.state = properties.state;
 					this.value = properties.value;
 					this.mnemonic = properties.mnemonic;

 					// UI filtering
 					this._filter = moment(properties.on).format('L');
				},

				toJSON: function () {
					return {
						by: this.by,
						on: this.on,
						state: this.state,
						value: this.value
					};
				}
			});

			this.update(properties);
		};

 		return ResponseReceipt;
	}]);