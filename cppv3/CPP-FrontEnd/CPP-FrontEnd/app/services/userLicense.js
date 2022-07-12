'use strict';

angular.module('cpp.services').
    factory('UserLicenseKey', ['$resource', function ($resource) {
        return {
            getLicenseForUser: function () {
                return $resource(serviceBasePath + "Request/RequestUserLicenseMapping/:userName");
            },
            persist: function () {
                return $resource(serviceBasePath + "Response/RegisterUserLicenseMapping");
            }
        };
    }
    ]);