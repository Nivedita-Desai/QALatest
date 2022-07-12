'use strict';

angular.module('cpp.services').
    factory('CertifiedPayroll', function ($resource) {
        return $resource(serviceBasePath + "Request/CertifiedPayroll");
    }).
    factory('UpdateCertifiedPayroll', function ($resource) {
        return $resource(serviceBasePath + "Response/CertifiedPayroll");
    });

