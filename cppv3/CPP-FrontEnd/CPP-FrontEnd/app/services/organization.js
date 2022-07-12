'use strict';

angular.module('cpp.services').

    factory('Organization', ['$resource', function ($resource) {
        return {
            lookup: function () {
                return $resource(serviceBasePath + "Request/Organization/");
            },
            persist: function () {
                return $resource(serviceBasePath + "Response/Organization/");
            }
        };
}]);