'use strict';

angular.module('cpp.services').
   factory('CostRow', ['$resource', function ($resource) {
       return {
           lookup: function () {
               return $resource(serviceBasePath + "Request/CostRow/:ActivityID/:Granularity/:LineID/:CostType");
           },
           persist: function () {
               return $resource(serviceBasePath + "Response/CostRow/");
           }
       };
   }
  ]);