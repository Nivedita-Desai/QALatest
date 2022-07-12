'use strict';

angular.module('cpp.services').
   factory('PurchaseOrderDetail', ['$resource', function ($resource) {
       return {
           getPurchaseOrderDetail: function () {
               return $resource(serviceBasePath + "Request/RequestPurchaseOrderDetail/:ProjectID");
           },
           persist: function () {
               return $resource(serviceBasePath + "Response/PurchaseOrderDetail");
           },
           getPOList: function () {
               return $resource(serviceBasePath + "Request/RequestPOList/:ProjectID");
           },
           getPurchaseOrderIDDetail: function () {
               return $resource(serviceBasePath + "Request/RequestPurchaseOrderIDDetail/:PurchaseOrderID");
           }
       };
   }
   ]);