'use strict';

angular.module('cpp.services').
    factory('Activity', ['$resource', function ($resource) {
       return $resource(serviceBasePath+"Request/Activity/0/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID");
        //return $resource("http://localhost:29986/api/Request/Activity/0/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID");
    }]).
    factory('GetActivity',['$resource',function($resource){
        return $resource(serviceBasePath + "Request/RequestActivityByID/:ID");
    }]).
    factory('UpdateActivity',['$resource',function($resource){
        return $resource(serviceBasePath+"Response/Activity");
    }]);