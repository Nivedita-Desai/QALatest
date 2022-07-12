angular.module('cpp.controllers').
    controller('fundModalCtrl', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {




        if ($scope.params.newOrEdit === 'new') {
            $scope.fundItem = null;

            $scope.saveChanges = function () {

                var dataObj = {
                    'Operation': '1',
                    'FundTypeId': '5',//$scope.fundItem.FundTypeId,
                    'Fund': $scope.fundItem.Fund,
                    'Amount': $scope.fundItem.Amount,
                    'Availability': $scope.fundItem.Availability,
                    'BalanceRemaining': $scope.fundItem.BalanceRemaining
                }
                $http({
                    //url: 'http://192.168.0.19:1832/response/fundtype',
                    url: serviceBasePath + '/response/fundtype',
                    //  url: 'http://localhost/api/response/phasecode',
                    method: "POST",
                    data: JSON.stringify(dataObj),
                    headers: {'Content-Type': 'application/json'}
                }).then(function (response) {
                    if (response.data.result === 'Success') {
                        $scope.$close();
                    }
                    else {
                        alert("Add Fund failed");
                    }

                });
            }
        }
        else if ($scope.params.newOrEdit === 'edit') {
            $scope.fundItem = angular.copy($scope.params.fundItem);

            $scope.saveChanges = function () {
                var dataObj = {
                    'Operation': '2',
                    'FundTypeId': '5',
                    'Fund': $scope.fundItem.Fund,
                    'Amount': $scope.fundItem.Amount,
                    'Availability': $scope.fundItem.Availability,
                    'BalanceRemaining': $scope.fundItem.BalanceRemaining
                }
                $http({
                    //url: 'http://192.168.0.19:1832/response/fundtype',
                    url: 'http://localhost:29986/api/response/fundtype',
                    //  url: 'http://localhost/api/response/phasecode',
                    method: "POST",
                    data: JSON.stringify(dataObj),
                    headers: {'Content-Type': 'application/json'}
                }).then(function (response) {
                    if (response.data.result === 'Success') {
                        $scope.$close();

                    }
                    else {
                        alert("Edit failed");
                    }
                });
            }


        }

        $scope.goBack = function () {
            $scope.$close();
        }
    }]);