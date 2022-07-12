'use strict';

angular.module('cpp.services').
    factory('ServiceClass', function ($resource) {
        return $resource(serviceBasePath + "Request/ServiceClass");
    }).
    factory('UpdateServiceClass', function ($resource) {
        return $resource(serviceBasePath + "Response/ServiceClass");
    });

