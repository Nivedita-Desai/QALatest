'use strict';
// agilegridCtrl
angular.module('cpp.services').
    factory('Agilegrid', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Agilegrid");
    }).
    factory('UpdateAgilegrid', function ($resource) {
        return $resource(serviceBasePath + "Response/Agilegrid");
    });