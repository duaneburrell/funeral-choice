

angular.module('profiler-xtra', ['app.config'])
	.factory('Profiler', ['_', '$rootScope', '$profiles', 'GENERAL_CONFIG', function (_, $rootScope, $profiles, GENERAL_CONFIG) {
		var Profiler = function ($scope, path, entity) {

 			angular.extend(this, {

 				items: [],
 				item: undefined,
 				keys: [],
				key: undefined,

				isVisible: GENERAL_CONFIG.SHOW_TEST_DATA,

 				init: function () {
 					// find our profiles by parsing the path as 'consumer.registration', etc.
 					var profiles = $profiles;
 					var paths = path.split('.');

 					// travers the $profiles data until we get to our desired profile data. 
					// loop 1 would be consumer, loop 2 would be registration, and profiles would be set to that array of data
 					_.each(paths, function (item) {
 						profiles = profiles[item];
 					});
 					
					// push each profile onto our list
 					_.each(profiles, function (profile) {
 						this.items.push(profile);
 						this.keys.push(profile.key);
 					}, this);
 				},

 				onKeyChanged: function () {
 					this.item = _.find(this.items, function (item) { return item.key === this.key; }, this);

 					if (entity.update)
 						entity.update(this.item);

 					// broadcast update event
 					$rootScope.$broadcast('profileChanged', this.data);
 				}
			});

 			this.init(path, entity);
		};

 		return Profiler;
}]);