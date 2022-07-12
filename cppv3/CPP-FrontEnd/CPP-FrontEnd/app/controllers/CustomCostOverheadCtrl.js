/**
 * Created by ikhong on 11/17/2015.
 */
/**
 * Created by ikhong on 8/14/2015.
 */
angular.module('cpp.controllers').
    //Confirmation Controller
    controller('CustomCostOverheadCtrl', ['$scope', '$uibModal', '$rootScope', '$http', 'localStorageService',
        function ($scope, $uibModal, $rootScope, $http, localStorageService) {
            $scope.message = $scope.params.message;
            console.log($scope.message);
            $scope.disabledField = 'disabled';
            $scope.authRole = localStorageService.get('authorizationData').role;
            console.log($scope.params.Trend);
            console.log($scope.params);
            $scope.back = function (param) {
                $scope.$close(param);
                $scope.params.confirm = 'back';
            }
            //if (localStorageService.get('authorizationData').role === "Admin") {
            if (localStorageService.get('authorizationData').role.indexOf('Admin') != -1) {
                //$('#btnSaveCostOverhead').removeAttr('disabled');
                $scope.disabledField = '';
                //===== Jignesh-09-04-2021 ===================
                if ($scope.params.TrendNumber != "0") {
                    $scope.disabledField = 'disabled';
                }
                //============================================
            }
            else {
                $scope.disabledField = 'disabled';
            }
            $scope.isBillable = ($scope.params.Trend.CostOverheadType.ID == 1) ? true : false;
            $scope.getBirdiRate = function (costType) {
                var birdiRate = 1;
                switch (costType) {
                    case 'F':
                        birdiRate = $scope.params.LABOR_RATE;
                        break;
                    case 'ODC':
                        birdiRate = $scope.params.ODC_RATE;
                        break;
                    case 'U':
                        birdiRate = $scope.params.MATERIAL_RATE;
                        break;
                    case 'L':
                        birdiRate = $scope.params.SUBCONTRACTOR_RATE;
                        break;
                    default:
                        birdiRate = 1;

                }
                return birdiRate;
            }
            $scope.changeMarkup = function (row) {
                console.log(row);
               var birdiRate = $scope.getBirdiRate(row.CostOverhead.CostType.AbbreviatedName);
               // alert(birdiRate);
                row.NewCost = parseFloat(row.OriginalCost)  * parseFloat(row.CurrentMarkup);
                $scope.updateTotalCost();
            }
            $scope.updateTotalCost = function () {
                $scope.totalCost = {
                    OriginalCost: 0,
                    CurrentCost: 0,
                    NewCost: 0
                }
                angular.forEach($scope.rowCollection, function (item) {
                    $scope.totalCost.OriginalCost += item.OriginalCost;
                    $scope.totalCost.CurrentCost += item.CurrentCost;
                    $scope.totalCost.NewCost += item.NewCost;
                });
            }
            $scope.selectRow = function (row) {
                $scope.selectedRow = row;
                 
            }
            $scope.onMouseLeave = function (row) {
                $scope.selectedRow = null;
            }

            // Simple GET request example:
            $http({
                method: 'GET',
                url: serviceBasePath + "Request/CustomCostOverhead/" + $scope.params.Trend.TrendID
            }).then(function successCallback(response) {
                console.log(response);
                $scope.rowCollection = response.data.result;
                $scope.updateTotalCost();
                angular.forEach($scope.rowCollection, function (item) {
                    if (item.Justification != null) {
                        $scope.Justification = item.Justification;
                    }
                });
            }, function errorCallback(response) {
                console.log(response);
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });


            $scope.saveChanges = function () {
                $('#btnSaveCostOverhead').prop('disabled', true); //Aditya
                //alert($scope.rowCollection[0].CurrentMarkup);
                var cnt = 0;
                angular.forEach($scope.rowCollection, function (item) {
                    //if (item.CurrentMarkup == 0) {
                    if (isNaN(item.CurrentMarkup) || Number.parseFloat(item.CurrentMarkup) < 1) {  //Manasi 23-07-2020
                        cnt++;
                    }
                });
                if (cnt > 0) {
                    $('#btnSaveCostOverhead').prop('disabled', false); //Aditya
                    dhtmlx.alert("Markup must be a valid number greater than or equal 1");  //Manasi 22-07-2020
                    return;
                }
                if (!$scope.Justification) {
                    $('#btnSaveCostOverhead').prop('disabled', false); //Aditya
                    dhtmlx.alert("Enter Justification.");
                    return;
                }
                angular.forEach($scope.rowCollection, function (item) {
                    item.Justification = $scope.Justification;
                });
                var config = {
                    method : "POST",
                    url: serviceBasePath + "Response/CustomCostOverhead",
                    data : $scope.rowCollection
                }

                $http(config)
                    .then(function success(response) {
                        var message = {
                            text: response.data.result,
                            width: "20%"
                        }
                        $('#btnSaveCostOverhead').prop('disabled', false); //Aditya
                        dhtmlx.alert(message);
                        $rootScope.modalInstance.close("reload");
                    }, function error() {
                        $('#btnSaveCostOverhead').prop('disabled', false); //Aditya
                    });
             }   
            
        }]);