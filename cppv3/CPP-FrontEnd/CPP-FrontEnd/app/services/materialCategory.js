'use strict';

angular.module('cpp.services').
    factory('MaterialCategory', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/MaterialCategory");
    }).
    factory('UpdateMaterialCategory', function ($resource) {
        return $resource(serviceBasePath + "Response/MaterialCategory");
    });