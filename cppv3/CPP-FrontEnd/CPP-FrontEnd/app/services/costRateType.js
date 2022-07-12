'use strict';

angular.module('cpp.services').
    factory('CostRateType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/CostRateType");
    }).
    factory('UpdateCostRateType', function ($resource) {
        return $resource(serviceBasePath + "Response/CostRateType");
    });