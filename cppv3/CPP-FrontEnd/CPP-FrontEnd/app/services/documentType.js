'use strict';

angular.module('cpp.services').
    factory('DocumentType', function ($resource) {
        console.log('test luan');
        return $resource(serviceBasePath + "Request/DocumentType");
    }).
    factory('UpdateDocumentType', function ($resource) {
        return $resource(serviceBasePath + "Response/DocumentType");
    });