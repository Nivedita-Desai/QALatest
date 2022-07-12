'use strict';

angular.module('cpp.services').
    factory('ServiceToSubserviceMapping', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/ServiceToSubserviceMapping");
    }).
    factory('UpdateServiceToSubserviceMapping', function ($resource) {
        return $resource(serviceBasePath + "Response/ServiceToSubserviceMapping");
    });