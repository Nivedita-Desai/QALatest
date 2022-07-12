'use strict';

angular.module('cpp.services').
    factory('ProjectType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/ProjectType");
    }).
    factory('UpdateProjectType', function ($resource) {
        return $resource(serviceBasePath + "Response/ProjectType");
    });