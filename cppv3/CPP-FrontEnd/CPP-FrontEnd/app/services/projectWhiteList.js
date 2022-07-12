'use strict';

angular.module('cpp.services').
    factory('ProjectWhiteList', function ($resource) {
    	console.log('test luan');
    	return $resource(serviceBasePath + "Request/ProjectWhiteList");
    }).
    factory('UpdateProjectWhiteList', function ($resource) {
    	return $resource(serviceBasePath + "Response/ProjectWhiteList");
    });