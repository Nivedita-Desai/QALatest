'use strict';

angular.module('cpp.services').
    factory('Wrap', function ($resource) {
        return $resource(serviceBasePath + "Request/Wrap");
    }).
    factory('UpdateWrap', function ($resource) {
        return $resource(serviceBasePath + "Response/Wrap");
    });