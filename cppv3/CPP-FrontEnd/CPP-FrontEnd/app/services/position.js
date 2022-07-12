'use strict';

angular.module('cpp.services').
    factory('UpdatePosition',function($resource){
      return $resource(serviceBasePath+"response/FTEPosition/:Operation/:PositionID/:PositionDescription/:MinHourlyRate" +
      "/:MaxHourlyRate/:CurrentHourlyRate")
          //return $resource("http://localhost:29986/api/response/FTEPosition/:Operation/:PositionID/:PositionDescription/:MinHourlyRate" +
        //"/:MaxHourlyRate/:CurrentHourlyRate")
    });