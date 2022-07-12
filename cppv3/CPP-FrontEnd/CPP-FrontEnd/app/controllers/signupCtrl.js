angular.module('cpp.controllers').
    controller('SignupCtrl',['authService','$scope','$rootScope','$location', function (authService,$scope, $rootScope,$location) {
        //$rootScope.isLoginPage = true;
        //$rootScope.isLightLoginPage = false;
        //$rootScope.isLockscreenPage = false;
        //$rootScope.isMainPage = false;
        $scope.emailRequired=false;
        $scope.userNameRequired=false;
        $scope.passwordRequired = false;

        // Reveal Login form
        setTimeout(function(){ $(".fade-in-effect").addClass('in'); }, 1);
        $scope.registerData = {
            "Operation" : "1",
            "FirstName":"",
            "MiddleName":"",
            "LastName":"",
            "UserID" : "",
            "LoginPassword" : "",
            "FullName" : "",
            "Email" : "",
            "Role" : ""
        };
        $scope.cancel = function(){

            $location.path('/login');
        };
        $scope.message = "";
        var verifyPassowrd = function(){
            if($scope.registerData.LoginPassword !== $scope.registerData.confirmPassword){
                dhtmlx.alert({text:"Password are not matched, please verify that password are the same!", width:"500px"});
                return false;
            }
            else{
                return true;
            }
        }
        $scope.checkEmail = function(){

            if($scope.emailRequired == true){
                $scope.emailRequired = false;
            }
        }
        $scope.checkUserName = function(){
            if($scope.userNameRequired == true)
                $scope.userNameRequired = false;
        }
        $scope.checkPassword = function(){
            if($scope.passwordRequired == true)
                $scope.passwordRequired = false;
        }

        $scope.register = function () {

            if($scope.registerData.Email == "" || $scope.registerData.UserID == ""){
               // $scope.message = "Email is required";
                if($scope.registerData.Email =="" )
                    $scope.emailRequired=true;
                if($scope.registerData.UserID == "")
                    $scope.userNameRequired = true;
                if($scope.registerData.LoginPassword == "")
                    $scope.passwordRequired = true;
                return;
            }
            var checkPassword = verifyPassowrd();
            console.log(checkPassword);
            if(checkPassword){
                console.log($scope.registerData);
                var registerData = [];
                registerData.push($scope.registerData);
                console.log($scope.registerData)
                authService.saveRegistration(registerData).then(function (response) {
                    console.log(response);
                    dhtmlx.alert(response.data.result);
                        //if(response.data.result === "Success"){
                        //      dhtmlx.alert("Registered successfully");
                        //}else{
                        //    return;
                        //}
                    $location.path('/login');

                    },
                    function (err) {
                        $scope.message = err.error_description;
                    });
            };
            }

        // Set Form focus
        $("form#login .form-group:has(.form-control):first .form-control").focus();
    }]);