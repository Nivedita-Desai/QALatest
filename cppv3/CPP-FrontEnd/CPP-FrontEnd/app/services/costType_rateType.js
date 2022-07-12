'use strict';

angular.module('cpp.services').
    factory('CostTypeRateType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/CostTypeRateType");
    }).
    factory('UpdateCostTypeRateType', function ($resource) {
        return $resource(serviceBasePath + "Response/CostTypeRateType");
    });