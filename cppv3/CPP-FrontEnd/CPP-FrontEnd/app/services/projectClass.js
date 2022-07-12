'use strict';

angular.module('cpp.services').
    factory('ProjectClass', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/ProjectClass");
    }).
    factory('ProjectClassByProgramId', function ($resource) {
        return $resource(serviceBasePath + "Request/ProjectClassByProgramId/:programID", { programID: "@programID" });
    }).
    factory('ProjectClassByProgramElementId', function ($resource) {
        return $resource(serviceBasePath + "Request/ProjectClassByProgramElemId/:programElemID", { programElemID: "@programElemID" });
    }).
    factory('UpdateProjectClass', function ($resource) {
        return $resource(serviceBasePath + "Response/ProjectClass");
    });
   