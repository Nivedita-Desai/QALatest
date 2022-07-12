'use strict';

angular.module('cpp.services').
    factory('Prime', function ($resource) {
        return $resource(serviceBasePath + "Request/prime");
    }).
    factory('UpdatePrime', function ($resource) {
        return $resource(serviceBasePath + "Response/prime");
    });

