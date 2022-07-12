'use strict';

angular.module('cpp.services').
    factory('Subcontractor', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Subcontractor");
    }).
    factory('UpdateSubcontractor', function ($resource) {
        return $resource(serviceBasePath + "Response/Subcontractor");
    });