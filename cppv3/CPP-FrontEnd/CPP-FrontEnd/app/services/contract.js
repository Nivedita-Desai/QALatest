'use strict';

angular.module('cpp.services').
    factory('Contract', function ($resource) {
    	console.log('test luan');
    	return $resource(serviceBasePath + "Request/Contract");
    }).
    factory('UpdateContract', function ($resource) {
    	return $resource(serviceBasePath + "Response/Contract");
    });