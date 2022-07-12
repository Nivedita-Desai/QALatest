'use strict';

angular.module('cpp.services').
    factory('ProjectElementNumber', function ($resource) {

        return $resource(serviceBasePath + "Request/ProjectElementNumber/:ProjectNumber/:OrganizationID", { ProjectNumber: '@ProjectNumber', OrganizationID: '@OrganizationID' });
    });