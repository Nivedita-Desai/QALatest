angular.module('cpp.controllers').
    controller('ApproveRejectPOModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'CostRow', 'CostType', 'Employee', 'AllEmployee',
        'PurchaseOrder', 'PurchaseOrderDetail', '$location',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, CostRow, CostType, Employee, AllEmployee
            , PurchaseOrder, PurchaseOrderDetail, $location) {

            $('.modal-backdrop').hide();

            $('[data-toggle="tooltip"]').tooltip();

            var dtFormat = "MM/DD/YYYY";
            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            console.log($scope.params);
            $scope.balanceerror = false;
            //Added by Amruta 27082021-- for disable the buttons post approve or rejection
            $scope.viewPO = false;
            $scope.costTypeFilters = [];
            $scope.purchaseOrder = {};
            $scope.purchaseOrder.Reason = "";
            $scope.purchaseOrder = $scope.params.purchaseOrder;
            
            //$scope.organizationID = $scope.params.organizationID;
            $scope.purchaseOrderDetails = $scope.params.purchaseOrderDetails;
            $scope.purchaseOrderDetails = $scope.params.purchaseOrderDetails;

            console.log("Purchase Order check===>");
            console.log($scope.params.purchaseOrder);
            console.log($scope.params.purchaseOrderDetails);

            if ($scope.params.edit)
                $scope.edit = true;
            else
                $scope.edit = false;
            $scope.total = 0.0;

            for (var podetail in $scope.purchaseOrderDetails) {
                if ($scope.purchaseOrderDetails[podetail].TCost > 0)
                    $scope.total = $scope.total + $scope.purchaseOrderDetails[podetail].TCost;
            }

            //Added by Amruta 27082021-- for disable the buttons post approve or rejection
            //for (var poOrder in $scope.purchaseOrder) {
            if ($scope.purchaseOrder.Status != 'Created')
                $scope.viewPO = true;
            //}
            console.log('Total: ' + $scope.total);

            $scope.selectRow = function (row) {
                $scope.selectedRow = row;
            }
            $scope.onMouseLeave = function (row) {
                $scope.selectedRow = null;
            }
            $scope.goBack = function (param) {
                $location.path('/app/po-Approval/');
                $scope.$close(param);
            }

            CostType.get({}, function (response) {
                console.log("Get cost type");
                $scope.costTypeCollection = response.result;
                // $scope.gridOptions.columnDefs[1].editDropdownOptionsArray = $scope.costTypeCollection;
                console.log(response.result);


                for (var x = 0; x < $scope.costTypeCollection.length; x++) {
                    var temp = {};
                    if ($scope.costTypeCollection[x].ID != 1) {
                        temp.value = $scope.costTypeCollection[x].Type;
                        temp.id = $scope.costTypeCollection[x].ID;
                        $scope.costTypeFilters.push($scope.costTypeCollection[x]);
                    }
                }
                console.log($scope.costTypeFilters);
                $scope.CostTypeList = $scope.costTypeFilters;
                console.log("Data==");
                console.log($scope.CostTypeList);
            });
            console.log("Employee Org Id List");
            console.log($scope.organizationID);

            //Modified by Amruta 27082021-- for viewing the selected employee
            AllEmployee.get({}, function (EmployeesData) {
                console.log(EmployeesData);
                console.log("Employee List");
                $scope.employeesList = EmployeesData.result;
                console.log($scope.employeesList);

                for (var po in $scope.purchaseOrderDetails) {
                    if ($scope.purchaseOrderDetails[po].CostType == 'ODC') {
                        $scope.purchaseOrderDetails[po].emplist = [];
                        for (var emplp in $scope.purchaseOrderDetails[po].EmployeeID) {
                            $scope.purchaseOrderDetails[po].emplist.push($scope.employeesList.find(e => e.ID == $scope.purchaseOrderDetails[po].EmployeeID[emplp]));
                        }
                    }
                }

            });
            $scope.setEmployee = function (empID, rowdata) {
                for (var chk in $scope.purchaseOrderDetails) {
                    if (rowdata.CostLineItemID == $scope.purchaseOrderDetails[chk].CostLineItemID)
                        $scope.purchaseOrderDetails[chk].EmployeeID = empID;
                }
            }

            //$scope.Approve = function () {

            //    alert('Approve');
            //};

            //$scope.Reject = function () {

            //    alert('Reject');
            //};


            $scope.CostTypeList = [
                {
                    name: 'All',
                    value: "0",
                    id: "0"
                }, {
                    name: 'Labor',
                    value: "F",
                    id: "F"
                }, {
                    //name: 'Contractor',
                    name: 'Subcontractor ',  //Manasi 17-07-2020
                    value: "L",
                    id: "L"
                }, {
                    name: 'Material',
                    value: "U",
                    id: "U"
                }, {
                    name: "ODC",
                    value: "ODC",
                    id: "ODC"
                }];

            $scope.calculateprice = function (POdtl) {
                $scope.balanceerror = false;
                console.log('Checking range..');
                console.log(POdtl);
                $scope.total = 0;

                //To update Total Cost and Sum Total
                for (var dtlp in $scope.purchaseOrderDetails) {
                    if (POdtl.CostLineItemID == $scope.purchaseOrderDetails[dtlp].CostLineItemID)
                        $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity = POdtl.RequestedAmountOrQuantity;
                    if ($scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity != undefined || $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity != 0) {
                        var qtyprice = parseFloat($scope.purchaseOrderDetails[dtlp].UnitPrice);
                        if (isNaN(qtyprice))
                            qtyprice = 0;
                        $scope.purchaseOrderDetails[dtlp].TCost = ($scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity) * qtyprice;
                    }
                    else
                        $scope.purchaseOrderDetails[dtl].TCost = 0;
                    $scope.total = $scope.total + $scope.purchaseOrderDetails[dtlp].TCost;
                    console.log('Qty. Cost --' + $scope.purchaseOrderDetails[dtlp].TCost);
                }

                console.log('Total Cost--' + $scope.total);

                // To display error 
                for (var dtl in $scope.purchaseOrderDetails) {

                    if ($scope.purchaseOrderDetails[dtl].BalancedAmountOrQuantity < ($scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity) || $scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity == undefined) {

                        $scope.balanceerror = true;
                        $scope.curcostitemID = $scope.purchaseOrderDetails[dtl].CostLineItemID;
                        console.log('Error:' + $scope.curcostitemID);
                        break;

                    }
                }
            }


            $scope.Approve = function () {
                var purchaseOrderList = [];

                angular.forEach($scope.purchaseOrderDetails, function (item) {
                    if (item.RequestedAmountOrQuantity > 0)
                        purchaseOrderList.push(item);
                });
                console.log('PurchaseOrderList-->');
                console.log(purchaseOrderList);
                var data = {
                    purchaseOrder: $scope.purchaseOrder,
                    purchaseOrderDetails: purchaseOrderList
                }

                data.purchaseOrder.operation = "update";
                data.purchaseOrder.Status = "Approved";
                //data.purchaseOrder.PurchaseOrderNumber = "";
                //data.purchaseOrder.PurchaseOrderBaseNumber = "";
                console.log('Purchase Order Details-->');
                console.log(data);

                PurchaseOrderDetail.persist().save(data)
                    .$promise.then(function sucess(response) {

                        /*dhtmlx.alert('Purchase Order :' + ' ' + response.result + '\n' + 'has been Approved.');*/
                        dhtmlx.alert({
                            text: 'Purchase Order :' + ' ' + response.result + '\n' + 'has been Approved.',
                            callback: function () {
                                $location.path('/app/po-Approval/');
                                window.location.reload();}
                        });
                      //  $location.path('/app/po-Approval/');
                    }, function error(response) {

                    });

                $scope.goBack('close');

                /* TO UPDATE THE PURCHASE ORDER DETAILS ---
                PurchaseOrderDetail.persist().save(data)
                    .$promise.then(function sucess(response) {
                        console.log(response);

                        var baseUrl = null;
                        baseUrl = serviceBasePath + 'Request/PurchaseOrderDetailReport';

                        var excelUrl = baseUrl
                            + '?PurchaseOrderNumber=' + response.result
                            + '&FileType=' + 'excel';

                        downloadAsExcel(excelUrl, 'PurchaseOrder_' + response.result);

                    }, function error(response) {

                    }); 
                    */



            }

            $scope.Reject = function () {
                console.log("In reject function==>");
                console.log($scope.purchaseOrder.Reason);

                var purchaseOrderList = [];
                //Added by Amruta 27082021-- for justification mandatory on rejection
                if (!$scope.purchaseOrder.Reason) {
                    dhtmlx.alert('Please provide a Justification for the Rejection of PO.');
                }
                else {
                    angular.forEach($scope.purchaseOrderDetails, function (item) {
                        if (item.RequestedAmountOrQuantity > 0)
                            purchaseOrderList.push(item);
                    });
                    console.log('PurchaseOrderList-->');
                    console.log(purchaseOrderList);
                    var data = {
                        purchaseOrder: $scope.purchaseOrder,
                        purchaseOrderDetails: purchaseOrderList
                    }

                    data.purchaseOrder.operation = "update";
                    data.purchaseOrder.Status = "Rejected";
                    //data.purchaseOrder.PurchaseOrderNumber = "";
                    //data.purchaseOrder.PurchaseOrderBaseNumber = "";
                    console.log('Purchase Order Details-->');
                    console.log(data);

                    PurchaseOrderDetail.persist().save(data)
                        .$promise.then(function sucess(response) {

                            dhtmlx.alert({
                                text: 'Purchase Order :' + ' ' + response.result + '\n' + 'has been Rejected.',
                                callback: function () {
                                    $location.path('/app/po-Approval/');
                                    window.location.reload();
                                }
                            });

                        }, function error(response) {

                        });

                    $scope.goBack('close');
                }





            }

            function downloadAsExcel(excelUrl, fileName) {
                console.log('downloading as excel');

                $http.get(excelUrl)
                    .then(function success(response) {
                        console.log(response);
                        var dedcodedRaw = atob(response.data);
                        console.log('attempt excel blob');

                        var array = new Uint8Array(dedcodedRaw.length);

                        for (i = 0; i < dedcodedRaw.length; i++) {
                            array[i] = dedcodedRaw.charCodeAt(i);
                        }

                        var blobtest = new Blob([array], {
                            //type: 'application/vnd.ms-excel;charset=charset=utf-8'
                        });

                        //saveAs comes from FileSaver.js, faster method of saving files
                        saveAs(blobtest, fileName + '.xls');

                    }, function error(response) {
                        console.log(response);
                    });
            }

        }
    ]);
