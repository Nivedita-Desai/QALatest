'use strict';

angular.module('cpp.services').

    factory("VersionDetails", function ($resource) {
        return {
            lookup: function () {
                return $resource(serviceBasePath + "Request/VersionDetails/:operation/:programElementID/:organizationID");
            }
        };
    });