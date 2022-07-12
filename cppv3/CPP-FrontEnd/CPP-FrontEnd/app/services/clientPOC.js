'use strict';

angular.module('cpp.services').
    factory('ClientPOC', function ($resource) {                      //Tanmay - 15/12/2021
        console.log('test luan');
        return $resource(serviceBasePath + "Request/ClientPOC");    //Tanmay
    }).
    factory('UpdateClientPOC', function ($resource) {
        return $resource(serviceBasePath + "Response/ClientPOC");   //Tanmay
    });
