'use strict';

angular.module('cpp.services').
    factory('Client', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Client");
    }).
    factory('UpdateClient', function ($resource) {
        return $resource(serviceBasePath + "Response/Client");
    });