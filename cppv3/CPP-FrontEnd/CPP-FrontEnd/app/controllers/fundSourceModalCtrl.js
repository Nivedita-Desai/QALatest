angular.module('cpp.controllers').
    controller('fundSourceModalCtrl', ['$scope', '$rootScope', '$http','TrendFund','ProgramFund','$uibModal','$filter',
        function ($scope, $rootScope, $http,TrendFund,ProgramFund,$uibModal,$filter) {

        $scope.checkFund = function(){
            var amount= $scope.assignAmount.replace(/[^0-9\.]+/g,"");
            console.log(amount);
            $scope.assignAmount =  $filter('currency')(amount,'$',2);
        }
        $scope.fundChange = function (fund) {
            console.log(fund);
            $scope.assignAmount = "";
            angular.forEach($scope.fundList, function (item) {
                if (item.FundName == fund) {
                    $scope.fundAvailable = item.FundRemaining;
                }
            });
        }
        var temp = angular.copy($scope.params.fundListLookup);
            console.log($scope.params.trend);
        var trendID = $scope.params.trend.TrendID;
        var projectID = $scope.params.projectID;
            $scope.totalBudget = $scope.params.totalBudget.toFixed(2);
            console.log($scope.params.totalBudget);
            $scope.listToDelete = [];

        TrendFund.lookup().get({
            "TrendID" : trendID,
            "ProjectID": projectID
        },function(response){
           console.log(response);
            $scope.fundTable = response.result;
            updateTotalCost();
        });
        console.log("What am I temp?");
        console.log(temp);
        $scope.fundList = temp;

        $scope.fundTable = [];
        $scope.addFund = function () {
           $scope.assignAmount =  $scope.assignAmount.replace(/[^0-9\.]+/g,"");
            if(isNaN($scope.assignAmount)== true){
                dhtmlx.alert("Please assign a valid number");
                return;
            }
            if(!$scope.assignAmount){
                dhtmlx.alert("Please assign a fund amount first");
                return;
            }

            var isExist = validateFundType($scope.fund);
            if (isExist == true) {
                //TO UPDATE
                var fundToAddBack ={};
                var fundTotal = 0;
                var isUpdateBigger = false;
                angular.forEach($scope.fundTable,function(item){
                    console.debug("FundTable",item);
                   if(item.FundName == $scope.fund){
                       angular.forEach($scope.fundList,function(fundItem){
                           console.debug("FundList", fundItem);
                           if(fundItem.FundName == item.FundName){
                               fundTotal = parseFloat(fundItem.FundRemaining) - parseFloat($scope.assignAmount);
                               $scope.fundAvailable = fundTotal;
                               fundItem.FundRemaining = fundTotal;
                           }
                       })
                       //if(parseFloat(fundTotal) < parseFloat($scope.assignAmount)){
                       //    dhtmlx.alert({text:"Can't assign amount that is bigger than what is available.",
                       //                     width:"400px"});
                       //    isUpdateBigger = true;
                       //    return;
                       //}
                       //else {
                       //    //New Code
                       //
                       //    //
                       //    fundToAddBack = angular.copy(item);
                       //    item.Operation = 1;
                       //    $scope.deleteRow(item);
                       //    return;
                       //}
                        item.Operation = 2;
                       item.FundAssign = parseFloat(item.FundAssign) + parseFloat($scope.assignAmount);
                       updateTotalCost();
                   }

                });
                //if(isUpdateBigger== false) {
                //    fundToAddBack.FundAssign = $scope.assignAmount;
                //    if (fundToAddBack.Operation != 1)
                //        fundToAddBack.Operation = 2;
                //    $scope.fundTable.push(fundToAddBack);
                //    angular.forEach($scope.fundList, function (item) {
                //        if (item.FundName == $scope.fund) {
                //
                //            var diff = parseFloat(item.FundRemaining) - parseFloat($scope.assignAmount);
                //            if (diff < 0) {
                //                dhtmlx.alert({text:"Can't assign amount that is bigger than what is available.",
                //                            width:"400px"});
                //                return;
                //            } else {
                //                item.FundRemaining = diff;
                //                item.FundRequest = parseFloat(item.FundRequest) + parseFloat($scope.assignAmount);
                //                $scope.fundAvailable = diff;
                //                updateTotalCost();
                //            }
                //        }
                //    })
                //}
               return;

            }

            var fundItem = {
                FundName: $scope.fund,
                FundAssign: $scope.assignAmount,

                Operation : 1
            }

            angular.forEach($scope.fundList, function (item) {
                if (item.FundName == $scope.fund) {
                    var diff = parseFloat(item.FundRemaining) - parseFloat($scope.assignAmount);
                    if(diff < 0){
                        dhtmlx.alert({text:"Can't assign amount that is bigger than what is available.",
                                        width:"400px"});
                        return;
                    }else {
                        $scope.fundTable.push(fundItem);
                        var fundRequest = parseFloat(item.FundRequest) + parseFloat($scope.assignAmount);
                        item.FundRemaining = diff;
                        item.FundRequest = fundRequest;
                        $scope.fundAvailable = diff;
                        updateTotalCost();
                    }
                }
            })
            console.log($scope.fundTable);
        }
        $scope.setFund = function(rowItem){
            //console.log()
            $scope.selectedItem = rowItem;
            $scope.fund = rowItem.FundName;
            $scope.assignAmount = rowItem.FundAssign;
            angular.forEach($scope.fundList, function (item) {
                if (item.FundName == $scope.fund) {
                    $scope.fundAvailable = item.FundRemaining;
                }
            });

        }
        $scope.deleteFund = function () {
            if($scope.fundTable.length>0) {
                var scope = $rootScope.$new();
                $scope.confirm = "";
                scope.params = {
                    confirm: $scope.confirm,
                    message:"All funds will be removed. Are you sure you want to continue?"
                };
                $rootScope.modalInstance = $uibModal.open({
                    templateUrl: 'app/views/Modal/confirmation_dialog.html',
                    size: 'md',
                    controller: 'ConfirmationCtrl',
                    scope: scope

                });


                console.log($scope.confirm);
                $rootScope.modalInstance.result.then(function (data) {
                    if (scope.params.confirm === 'yes') {
                        console.log($scope.fundTable);
                        var tempTable = angular.copy($scope.fundTable);
                        angular.forEach(tempTable, function (item) {
                            console.log(item);
                            $scope.deleteRow(item);
                        });

                    } else {
                        return;
                    }
                });
            }
        }
        $scope.saveChanges = function () {
            console.log($scope.totalFund);
            console.log($scope.totalBudget);
            //if($scope.totalFund > $scope.totalBudget){
            //    dhtmlx.alert({text:"The Assigned Funds Are larger than the total budget.",
            //                    width:"400px"});
            //    return;
            //}
            //if($scope.totalFund < $scope.totalBudget){
            //    dhtmlx.alert({text:"The assgined Funds are not sufficient.",width:"350px"});
            //    return;
            //}

            //
            var listToUpdate = [];
            angular.forEach($scope.fundTable,function(item){
                item.ProjectID = projectID;
                item.TrendID = trendID;
                item.programFundList = $scope.fundList;
                listToUpdate.push(item);
            });
            angular.forEach($scope.listToDelete,function(item){
                item.Operation = 3;
                item.programFundList = $scope.fundList;
                listToUpdate.push(item);
            });
            TrendFund.persist().save(listToUpdate,function(response){
                angular.forEach($scope.fundList,function(item){
                   item.Operation = 2;
                    ProgramFund.persist().save(item,function(response){
                        $scope.$close();
                    });
                });

             //
            });

        }
        $scope.deleteRow = function (rowItem) {
            console.log('is luan here?');
            for (var i = 0; i < $scope.fundTable.length; i++) {
                if ($scope.fundTable[i].FundName === rowItem.FundName) {
                    var index = i;
                }
            }

            console.log($scope.fundTable[index]);
            if($scope.fundTable[index].Operation != 1)
                  $scope.listToDelete.push($scope.fundTable[index]);

            $scope.fundTable.splice(index, 1);
            updateTotalCost();
            console.log(rowItem);
            angular.forEach($scope.fundList, function (item) {
                console.log(item);
                if (item.FundName == rowItem.FundName) {
                    item.FundRemaining = parseFloat(item.FundRemaining) + parseFloat(rowItem.FundAssign);
                    item.FundRequest = parseFloat(item.FundRequest) - parseFloat(rowItem.FundAssign);
                    if ($scope.fund == rowItem.FundName) {
                        $scope.fundAvailable = item.FundRemaining;
                    }
                }
                console.log(item);
            })


        }
        var updateTotalCost = function () {
            var total = 0;
            angular.forEach($scope.fundTable, function (item) {
                total += parseFloat(item.FundAssign);
            });
            console.log(total);
            $scope.totalFund = total;
        }
        var validateFundType = function (fundName) {
            var isExist = false;
            angular.forEach($scope.fundTable, function (item) {
                if (item.FundName == fundName) {
                    isExist = true;
                    return;
                }
            })
            return isExist;
        }
        $scope.goBack = function () {
            $scope.$close();
        }


    }]);