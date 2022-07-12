'use strict';

angular.module('cpp.services').
   factory('PurchaseOrder', ['$resource', function ($resource) {
       return {
           getNewPurchaseOrderNumber: function () {
               return $resource(serviceBasePath + "Request/RequestNewPurchaseOrderNumber/:ProjectID");
           },
           //persist: function () {
           //    return $resource(serviceBasePath + "Response/PurchaseOrderDetail");
           //}
       };
   }
   ]);