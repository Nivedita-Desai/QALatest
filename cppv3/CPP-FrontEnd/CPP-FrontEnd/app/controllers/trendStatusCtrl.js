angular.module('cpp.controllers').    
    //Trend status Control
    controller('TrendStatusCtrl',['$scope','$rootScope','Status','$uibModal','$http',
        function($scope, $rootScope,Status,$uibModal,$http){

        Status.get({},function(statusData){
            $scope.statusCollection = statusData.result;

        });
        $scope.setTrendStatus = function(status) {

            $scope.statusItem = status;

        }
        $scope.newTrendStatus= function(){
        }

        $scope.deleteTrendStatus = function() {
            alert("Delete Trend Status item");
        }

        //open a modal to edit data
        $scope.editTrendStatus = function(){
            var scope = $rootScope.$new();
            scope.params= {statusItem:$scope.statusItem};
            $rootScope.modalInstance = $uibModal.open({
                scope: scope,
                backdrop: 'static',
                keyboard: false,
                templateUrl: 'app/views/modal/trend_status_modal.html',
                controller:'TrendStatusModalCtrl',
                size: 'md'
            });

            // after close the popup modal
            $rootScope.modalInstance.result.then(function () {

            });
        }

    }]);