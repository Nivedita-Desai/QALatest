'use strict';

angular.module('cpp.services').
    factory('MPPFile', function ($resource) {
        return {

            Export: function (projectId, trendNumber, granularity) {
                //return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
                //    http://localhost:29986/api/Request/FundTypeByOrgID/79
                return $resource(serviceBasePath + "Request/Export/:projectId/:trendNumber/:granularity",
                    { projectId: '@projectId', trendNumber: "@trendNumber", granularity: "@granularity" });
            }
            //,
            //delByDocIDs: function () {
            //    //return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
            //    //    http://localhost:29986/api/Request/FundTypeByOrgID/79
            //    return $resource(serviceBasePath + "Register/Document/:docIDs");
            //}
            //,
            //getFundTypeAll: function () {
            //    return $resource(serviceBasePath + "request/FundType");
            //}
        };
    });