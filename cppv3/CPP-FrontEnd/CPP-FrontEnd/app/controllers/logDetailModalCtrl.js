// Log Detail Modal Controller

angular.module('cpp.controllers')
    .controller('LogDetailModalCtrl',
    ['$state','$scope',
        function($state,$scope){
            console.log($scope.params);

            // set data
            $scope.isNew = $scope.params.isNew;
            $scope.ldasset = ($scope.isNew)? {Tag:"New Asset"} : $scope.params.asset;
            $scope.logDetail = $scope.params.dt.entity;

            console.log($scope.params.asset);

            // format date and cost
            $scope.sd = moment($scope.logDetail.StartDate).format("MM/DD/YYYY");
            $scope.ed = moment($scope.logDetail.EndDate).format("MM/DD/YYYY");
            $scope.cost = "$" + parseFloat($scope.logDetail.Cost).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g,"$1,");

            // ok and cancel to close the modal
            $scope.close = function(){
                $scope.$close();
            };

        }]);
