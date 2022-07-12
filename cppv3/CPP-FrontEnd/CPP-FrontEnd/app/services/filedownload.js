'use strict';
// territoryCtrl
angular.module('cpp.services').
    factory('Filedownload', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/Filedownload");
    }).
    factory('UpdateFiledownload', function ($resource) {
        return $resource(serviceBasePath + "Response/Filedownload");
    });