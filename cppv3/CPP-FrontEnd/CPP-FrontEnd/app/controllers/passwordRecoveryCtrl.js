/**
 * Created by ikhong on 1/29/2016.
 */
angular.module('cpp.controllers').
    controller('PasswordRecoveryCtrl', ['UserName', 'authService', '$scope', '$rootScope', '$location', 'localStorageService',
        '$menuItems', '$state', '$window', "PasswordRecovery", '$http',
        function (UserName, authService, $scope, $rootScope, $location, localStorageService,
            $menuItems, $state, $window, PasswordRecovery, $http) {

            $rootScope.isLoginPage = true;
            $rootScope.isLightLoginPage = false;
            $rootScope.isLockscreenPage = false;
            $rootScope.isMainPage = false;
            $scope.message = "";
            console.log($rootScope);
            // Reveal Login form
            setTimeout(function(){ $(".fade-in-effect").addClass('in'); }, 1);

            //cancel
            $scope.cancel = function(){
                $location.path('/login');
            }
            $scope.info = {
                userName: "",
                email: ""
            };
           $scope.getNewPassword = function(){
               var obj = {};

               console.log($scope.info.userName);
               console.log($scope.info.email);
               if($scope.info.userName == ""){
                   dhtmlx.alert("Username is empty");
                   return;
               }
               if($scope.info.email == ""){
                   dhtmlx.alert("Email is empty");
                   return;
               }
                obj.UserID = $scope.info.userName;
               obj.Email = $scope.info.email;
               var baseLen = $location.absUrl().length - $location.url().length;
               obj.routeInfo = $location.absUrl().substring(0,baseLen);
               console.log(obj);
               var req = {
                   method: 'POST',
                   url: serviceBasePath + "/Response/forgotPassword",
                   headers: {
                       'Content-Type': 'application/json'
                   },
                   data: obj
               }

               $http(req).then(function success(response) {
                   console.log(response);
                   dhtmlx.alert(response.data.result);
                   if(response.data.result == "The instructions to reset your password has been sent to your email. Please check your email.")
                       $location.path('/login');
               }, function error(response){
                   console.log(response);
                   dhtmlx.alert("Unable to process password reset at this time. Please try again later!");
              });

               //PasswordRecovery.save(obj,function(response){
               //    console.log(response);
               //    //$scope.message = response.data.result;
               //})
            }
            $scope.message = "";
            //var $sidebarMenuItems = $menuItems.instantiate();
            //$scope.menuItems = $sidebarMenuItems.prepareSidebarMenu().getAll();
            //console.log($scope.menuItems[5]);
            //
            //
            //$scope.login = function () {
            //    console.log($scope.loginData);
            //    authService.login($scope.loginData).then(function (response) {
            //            UserName.addUser($scope.loginData.userName);
            //            console.log(UserName.getUser());
            //            $rootScope.user = $scope.loginData.userName;
            //            $scope.$emit('user',{userName: $scope.loginData.userName});
            //            window.location.hash = '#/app/wbs';
            //            //$rootScope.$broadcast('USERNAME', { any: userName });
            //            var auth = localStorageService.get("authorizationData");
            //            console.log(auth);
            //            $location.path('/layout/user-info-navbar.html');
            //            //$state.reload();
            //            //$window.location.reload();
            //            //if(auth.userName === 'rmani'){
            //            //
            //            //    $scope.menuItems.splice(5,1);
            //            //}
            //        },
            //        function (err) {
            //            if(err)
            //                $scope.message = err.error_description;
            //        });
            //};
            // Set Form focus
            $("form#login .form-group:has(.form-control):first .form-control").focus();
        }]);