'use strict';

angular.module('cpp.services').
    factory('ViewGanttCost', function ($resource) {
        //return $resource(serviceBasePath + "Request/Cost/:ProjectID/:TrendNumber/:Phasecode/:ActivityID/:Granularity/:ViewLabor");
        //return $resource(serviceBasePath + "Request/Cost/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID/:Granularity/:BudgetID/:BudgetCategory/:BudgetSubCategory/:ViewLabor");
        return $resource(serviceBasePath + "Request/ProjectCost/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID/:Granularity/:BudgetID/:ViewLabor/:ProgramElementID");
        // return $resource("http://localhost:29986/api/Request/Cost/:ProjectID/:TrendNumber/:Phasecode/:ActivityID");
    });