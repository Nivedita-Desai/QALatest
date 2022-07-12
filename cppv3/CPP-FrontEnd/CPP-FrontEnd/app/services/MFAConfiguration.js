'use strict';

angular.module('cpp.services').
    factory('MFAConfiguration', function ($resource) {
        return {
            lookup: function () {
                return $resource(serviceBasePath + "Request/MFAConfiguration");
            },
            persist: function () {
                return $resource(serviceBasePath + "Response/MFAConfiguration");
            }
        }
    });
