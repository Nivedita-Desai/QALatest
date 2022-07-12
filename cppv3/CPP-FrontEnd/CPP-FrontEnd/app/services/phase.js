'use strict';

angular.module('cpp.services').
    factory('PhaseCode', function ($resource) {
        return $resource(serviceBasePath+"Request/PhaseCode");
    }).
    factory('Phase', ['$resource', function ($resource) {
       return $resource(serviceBasePath+"Request/Phase/:ProjectID/:TrendNumber");
    }]);