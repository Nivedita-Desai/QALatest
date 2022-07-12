angular.module('cpp.controllers').
    controller('PositionModalCtrl', ['$scope', '$rootScope', '$uibModal', 'UpdatePosition', '$http',
        function ($scope, $rootScope, $uibModal, UpdatePosition, $http) {

        $scope.goBack = function () {
            $scope.$close();
        }

            if ($scope.params.newOrEdit === "new" && $scope.params) {
                var url = serviceBasePath + 'response/FTEPosition/';
                //  var url = 'http://localhost:29986/api/response/FTEPosition/';
                $scope.saveChanges = function () {
                    /*   UpdatePosition.get({
                           Operation: 1,
                           PositionID:$scope.positionItem.PositionID,
                           PositionDescription: $scope.positionItem.PositionDescription,
                           MinHourlyRate: $scope.positionItem.MinHourlyRate,
                           MaxHourlyRate: $scope.positionItem.MaxHourlyRate,
                           CurrentHourlyRate: $scope.positionItem.CurrentHourlyRate
                       },function(response) {
                           if(response.result ==="Success"){
                           }else{
                           }
       
                       });*/
                    console.log('at new');
                    console.log(dataObj);
                    var dataObj = {
                        "Operation": "1",
                        "PositionID": $scope.positionItem.PositionID,
                        "PositionDescription": $scope.positionItem.PositionDescription,
                        "MinHourlyRate": $scope.positionItem.MinHourlyRate,
                        "MaxHourlyRate": $scope.positionItem.MaxHourlyRate,
                        "CurrentHourlyRate": $scope.positionItem.CurrentHourlyRate
                    }

                    $http.post(url, dataObj).then(function (response) {
                        if (response.data.result === "Success") {
                        } else {
                            alert("Failed to add new Position");
                        }
                    })
                }

            }
            else if($scope.params.newOrEdit === "edit" && $scope.params) {
           var url = serviceBasePath+'response/FTEPosition/';
          //  var url = 'http://localhost:29986/api/response/FTEPosition/';
            $scope.positionItem= angular.copy($scope.params.positionItem);


            $scope.saveChanges = function(){
                //UpdatePosition.get({Operation:2,
                //PositionID:$scope.positionItem.PositionID,
                //    PositionDescription:$scope.positionItem.PositionDescription,
                //    MinHourlyRate:$scope.positionItem.MinHourlyRate,
                //    MaxHourlyRate:$scope.positionItem.MaxHourlyRate,
                //    CurrentHourlyRate:$scope.positionItem.CurrentHourlyRate
                //    },function(response){
                //
                //    if(response.result === "Success") {
                //    }
                //    else
                //    {
                //    }
                //});
                console.log('at edit');
                console.log(dataObj);
                var dataObj =
                {
                    "Operation": "2",
                    "PositionID": $scope.positionItem.PositionID,
                    "PositionDescription": $scope.positionItem.PositionDescription,
                    "MinHourlyRate": $scope.positionItem.MinHourlyRate,
                    "MaxHourlyRate": $scope.positionItem.MaxHourlyRate,
                    "CurrentHourlyRate": $scope.positionItem.CurrentHourlyRate
                }

                $http.post(url,dataObj).then(function(response){
                    if(response.data.result === 'Success'){

                        //alert('Edit Succesfully');
                    }
                    else
                    {
                        alert('Edit failed');
                    }
                })
            }

        }

    }]);