'use strict';


angular.module('cpp.services').
     factory('TrendCostOverhead', ['$resource', function ($resource) {
         return $resource(serviceBasePath + "Request/TrendCostOverhead/:ProjectID/:TrendNumber");
     }]);
 
    