'use strict';

angular.module('cpp.services').
    factory('UniqueIdentityNumber', function ($resource) {
        return $resource(serviceBasePath + "Request/NextUniqueIdentityNumber/:NumberType/:OrganizationID/:PhaseID/:CategoryID");
    })