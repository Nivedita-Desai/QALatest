angular.module('cpp.controllers').
    controller('CostRowDetailCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'CostRow',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, CostRow) {

            $('.modal-backdrop').hide();
            var dtFormat = "DD/MM/YYYY";
            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            $scope.RoundToTwo = function (number) {
                if (!isNaN(number))
                    return +(Math.round(number + "e+2") + "e-2");
            }

            $scope.materialCategory = {};
            var url = serviceBasePath + 'response/MaterialCategory/';
            var requestData = {
                ActivityID: $scope.params.ActivityID,
                Granularity: $scope.params.Granularity,
                LineID: $scope.params.LineID,
                CostType: $scope.params.CostType
            }
            CostRow.lookup().get(requestData, function success(response) {
                var temp = response.result[0].OriginalCost;
                var originalCellValue = response.result[0].CellValue;
                //Manasi 05-08-2020
                //response.result[0].OriginalCost = response.result[0].TotalCost;
                //response.result[0].CellValue = response.result[0].TotalCost;
                //response.result[0].TotalCost = temp;
                console.log(response);
                $scope.data = response.result;

                $scope.isLabor = ($scope.data[0].CostType == "F" || $scope.data[0].CostType == "FTE") || false;

                if ($scope.isLabor) {
                    //response.result[0].CellValue = response.result[0].FTEHours / 5;
                    response.result[0].CellValue = response.result[0].CellValue;
                }

                if ($scope.data[0].CostType == "U") {
                    response.result[0].CellValue = originalCellValue;
                }
               // response.result[0].CellValue = parseFloat(response.result[0].CellValue).toFixed(2);
                $scope.data = response.result;
                var id = 1;
                angular.forEach($scope.data, function (item) {
                    var cellVal = item.CellValue;
                    item.StartDate = moment(item.StartDate).format(dtFormat);
                    item.EndDate = moment(item.EndDate).format(dtFormat);
                    item.ID = id;

                    //-----------------Manasi 19-08-2020--------------------------------------------------------------------
                    item.CellValue = parseFloat(cellVal).toFixed(2);

                    if (item.CostType == "FTE" || item.CostType == "F") {
                        //item.FTEHours = item.CellValue * 5;
                        item.TotalCost = item.FTEHours * 8 * item.OriginalRate;
                        item.TotalCost = item.TotalCost.toFixed(2);

                    } else if (item.CostType == "L" || item.CostType == "ODC") {
                        item.TotalCost = parseFloat(item.CellValue * $scope.params.CUSTOM_OVERHEAD_RATE).toFixed(2);
                    } else if (item.CostType == "U") {
                        item.TotalCost = parseFloat(item.CellValue * item.OriginalRate).toFixed(2);
                    }
                    //------------------------------------------------------------------------------------------------------------

                    if ($scope.params.trend.TrendNumber == "1000") {
                        item.isActual = false; item.isETC = false;
                        if (moment(item.EndDate).isBefore(moment()))
                            item.isActual = true;
                        else
                            item.isETC = true;
                    }


                    id++;
                });
                $scope.calculateTotal();
            }, function error(response) {
                dhtmlx.alert("Unable to retrive costs.");
                console.log(response);
            });

            $scope.CalcualteTotalCost = function (row) {
                if (row.CostType == "FTE" || row.CostType == "F") {
                    //row.FTEHours = row.CellValue * 5;
                    //row.TotalCost =  row.FTEHours * 8 * row.BaseRate ; //1 - CustomCostOverhead //1- CostOverhead
                    row.TotalCost = row.FTEHours * 8 * row.OriginalRate; //1 - CustomCostOverhead //1- CostOverhead     //Manasi 05-08-2020
                    row.TotalCost = row.TotalCost.toFixed(2);

                } else if (row.CostType == "L" || row.CostType == "ODC") {
                    row.TotalCost = row.CellValue * $scope.params.CUSTOM_OVERHEAD_RATE; //* $scope.params.OVERHEAD_RATE
                } else if (row.CostType == "U") {
                    //row.TotalCost = row.CellValue * row.BaseRate  * $scope.params.CUSTOM_OVERHEAD_RATE;
                    row.TotalCost = row.CellValue * row.OriginalRate // * $scope.params.CUSTOM_OVERHEAD_RATE;    //Manasi 05-08-2020
                }
                $scope.calculateTotal();
            }

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }
            $scope.selectRow = function (row) {
                $scope.selectedRow = row;

            }
            $scope.onMouseLeave = function (row) {
                $scope.selectedRow = null;
            }
            //When clicked on save button
            $scope.save = function () {
                CostRow.persist().save($scope.data)
                    .$promise.then(function sucess(response) {
                        console.log(response);
                        if (response.result == "success") {
                            //dhtmlx.alert(response.result);
                        } else {
                            //dhtmlx.alert('No changes saved');
                        }
                        $scope.goBack($scope.data);
                    }, function error(response) {
                        dhtmxl.alert("Unable to save costs.");
                    });

            }
            $scope.calculateTotal = function () {
                var total = 0;
                angular.forEach($scope.data, function (item) {
                    if (!item.isActual)
                        //total = total + parseFloat(item.TotalCost);
                        //Manasi 05-08-2020
                        if (item.CostType == "FTE")
                            total = parseFloat(total) + parseFloat(parseFloat(item.OriginalRate) * parseFloat(parseFloat(item.FTEHours) * 8));
                        else if (item.CostType == "L")
                            total = parseFloat(total) + parseFloat(item.OriginalCost);
                        else if (item.CostType == "ODC")
                            total = parseFloat(total) + parseFloat(item.OriginalCost);
                        else if (item.CostType == "U")
                            total = parseFloat(total) + parseFloat(parseFloat(item.OriginalRate) * parseFloat(item.CellValue));
                });

                var countDecimals = function (value) {
                    if (Math.floor(value) === value) return 0;
                    return value.toString().split(".")[1].length || 0;
                }
                var DecimalCount = countDecimals(total);
                if (DecimalCount => 3) {
                    total = total.toFixed(3);
                }
              //  alert("Three: ");
               // alert(total + " -- After Round : " + $scope.RoundToTwo(total));
                $scope.total = parseFloat(total).toFixed(2);
            }
        }
    ]);