'use strict';

angular.module('cpp.services').
    factory('Milestone', function ($resource) {
    	console.log('test luan');
    	return $resource(serviceBasePath + "Request/Milestone");
    }).
    factory('UpdateMilestone', function ($resource) {
    	return $resource(serviceBasePath + "Response/Milestone");
    });