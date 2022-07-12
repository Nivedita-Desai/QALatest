angular.module('cpp.controllers').
    controller('ChangePasswordOnLoginCtrl', ['$timeout', '$http', '$scope', '$rootScope', '$uibModal', 'Organization',
        '$location', '$timeout', 'localStorageService', 'User', 'authService', '$window', '$menuItems', '$state',
        function ($timeout, $http, $scope, $rootScope, $modal, Organization,
            $location, $timeout, localStorageService, User, authService, $window, $menuItems, $state) {

            var user = localStorageService.get('authorizationData');

            console.log(user);

            $scope.message = "Password change is required!";

            $scope.goBack = function () {
                $location.path('/login');
            }

            $scope.somethingTyped = false;

            $scope.logins = function(input) {
                $scope.somethingTyped = true;
            }

            if (user == undefined || user == null) {
                dhtmlx.alert('An error has occured. Please log out and log in again, then try again.');
                $scope.goBack();
                return;
            }

            if (user.passwordChangeRequired == "False") {
                $scope.goBack();
                return;
            }

            $rootScope.isLoginPage = true;
            $rootScope.isLightLoginPage = false;
            $rootScope.isLockscreenPage = false;
            $rootScope.isMainPage = false;
            //$scope.message = "";
            console.log($rootScope);
            // Reveal Login form
            setTimeout(function () { $(".fade-in-effect").addClass('in'); }, 1);


            $scope.showOldPassword = false;
            $scope.showNewPassword = false;
            $scope.showConfirmNewPassword = false;
            $scope.loginData = {
                username: user.userName,
                oldPassword: null,
                newPassword: null,
                newPasswordConfirm: null
            }

            //Get all users
            User.get({}, function (Users) {
                var users = Users.result;

                //Find the current user
                for (var x = 0; x < users.length; x++) {
                    if (users[x].UserID == user.userName) {
                        console.log(users[x]);
                        $scope.loginData.FirstName = users[x].FirstName;
                        $scope.loginData.FullName = users[x].FullName;
                        $scope.loginData.Id = users[x].Id;
                        $scope.loginData.LastName = users[x].LastName;
                        $scope.loginData.MiddleName = users[x].MiddleName;
                        $scope.loginData.Operation = 2;
                        $scope.loginData.Role = users[x].Role;
                        $scope.loginData.UserID = users[x].UserID;
                        $scope.loginData.EmployeeID = users[x].EmployeeID;
                        $scope.loginData.PasswordChangeRequired = users[x].PasswordChangeRequired;
                        $scope.loginData.DepartmentID = users[x].DepartmentID;
                    }
                }
            });


            $scope.toggleShowOldPassword = function () {
                $scope.showOldPassword = !$scope.showOldPassword;
            }
            $scope.toggleShowNewPassword = function () {
                $scope.showNewPassword = !$scope.showNewPassword;
            }
            $scope.toggleShowConfirmNewPassword = function () {
                $scope.showConfirmNewPassword = !$scope.showConfirmNewPassword;
            }



            $scope.reset = function () {
                console.log('resetting');

                if ($scope.loginData.oldPassword == undefined || $scope.loginData.oldPassword == null || $scope.loginData.oldPassword == '') {
                    dhtmlx.alert("Old password field is empty!");
                    //$scope.message = "Old password field is empty!";
                    return;
                }
                if ($scope.loginData.newPassword == undefined || $scope.loginData.newPassword == null || $scope.loginData.newPassword == '') {
                    dhtmlx.alert("New password field is empty!");
                    //$scope.message = "New password field is empty!";
                    return;
                }
                if ($scope.loginData.newPasswordConfirm == undefined || $scope.loginData.newPasswordConfirm == null || $scope.loginData.newPasswordConfirm == '') {
                    dhtmlx.alert("Confirm new password field is empty!");
                    //$scope.message = "Confirm new password field is empty!";
                    return;
                }

                //-----------------------------Nivedita 09-11-2021 password validation-------------------------------------------------------------------------



                const regexPassword = /^(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$/;
                if (!regexPassword.test($scope.loginData.newPassword)) {
                    dhtmlx.alert({
                        text: "Enter valid password.<br/> Password should be at least 8 characters long, should contain atleast one number,one capital character and one special character.",
                        width: "300px"
                    });
                    return;
                }
                if ($scope.loginData.newPassword !== $scope.loginData.newPasswordConfirm) {
                    dhtmlx.alert({
                        text: "Password and Confirm Password must be match.",
                        width: "300px"
                    });
                    return;
                }
//------------------------------------------------------------------------------------------------------------------------

                $http.get(serviceBasePath + 'Request/UserLogin/' + user.userName + '/' + encodeURIComponent($scope.loginData.oldPassword), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                    .then(function (response) {
                        console.log(response);
                        if (response.data.result.Role == 'Not Authorized') {
                            dhtmlx.alert("Invalid old password!");
                            //$scope.message = "Invalid old password!";
                        } else if ($scope.loginData.newPassword != null && $scope.loginData.newPassword != '') {
                            if ($scope.loginData.newPassword == $scope.loginData.oldPassword) {
                                dhtmlx.alert("New password must be different from current password.");
                            }
                            else if ($scope.loginData.newPassword != $scope.loginData.newPasswordConfirm) {
                                dhtmlx.alert("Password does not match.");
                            }
                            else {
                                //else if ($scope.loginData.newPassword == $scope.loginData.newPasswordConfirm && $scope.loginData.newPassword != $scope.loginData.oldPassword) {
                                console.log('Ready to reset password!');
                                $scope.loginData.LoginPassword = $scope.loginData.newPassword;
                                $scope.loginData.PasswordChangeRequired = 0;    //requirement done!

                                var listToSave = [];
                                listToSave.push($scope.loginData);

                                $http({
                                    url: serviceBasePath + 'response/user',
                                    method: "POST",
                                    data: listToSave,
                                    headers: { 'Content-Type': 'application/json' }
                                }).then(function success(response) {
                                    console.log(response)
                                    dhtmlx.alert(response.data.result);

                                    localStorageService.set('authorizationData', { token: user.token, userName: user.userName, role: user.role, acl: user.acl, threshold: user.threshold, employeeID: user.employeeID, passwordChangeRequired: "False" });

                                    $scope.goBack();
                                    return;

                                    //if (response.data.result === 'Success') {
                                    //    dhtmlx.alert("Your password has been changed successfully!");

                                    //    //authService.logOut();

                                    //    //$location.path('/login');
                                    //}
                                    //else {
                                    //    dhtmlx.alert("Failed to save.");
                                    //}
                                }, function error(response) {
                                    dhtmlx.alert("Failed to save.");
                                });
                            }
                        }
                        //else {
                        //    dhtmlx.alert('New passwords do not match!');
                        //}
                    });
            }
        }]);