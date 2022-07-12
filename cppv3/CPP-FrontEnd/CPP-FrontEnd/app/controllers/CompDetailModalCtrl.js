// Component Detail Modal Controller

angular.module('cpp.controllers')
    .controller('CompDetailModalCtrl',
    ['$state','$scope','$rootScope',
        function($state,$scope){
            console.log($scope.params);

            // set data
            $scope.comp = $scope.params.dt.entity;
            $scope.par = $scope.params.asset;
            $scope.isNew = $scope.params.isNew;

            // format date and cost
            $scope.adate = moment($scope.comp.AcquisitionDate).format("MM/DD/YYYY");
            $scope.cost = "$" + parseFloat($scope.comp.Cost).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g,"$1,");

            // close modal
            $scope.cancel = function() {
                $scope.$close();
            };

        }]);
