'use strict';

angular.module('cpp.services').
    factory('Manufacturer', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Manufacturer");
    }).
    factory('UpdateManufacturer', function ($resource) {
        return $resource(serviceBasePath + "Response/Manufacturer");
    });