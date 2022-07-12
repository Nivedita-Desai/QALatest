'use strict';

angular.module('cpp.services').
    factory('LobPhase', function ($resource) { // Line Of Business
        return $resource(serviceBasePath + "Request/lobphase/:lobID");
    });