angular.module('cpp.controllers').
    controller('ApprovalModalCtrl',['$scope','$rootScope',
    function($scope, $rootScope){
        $scope.cancel = function(){
            $scope.$close();

        }
    }]);