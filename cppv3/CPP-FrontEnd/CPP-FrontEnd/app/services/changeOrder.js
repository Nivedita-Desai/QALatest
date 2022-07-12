'use strict';

angular.module('cpp.services').
    factory('ChangeOrder', function ($resource) {
    	console.log('test luan');
    	return $resource(serviceBasePath + "Request/ChangeOrder");
    }).
    factory('UpdateChangeOrder', function ($resource) {
    	return $resource(serviceBasePath + "Response/ChangeOrder");
    });