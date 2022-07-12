angular.module('cpp.controllers').
    controller('UserModalCtrl',['$http','$scope','$rootScope',function($http,$scope, $rootScope){
        $scope.userItem = angular.copy($scope.params.userItem);

        var operation = ($scope.params.newOrEdit === 'new') ? '1' : '2';

        var serviceUrl = serviceBasePath+'response/user'
        $http.get(serviceBasePath+'request/role').then(function(response){
            $scope.Role = response.data.result;

        })
        $scope.saveChanges= function(){
            //var data = {
            //    'Operation' : operation ,
            //     'UserID' : $scope.userItem.UserID,
            //    'FirstName' : $scope.userItem.FirstName,
            //    'MiddleName' : $scope.userItem.MiddleName,
            //    'LastName' : $scope.userItem.LastName,
            //    'Email' : $scope.userItem.Email,
            //    'Role' : $scope.userItem.Role
            //
            //}
            var fullName = $scope.userItem.FirstName + ' ' + $scope.userItem.LastName;
            var data = {
                'Operation': operation,
                'FullName' : fullName,
                'UserID' : $scope.userItem.UserID,
                'FirstName': $scope.userItem.FirstName,
                'MiddleName' : $scope.userItem.MiddleName,
                'LastName' : $scope.userItem.LastName,
                'LoginPassword': $scope.userItem.LoginPassword,
                'Email': $scope.userItem.Email,
                'Role': $scope.userItem.Role

            }
            if($scope.params.newOrEdit === 'new'){
                    console.log(data);
                $http({
                    url: serviceUrl,
                    method: "POST",
                    data: JSON.stringify(data),
                    headers: {'Content-Type': 'application/json'}
                }).then(function(response){
                    console.log(response);
                    if(response.data.result === 'Success'){
                        $scope.$close();
                    }
                    else{
                        alert("failed to add new User");
                    }
                });

            }
            else if($scope.params.newOrEdit ==='edit'){
                $http({
                    url: serviceUrl,
                    method: "POST",
                    data: JSON.stringify(data),
                    headers: {'Content-Type': 'application/json'}
                }).then(function(response){
                    console.log(response);
                    if(response.data.result === 'Success'){
                        $scope.$close();
                    }
                    else{
                        alert("failed to Edit User");
                    }
                });
            }
        }
    }]);