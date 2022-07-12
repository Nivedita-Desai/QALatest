'use strict';

angular.module('cpp.services').
    factory('ProjectNumber', function ($resource) {

        return $resource(serviceBasePath + "Request/ProjectNumber/:OrganizationID", {OrganizationID: '@OrganizationID' });
    });