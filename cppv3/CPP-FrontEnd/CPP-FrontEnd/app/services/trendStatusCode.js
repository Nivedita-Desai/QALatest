'use strict';

angular.module('cpp.services').
    factory('TrendStatusCode', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/TrendStatusCode");
    }).
    factory('UpdateTrendStatusCode', function ($resource) {
        return $resource(serviceBasePath + "Response/TrendStatusCode");
    });