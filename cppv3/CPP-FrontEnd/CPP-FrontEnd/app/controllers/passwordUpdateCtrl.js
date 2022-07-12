/**
 * Created by ikhong on 1/29/2016.
 */
angular.module('cpp.controllers').
    controller('PasswordUpdateCtrl', ['UserName', 'authService', '$scope', '$rootScope', '$location', 'localStorageService', '$menuItems', '$state', '$window', "PasswordRecovery", '$http','$stateParams',
function (UserName, authService, $scope, $rootScope, $location, localStorageService, $menuItems, $state, $window, PasswordRecovery, $http,$stateParams) {
           
            $rootScope.isLoginPage = true;
            $rootScope.isLightLoginPage = false;
            $rootScope.isLockscreenPage = false;
            $rootScope.isMainPage = false;
            $scope.message = "";
            console.log($rootScope);
            // Reveal Login form
            setTimeout(function () { $(".fade-in-effect").addClass('in'); }, 1);

            //cancel
            $scope.back = function () {
                $location.path('/login');
            }
            $scope.info = {
                UserID: "",
                password: "",
                confirmPassword : ""
    };

   
            console.log($stateParams.token);
    
            $scope.getUserNameByToken = function () {
                var token = {
                    Token : $stateParams.token
                }
                $http({
                    method: 'POST',
                    url: serviceBasePath + "Request/getUserByToken",
                    data: token ,
                    headers: { 'Content-Type': 'application/json' }

                }).then(function successCallback(response) {
                    console.log(response);
                    // this callback will be called asynchronously
                    // when the response is available
                    if (response.data.result == null) {
                        dhtmlx.alert("The token has been used!")
                        $location.path('/login');
                    }
                    $scope.user = response.data.result;
                    $scope.info.UserID = response.data.result.UserID;
                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            }
            $scope.getUserNameByToken();

            $scope.reset = function () {
                console.log('resetting');

                if ($scope.info.password == undefined || $scope.info.password == null || $scope.info.password == '') {
                    dhtmlx.alert("Password field is empty!");
                    return;
                }
                if ($scope.info.confirmPassword == undefined || $scope.info.confirmPassword == null || $scope.info.confirmPassword == '') {
                    dhtmlx.alert("You need to confirm your password!");
                    return;
                }
                if ($scope.info.password != $scope.info.confirmPassword ) {
                    dhtmlx.alert("Passwords don't match!");
                    return;
                }

                //-----------------------------Nivedita 09-11-2021 password validation-------------------------------------------------------------------------



                const regexPassword = /^(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$/;
                if (!regexPassword.test($scope.info.password)) {
                    dhtmlx.alert({
                        text: "Enter valid password.<br/> Password should be at least 8 characters long, should contain atleast one number,one capital character and one special character.",
                        width: "300px"
                    });
                    return;
                }
                if ($scope.info.password !== $scope.info.confirmPassword) {
                    dhtmlx.alert({
                        text: "Password and Confirm Password must be match.",
                        width: "300px"
                    });
                    return;
                }
//------------------------------------------------------------------------------------------------------------------------

                var listToSave = [];
                listToSave.push({
                    UserID: $scope.info.UserID,
                    LoginPassword: $scope.info.password,
                    Operation: 4,
                    Id: $scope.user.Id,
                    FirstName: $scope.user.FirstName,
                    MiddleName : $scope.user.MiddleName,
                    LastName: $scope.user.LastName,
                    Email: $scope.user.Email,
                    FullName: $scope.user.FullName,
                    DOB: $scope.user.DOB,
                    Role :$scope.user.Role
                });

                $http({
                    url: serviceBasePath + 'response/user',
                    method: "POST",
                    data: listToSave,
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    console.log(response)
                    dhtmlx.alert(response.data.result);
                    $location.path('/login');
                }, function error(response) {
                    dhtmlx.alert("Failed to save.");
                });


            }

            $scope.message = "";
           
            $("form#login .form-group:has(.form-control):first .form-control").focus();
        }]);