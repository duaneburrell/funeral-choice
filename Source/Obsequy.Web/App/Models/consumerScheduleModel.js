

angular.module('consumerSchedule-model', [])
 .factory('ConsumerSchedule', [function () {
 	var ConsumerSchedule = function (properties) {

 		angular.extend(this, {

 			scheduleChoiceType: undefined,
 			wakeDate: undefined,
 			ceremonyDate: undefined,

 			anyScheduleSelections: undefined,

 			update: function (properties) {
 				properties = properties || {};

				// update properties
 				this.scheduleChoiceType = properties.scheduleChoiceType;
 				this.wakeDate = properties.wakeDate;
 				this.ceremonyDate = properties.ceremonyDate;
 				this.anyScheduleSelections = properties.anyScheduleSelections;

 				// UI filtering
 				this._filterWakeDate = moment(properties.wakeDate).format('L');
 				this._filterCeremonyDate = moment(properties.ceremonyDate).format('L');
			},

			toJSON: function () {
				return {
					scheduleChoiceType: this.scheduleChoiceType,
					wakeDate: this.wakeDate,
					ceremonyDate: this.ceremonyDate,
					anyScheduleSelections: this.anyScheduleSelections
				};
			}
		});

		this.update(properties);
	};

 	return ConsumerSchedule;
}]);