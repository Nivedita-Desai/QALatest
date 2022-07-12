'use strict';

angular.module('cpp.services').
    factory('ODCType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/ODCType");
    }).
    factory('UpdateODCType', function ($resource) {
        return $resource(serviceBasePath + "Response/ODCType");
    });