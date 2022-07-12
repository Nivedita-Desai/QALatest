'use strict';

angular.module('cpp.services').

    //Authentication Services
    //authService is a services for authenticate and validating user identity
    factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

        var serviceBase = serviceBasePath;
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var _saveRegistration = function (registration) {

            _logOut();
            // return $http.post(serviceBase + 'response/user', registration).then(function (response)
            var config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
            return $http.post(serviceBase + 'response/user', registration, config).then(function (response) {
                return response;
            });

        };

        var _checklicense = function (registrationkey) {
            var config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
            return $http({
                method: 'GET',
                url: license4jPath + 'verifylicense/' + registrationkey,
                config: config

            }).then(function success(response) {
                return response;
            }, function error(err) {
                return err;
            });


        };

        var _releaselicense = function (registrationkey) {
            var config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
            return $http({
                method: 'GET',
                url: license4jPath + 'releaselicense/' + registrationkey,
                config: config



            }).then(function success(response) {
                return response;
            }, function error(err) {
                return err;
            });




        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            console.log(data);

            var deferred = $q.defer();
            var req = {
                method: 'POST',
                url: serviceBase + 'Token',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                data: data,
                ignore: true

            }



            $http(req).then(function success(response) {
                console.log(response);
                localStorageService.set('authorizationData', { token: response.data.access_token, userName: loginData.userName, role: response.data.role, acl: response.data.acl, threshold: response.data.threshold, employeeID: response.data.employeeID, passwordChangeRequired: response.data.passwordChangeRequired });

                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;

                localStorage.removeItem('pgmId');
                localStorage.removeItem('projId');
                localStorage.removeItem('pgmEltId');

                deferred.resolve(response);
            }, function error(err, status) {
                console.log(err);

                _logOut();
                deferred.reject(err);
            });


            return deferred.promise;
        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');
            localStorage.removeItem('orgId');
            localStorage.removeItem('pgmId');
            localStorage.removeItem('projId');
            localStorage.removeItem('pgmEltId');
            _authentication.isAuth = false;
            _authentication.userName = "";

        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
            }

        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.checklicense = _checklicense;
        authServiceFactory.releaselicense = _releaselicense;

        return authServiceFactory;
    }]).

    //authInterceptorService is used to capture http request and add token to its header before
    //it is sent to the backend
    factory('authInterceptorService', ['$q', '$location', 'localStorageService', function ($q, $location, localStorageService) {

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {

                config.headers.Authorization = 'Bearer ' + authData.token;

            }
            //console.log(config);
            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $location.path('/login');
            }
            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }]).
    factory('UserName', function ($rootScope) {
        var UserName = [];
        //  $rootScope.$broadcast("USERNAME",UserName);
        var addUser = function (user) {
            UserName.push(user);
        };

        var getUser = function () {
            return UserName;

        };

        return {
            addUser: addUser,
            getUser: getUser
        }; s
    }).
    factory('PasswordRecovery', function ($resource) {
        var serviceBase = serviceBasePath;
        console.log(serviceBase);
        return $resource(serviceBase + 'response/forgotPassword');
    }).
    factory('User', ['$resource', function ($resource) {
        //  return $resource("http://localhost:29986/api/request/user");
        return $resource(serviceBasePath + "Request/User");
    }]).factory('UserByEmployeeListID', ['$resource', function ($resource) {
        return $resource(serviceBasePath + "Request/GetUserByEmployeeListID/:EmployeeListID/:Dummy");
    }]);