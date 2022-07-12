'use strict';

angular.module('cpp.services').
    factory('LineOfBusiness', function ($resource) {
        return $resource(serviceBasePath + "Request/LineOfBusiness");
    });