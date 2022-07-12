'use strict';

angular.module('cpp.services').
    factory('Employee', function ($resource) {
        return $resource(serviceBasePath + "Request/Employee/:OrganizationID");
    }).
    factory('AllEmployee', function ($resource) {
        return $resource(serviceBasePath + "Request/AllEmployee");
    });