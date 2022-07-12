angular.module('cpp.controllers').
    controller('orderController',['$location','$scope','$rootScope','ordersService','authService',
        function($location,$scope,$rootScope,ordersService,authService){
        ordersService.getOrders().then(function(result){
            console.log(result);
            $scope.orders = result.data;
            console.log($scope.orders);
        });
        $scope.logOut = function(){

                authService.logOut();
                $location.path('/app/wbs');


            $scope.authentication = authService.authentication;
        }
    }]);