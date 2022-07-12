'use strict';

angular.module('cpp.services').
    factory('InsertCost', function ($resource) {
       return $resource(serviceBasePath+"Response/Cost");
        //return $resource("http://localhost:29986/api/Response/Cost");
    }).
    factory('Cost', function ($resource) {
        //return $resource(serviceBasePath + "Request/Cost/:ProjectID/:TrendNumber/:Phasecode/:ActivityID/:Granularity/:ViewLabor");
        //return $resource(serviceBasePath + "Request/Cost/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID/:Granularity/:BudgetID/:BudgetCategory/:BudgetSubCategory/:ViewLabor");
        return $resource(serviceBasePath + "Request/Cost/:ProjectID/:TrendNumber/:PhaseCode/:ActivityID/:Granularity/:ViewLabor/:BudgetID");
      // return $resource("http://localhost:29986/api/Request/Cost/:ProjectID/:TrendNumber/:Phasecode/:ActivityID");
    }).
    factory('FTEPositionCost', ['$resource', function ($resource) {
        return $resource(serviceBasePath+"Request/FTEPosition/:Id");
       //return $resource("http://localhost:29986/api/FTEPosition/:PositionID");
    }]).
    factory('LaborRate', ['$resource', function ($resource) {
        console.log($resource);
        return $resource(serviceBasePath + "Request/LaborRate/:TrendID/:PositionID/:EmployeeID");
    }]);