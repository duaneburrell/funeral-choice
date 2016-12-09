

angular.module('response-model', [])
	.factory('Response', [function () {
 		var Response = function (properties) {

 			angular.extend(this, {

 				id: undefined,
 				state: undefined,
 				value: undefined,

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.id = properties.id;
 					this.state = properties.state;
 					this.value = properties.value;
				},

				toJSON: function () {
					return {
						id: this.id,
						state: this.state,
						value: this.value
					};
				}
			});

			this.update(properties);
		};

 		return Response;
	}]);