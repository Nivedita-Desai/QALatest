angular.module('cpp.controllers').
    controller('LoginCtrl',['UserName','authService','$scope','$rootScope','$location','localStorageService','$menuItems','$state','$window',
        function (UserName, authService, $scope, $rootScope, $location, localStorageService, $menuItems, $state, $window) {

            console.log(localStorage.getItem('directUrlPath'));

            $rootScope.isLoginPage = true;
            $rootScope.isLightLoginPage = false;
            $rootScope.isLockscreenPage = false;
            $rootScope.isMainPage = false;
            console.log($rootScope);
            // Reveal Login form
            setTimeout(function(){ $(".fade-in-effect").addClass('in'); }, 1);
            $scope.loginData = {
                userName: "",
                password: ""
            };
            $scope.register = function(){
                console.log("Succesfully");
                $location.path('/signup');
            };
            $scope.recoverPassword = function(){
                console.log("recover password");
                $location.path('/password-recovery');
            }
            $scope.changePassword = function () {
                console.log("change password");
                $location.path('/change-password');
            }
            $scope.message = "";
            var $sidebarMenuItems = $menuItems.instantiate();
            $scope.menuItems = $sidebarMenuItems.prepareSidebarMenu().getAll();
            console.log($scope.menuItems[5]);


            $scope.login = function () {
                document.getElementById("loading").style.display = "block";
                if ($scope.loginData.userName == "") {
                    dhtmlx.alert("Please enter username.");
                    document.getElementById("loading").style.display = "none";
                    return;
                }
                else if ($scope.loginData.password == "") {
                    dhtmlx.alert("Please enter password.");
                    document.getElementById("loading").style.display = "none";
                    return;
                }
                else {
                    console.log("DIrect login Path");
                    console.log($scope.loginData);
                    //debugger;
                    authService.login($scope.loginData).then(function (response) {

                        console.log(response);
                        ///luan here - change password
                        console.log(response.data.passwordChangeRequired);
                        if (response.data.passwordChangeRequired == "True") {
                            document.getElementById("loading").style.display = "none";
                            $scope.changePassword();
                            return;
                        }

                        UserName.addUser($scope.loginData.userName);
                        console.log(UserName.getUser());
                        $rootScope.user = $scope.loginData.userName;
                        $scope.$emit('user', { userName: $scope.loginData.userName });

                        //debugger;
                        //luan here
                        var directUrl = localStorageService.get("directUrlPath");
                        console.log(directUrl);

                        //$rootScope.$broadcast('USERNAME', { any: userName });
                        var auth = localStorageService.get("authorizationData");
                        console.log(auth);

                        if (directUrl && directUrl.indexOf("cost-gantt") >= 0) {
                            document.getElementById("loading").style.display = "none";
                            var urlArray = directUrl.split("cost-gantt/")[1].split("/");
                            window.location.hash = '#/app/cost-gantt/' + urlArray[0] + "/" + urlArray[1] + "/" + urlArray[2];
                        }
                        //debugger
                        else if (directUrl && directUrl.indexOf("po-Approval") >= 0 && auth.role == "Accounting") {
                            document.getElementById("loading").style.display = "none";
                            var urlArray = directUrl.split("po-Approval/")[1].split("/");
                            window.location.hash = '#/app/po-Approval/' + urlArray[0];
                        } 
                        else {

                            document.getElementById("loading").style.display = "none";
                            window.location.hash = '#/app/wbs';
                        }




                        $location.path('/layout/user-info-navbar.html');
                        //$state.reload();
                        //$window.location.reload();
                        //if(auth.userName === 'rmani'){
                        //
                        //    $scope.menuItems.splice(5,1);
                        //}
                    },
                        function (err) {
                            console.log(err);
                            document.getElementById("loading").style.display = "none";
                            if (err && err.data)
                                $scope.message = err.data.error_description;
                        });
                }
            };

            // Set Form focus
            $("form#login .form-group:has(.form-control):first .form-control").focus();
        }]);