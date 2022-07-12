angular.module('cpp.controllers').        
    controller('TrendStatusModalCtrl',['$scope','$rootScope','$uibModal',function($scope,$rootScope,$uibModal) {
        $scope.statusItem= angular.copy($scope.params.statusItem);
        $scope.goBack = function(){
            $scope.$close();
        }
        $scope.saveChanges = function(){
            $scope.params.statusItem.StatusID= $scope.statusItem.StatusID;
            $scope.params.statusItem.StatusDescription= $scope.statusItem.StatusDescription;
            $scope.$close();
        }

    }]);