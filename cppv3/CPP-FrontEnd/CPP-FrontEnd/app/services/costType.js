'use strict';

angular.module('cpp.services').
    factory('CostType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/CostType");
    }).
    factory('UpdateCostType', function ($resource) {
        return $resource(serviceBasePath + "Response/CostType");
    });