/**
 * Created by ikhong on 11/17/2015.
 */
/**
 * Created by ikhong on 8/14/2015.
 */
angular.module('cpp.controllers').
    //Confirmation Controller
    controller('alertConfirmation',['$scope','$uibModal','$rootScope',
        function($scope,$uibModal,$rootScope){
            $scope.message = $scope.params.message;
            console.log($scope.message);
            $scope.back = function(){
                $scope.$close();
                $scope.params.confirm = 'back';
            }
        }]);