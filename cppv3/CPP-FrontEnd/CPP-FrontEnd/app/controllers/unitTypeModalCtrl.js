angular.module('cpp.controllers').
    controller('UnitTypeModalCtrl',['UpdateUnitType','$scope','$rootScope',
    function(UpdateUnitType,$scope, $rootScope){
        $scope.goBack = function(){
            $scope.$close();
        }

        if($scope.params.newOrEdit === "new" && $scope.params){
            $scope.unitItem = angular.copy($scope.params.selectedUnit);
            $scope.saveChanges = function() {
                    UpdateUnitType.save({
                        "Operation": 1,
                        "UnitID": $scope.unitItem.UnitId,
                        "UnitName": $scope.unitItem.UnitName,
                        "UnitAbbr": $scope.unitItem.UnitAbbr
                    }, function (response) {
                        console.log(response);
                        $scope.$close();
                    });
            }
        }else if($scope.params.newOrEdit == "edit" && $scope.params){
            $scope.unitItem = angular.copy( $scope.params.selectedUnit);
            $scope.saveChanges = function(){
                UpdateUnitType.save({
                    "Operation" : 2,
                    "UnitID": $scope.unitItem.UnitId,
                    "UnitName" : $scope.unitItem.UnitName,
                    "UnitAbbr": $scope.unitItem.UnitAbbr
                },function(response){
                    console.log(response);
                    $scope.$close();
                });

            }
        }
    }]);
