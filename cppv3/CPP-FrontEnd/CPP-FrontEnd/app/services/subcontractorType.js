'use strict';

angular.module('cpp.services').
    factory('SubcontractorType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/SubcontractorType");
    }).
    factory('UpdateSubcontractorType', function ($resource) {
        return $resource(serviceBasePath + "Response/SubcontractorType");
    });