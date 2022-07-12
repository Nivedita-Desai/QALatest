'use strict';
// territoryCtrl
angular.module('cpp.services').
    factory('Location', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Location");
    }).
    factory('UpdateLocation', function ($resource) {
        return $resource(serviceBasePath + "Response/Location");
    });