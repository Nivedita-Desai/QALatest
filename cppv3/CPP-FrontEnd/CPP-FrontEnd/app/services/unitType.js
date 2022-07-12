'use strict';

angular.module('cpp.services').
    factory('UnitType', function ($resource) {
        return $resource(serviceBasePath + "Request/UnitType");
    }).
    factory('UpdateUnitType',function($resource){
        return $resource(serviceBasePath + "Response/UnitType");
    });