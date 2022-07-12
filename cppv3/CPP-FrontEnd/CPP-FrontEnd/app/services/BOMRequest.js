'use strict';

angular.module('cpp.services').
    factory('BOMRequest', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/BOMRequest");
    }).
    factory('UpdateBOMRequest', function ($resource) {
        return $resource(serviceBasePath + "Response/BOMRequest");
    });