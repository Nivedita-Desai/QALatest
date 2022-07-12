'use strict';

angular.module('cpp.services').
    factory('ViewProjectGanttActivities', ['$resource', function ($resource) {
        return $resource(serviceBasePath + "Request/Activities/0/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID");
        //return $resource("http://localhost:29986/api/Request/Activity/0/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID");
    }]).
    factory('ViewProjectGanttPhase', function ($resource) {
        return $resource(serviceBasePath + "Request/Phases/");
    });