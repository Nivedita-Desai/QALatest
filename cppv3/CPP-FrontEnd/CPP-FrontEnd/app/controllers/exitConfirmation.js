/**
 * Created by ikhong on 8/14/2015.
 */
angular.module('cpp.controllers').
    //Confirmation Controller
    controller('exitConfirmation',['$scope','$uibModal','$rootScope',
        function($scope,$uibModal,$rootScope){

            $scope.saveChanges = function(){
                $scope.$close();
                $scope.params.confirm = "save";
            }
            $scope.goBack = function(){

                $scope.$close();
                $scope.params.confirm = 'back';

            }

            $scope.exit = function(){

                $scope.$close();
                $scope.params.confirm = 'exit';

            }

        }]);