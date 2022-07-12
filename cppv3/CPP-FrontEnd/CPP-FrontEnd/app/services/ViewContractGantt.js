'use strict';

angular.module('cpp.services').
    factory('ViewContractGanttActivities', ['$resource', function ($resource) {
        return $resource(serviceBasePath + "Request/ContractActivities/0/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID");
        //return $resource("http://localhost:29986/api/Request/Activity/0/:ProgramID/:ProgramElementID/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID");
    }]).
    factory('ViewContractGanttPhase', function ($resource) {
        return $resource(serviceBasePath + "Request/ContractPhases/");
    });