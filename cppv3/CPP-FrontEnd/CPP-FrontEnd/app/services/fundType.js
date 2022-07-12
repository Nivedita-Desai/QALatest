'use strict';

angular.module('cpp.services').
    factory('FundType', function ($resource) {
        return {

            getFundTypeByOrgID: function (orgID) {
                //return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
                //    http://localhost:29986/api/Request/FundTypeByOrgID/79
                return $resource(serviceBasePath + "Request/FundTypeByOrgID/:OrganizationID");
            },
            getFundTypeAll: function () {
                return $resource(serviceBasePath + "request/FundType");
            }
        };
    });