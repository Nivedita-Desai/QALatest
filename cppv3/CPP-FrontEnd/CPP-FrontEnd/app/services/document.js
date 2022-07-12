'use strict';

angular.module('cpp.services').
    factory('Document', function ($resource) {
        return {

            getDocumentByProjID: function (DocumentSet, ProjectID) {
                //return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
                //    http://localhost:29986/api/Request/FundTypeByOrgID/79
                return $resource(serviceBasePath + "Request/Document/:DocumentSet/:ProjectID");
            },
            delByDocIDs: function () {
                //return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
                //    http://localhost:29986/api/Request/FundTypeByOrgID/79
                return $resource(serviceBasePath + "Register/Document/:docIDs");
            },

            getWarrantyByProgramId: function (programId) {
                return $resource(serviceBasePath + "contractWarranty/getContractWarranty/:programId");
            },

            getNoticeByProgramId: function (programId) {
                return $resource(serviceBasePath + "prelimnaryNotice/getPrelimnaryNotice/:programId");
            },

            getInsuranceByProgramId: function (programId) {
                return $resource(serviceBasePath + "contractInsurance/getContractInsurance/:programId");
            },

            //====================================== Created By Jignesh 28 - 10 - 2020 =======================================
            getModificationByProgramId: function (programId) {
                return $resource(serviceBasePath + "contractModification/getContractModificationData/:programId");
            },
            getModificationByModificationId: function (modificationId) {
                return $resource(serviceBasePath + "contractModification/getContractModificationData/null/:modificationId");
            }
            //============================================================================================================

            //,
            //getFundTypeAll: function () {
            //    return $resource(serviceBasePath + "request/FundType");
            //}
        };
    });
    //factory('Document', function ($resource) {
    //    console.log('test luan');
    //    return $resource(serviceBasePath + "Request/Document/:ProjectID");
    //}).
    //factory('UpdateDocument', function ($resource) {
    //    return $resource(serviceBasePath + "Response/Document/:ProjectID");
    //});