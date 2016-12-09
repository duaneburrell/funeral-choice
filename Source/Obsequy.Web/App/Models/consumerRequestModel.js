

angular.module('consumerRequest-model', [])
 .factory('ConsumerRequest', ['_', 'RequestReceipt', function (_, RequestReceipt) {
 	var ConsumerRequest = function (properties) {

 		angular.extend(this, {

 			state: undefined,
 			current: new RequestReceipt(),
 			receipts: [],
 			mnemonic: undefined,

 			update: function (properties) {
 				properties = properties || {};

				// update properties
 				this.state = properties.state;
 				this.current.update(properties.current);
 				this.mnemonic = properties.mnemonic;
				
 				this.receipts = [];
 				_.each(properties.receipts, function (item) {
 					this.receipts.push(new RequestReceipt(item));
 				}, this);
			},

			toJSON: function () {
				return {
				    state: this.state,
				    current: this.current.toJSON(),
				    receipts: this.receipts,
				    mnemonic: this.mnemonic
				};
			}
		});

		this.update(properties);
	};

 	return ConsumerRequest;
}]);