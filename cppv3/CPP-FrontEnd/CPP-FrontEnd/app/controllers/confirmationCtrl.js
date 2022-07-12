angular.module('cpp.controllers').    
    //Confirmation Controller
    controller('ConfirmationCtrl',['$scope','$uibModal','$rootScope','$location',
        function($scope,$uibModal,$rootScope, $location){

            console.log($location.path());
            console.log($scope.params);

            $scope.goBack = function(){

                $scope.$close();
                $scope.params.confirm = 'no';

            }
            if($scope.params.message){
                $scope.message = $scope.params.message;
            }else{
                $scope.message = "Are you sure you want to delete?";
            }

            $("#confirmation").on('show.bs.modal',function(event){
            });
            $scope.delete = function(){

                $scope.$close();
                $scope.params.confirm = 'yes';

            }

        }]);