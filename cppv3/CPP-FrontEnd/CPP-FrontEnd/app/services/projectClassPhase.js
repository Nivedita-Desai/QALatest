'use strict';

angular.module('cpp.services').
    factory('ProjectClassPhase', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/ProjectClassPhase");
    }).
    factory('UpdateProjectClassPhase', function ($resource) {
        return $resource(serviceBasePath + "Response/ProjectClassPhase");
    });