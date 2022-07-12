'use strict';

angular.module('cpp.services').
    factory('ApprovalMatrix',['$q','$resource',function($q,$resource){
        return $resource(serviceBasePath+"request/approvalmatrix");
    }]).
    factory('UpdateApprovalMatrix',['$q','$http',function($q,$http){
        var url = serviceBasePath+"response/approvalMatrix";
        var approvalMatrixObj = {};
        var deferred = $q.defer();
        var _addNew = function(obj){
            console.log(obj);
            $http({
                url: url,
                method: "POST",
                data: JSON.stringify(obj),
                headers: { 'Content-Type': 'application/json' }
            }
            ).then(function success(response) {
                console.log(response);
                deferred.resolve(response.data);
            }, function error(response) {

            });

             
            return deferred.promise;
        }


        approvalMatrixObj.addNew = _addNew;
        return approvalMatrixObj;
    }]);