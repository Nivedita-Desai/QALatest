'use strict';

angular.module('cpp.services').
    factory('Inventory', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Inventory");
    }).
    factory('UpdateInventory', function ($resource) {
        return $resource(serviceBasePath + "Response/Inventory");
    });