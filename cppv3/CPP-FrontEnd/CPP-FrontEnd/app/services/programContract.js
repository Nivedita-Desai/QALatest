'use strict';

angular.module('cpp.services').
    factory('ProgramContract', function ($resource) {
    	console.log('test luan');
    	return $resource(serviceBasePath + "Request/ProgramContract");
    }).
    factory('UpdateProgramContract', function ($resource) {
    	return $resource(serviceBasePath + "Response/ProgramContract");
    });