﻿angular.module('app.config', [])
{
    var config_data = {
        'GENERAL_CONFIG': {
            'SHOW_TEST_DATA': 'false',
        }
    }
    angular.forEach(config_data, function (key, value) {
        angular.module('app.config').constant(value, key);
    })
}
