angular.module('cpp.controllers').
    controller('PhaseCodeModalCtrl',['$scope','$rootScope','$uibModal','$uibModalInstance','PhaseCode','$http',
        function($scope,$rootScope,$uibModal,$uibModalInstance,PhaseCode,$http){

            $scope.goBack = function(){
                $scope.$close();
            }
            if($scope.params.newOrEdit === 'new' && $scope.params){
                $scope.phaseCodeItem = null;

                $scope.saveChanges = function(){
                    var dataObj = {
                        "Operation":"1",
                        "PhaseID":$scope.phaseCodeItem.PhaseID,
                        "PhaseDescription":$scope.phaseCodeItem.PhaseDescription,
                        "Code":$scope.phaseCodeItem.Code
                    }
                    $http({
                        url: serviceBasePath+'response/phasecode',
                        //url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(dataObj),
                        headers: {'Content-Type': 'application/json'}
                    }).then(function(response){
                       if(response.data.result === 'Success'){
                           $scope.$close();
                       }
                       else
                       {
                           alert("Add new phase code failed");
                       }

                    });
                }
            }
            else if($scope.params.newOrEdit ==='edit' && $scope.params){
                $scope.phaseCodeItem = angular.copy($scope.params.phaseCodeItem);


                $scope.saveChanges = function(){
                    var dataObj = {
                        "Operation":"2",
                        "PhaseID":$scope.phaseCodeItem.PhaseID,
                        "PhaseDescription":$scope.phaseCodeItem.PhaseDescription,
                        "Code":$scope.phaseCodeItem.Code
                    }
                    $http({
                        url: serviceBasePath+'response/phasecode',
                       // url: 'http://localhost:29986/api/response/phasecode',
                        method: "POST",
                        data: JSON.stringify(dataObj),
                        headers: {'Content-Type': 'application/json'}
                    }).then(function(response){
                        if(response.data.result === 'Success'){
                          $scope.$close();
                        }
                        else
                        {
                            alert("Edit phaseCode failed");
                        }
                    });
                }
                }
        }]);