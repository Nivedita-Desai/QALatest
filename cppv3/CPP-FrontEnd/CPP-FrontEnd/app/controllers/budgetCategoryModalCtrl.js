angular.module('cpp.controllers').
    controller('BudgetCategoryModalCtrl',['$scope','$rootScope','$uibModal','UpdateCategory','$uibModalInstance','$http',
        function($scope,$rootScope,$uibModal,UpdateCategory,$uibModalInstance,$http) {
            var url = serviceBasePath+'response/activityCategory/';
           // var url = 'http://localhost:29986/api/response/activityCategory/';
        if($scope.params.newOrEdit==="edit" && $scope.params) {

            $scope.budgetCategoryItem = angular.copy($scope.params.budgetCategoryItem);
            $scope.saveChanges = function(){
                var dataObj = {Operation:'2',
                    CategoryID:$scope.budgetCategoryItem.CategoryID,
                    CategoryDescription:$scope.budgetCategoryItem.CategoryDescription,
                    SubCategoryID:$scope.budgetCategoryItem.SubCategoryID,
                    SubCategoryDescription:$scope.budgetCategoryItem.SubCategoryDescription,
                    Phase:$scope.budgetCategoryItem.Phase
                }

            $http.post(url,dataObj).then(function(response){
                if(response.data.result ==='Success'){
                }else{
                    alert("Failed to update Budget Category");
                }
                $uibModalInstance.close();
            })
        }}
        else if($scope.params.newOrEdit ==='new' && $scope.params)
        {
            $scope.saveChanges = function(){

              var dataObj = {Operation:'1',
                  CategoryID:$scope.budgetCategoryItem.CategoryID,
                  CategoryDescription:$scope.budgetCategoryItem.CategoryDescription,
                  SubCategoryID:$scope.budgetCategoryItem.SubCategoryID,
                  SubCategoryDescription:$scope.budgetCategoryItem.SubCategoryDescription,
                  Phase: $scope.budgetCategoryItem.Phase
              }

                $http.post(url,dataObj).then(function(response){
                    if(response.data.result==='Success'){
                    }else{
                        alert('Failed to add new Budget Category');

                    }
                    $uibModalInstance.close();
                })
            }
        }
function update(){

    if($scope.params.newOrEdit==="edit" && $scope.params) {

            var dataObj = {
                Operation: '2',
                CategoryID: $scope.budgetCategoryItem.CategoryID,
                CategoryDescription: $scope.budgetCategoryItem.CategoryDescription,
                SubCategoryID: $scope.budgetCategoryItem.SubCategoryID,
                SubCategoryDescription: $scope.budgetCategoryItem.SubCategoryDescription,
                Phase: $scope.budgetCategoryItem.Phase
            }

            $http.post(url,dataObj).then(function(response){
                if(response.data.result ==='Success'){
                }else{
                    alert("Failed to update Budget Category");
                }
                $uibModalInstance.close();
            })
        }
    else if($scope.params.newOrEdit ==='new' && $scope.params)
    {

            var dataObj = {Operation:'1',
                CategoryID:$scope.budgetCategoryItem.CategoryID,
                CategoryDescription:$scope.budgetCategoryItem.CategoryDescription,
                SubCategoryID:$scope.budgetCategoryItem.SubCategoryID,
                SubCategoryDescription:$scope.budgetCategoryItem.SubCategoryDescription,
                Phase: $scope.budgetCategoryItem.Phase
            }

            $http.post(url,dataObj).then(function(response){
                if(response.data.result==='Success'){
                }else{
                    alert('Failed to add new Budget Category');
                }
                $uibModalInstance.close();
            })

    }
}

        $scope.goBack = function(){
            $uibModalInstance.close();

        }



    }]);