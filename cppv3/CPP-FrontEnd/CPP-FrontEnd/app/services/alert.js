/**
 * Created by ikhong on 11/17/2015.
 */
'use strict';

angular.module('cpp.services').
    service('alertBox', ['$http', '$q', 'localStorageService','$rootScope','$uibModal',
            function ($http, $q, localStorageService, $rootScope,$uibModal) {
                var alertFactory = {};
                var _showAlert = function(message){
                    //$scope.confirm = "";
                    console.log(message);
                    var scope = $rootScope.$new();
                    scope.params = {
                       // confirm:$scope.confirm,
                        message : message
                    };
                    $rootScope.modalInstance = $uibModal.open({
                        scope: scope,
                        templateUrl: 'app/views/Modal/alert_modal.html',
                        controller: 'alertConfirmation',
                        size: 'md',
                        backdrop: true
                    });
                    //$rootScope.modalInstance.result.then(function(data){
                    //    console.log(scope.params.confirm);
                    //         if(scope.params.confirm === "back"){
                    //                //do nothing
                    //                $scope.$close();
                    //         }
                    //});
                }

                alertFactory.showAlert = _showAlert;

                return alertFactory;
    }]);