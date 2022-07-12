'use strict';
angular.module('cpp.services').
    factory('Whitelist', function ($resource) {
        return $resource(serviceBasePath + "Request/Whitelist");
    }).
    factory('UpdateWhitelist', function ($resource) {
        return $resource(serviceBasePath + "Response/Whitelist");
    });