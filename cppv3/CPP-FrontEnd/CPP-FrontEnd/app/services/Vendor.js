'use strict';

angular.module('cpp.services').
    factory('Vendor', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Vendor");
    }).
    factory('UpdateVendor', function ($resource) {
        return $resource(serviceBasePath + "Response/Vendor");
    });