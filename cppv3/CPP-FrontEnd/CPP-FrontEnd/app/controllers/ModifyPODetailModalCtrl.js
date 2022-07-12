angular.module('cpp.controllers').
    controller('ModifyPODetailModalCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', '$uibModalInstance', 'CostRow', 'CostType', 'Employee',
        'PurchaseOrder', 'PurchaseOrderDetail',
        function ($scope, $timeout, $uibModal, $rootScope, $http, $uibModalInstance, CostRow, CostType, Employee
            , PurchaseOrder, PurchaseOrderDetail) {



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
            $scope.costTypeFilters = [];
            $scope.purchaseOrder = $scope.params.purchaseOrder;
            $scope.purchaseOrder.vendorDescription = "";
            $scope.organizationID = $scope.params.organizationID;
            $scope.purchaseOrderDetails = $scope.params.purchaseOrderDetails;
            $scope.purchaseOrderDetailsData = angular.copy($scope.params.purchaseOrderDetails);
            $scope.purchaseOrderData = angular.copy($scope.params.purchaseOrder);
            if ($scope.params.edit)
                $scope.edit = true;
            else
                $scope.edit = false;
            $scope.total = 0.0;
            
            for (var poindx in $scope.purchaseOrderDetails) {
                if ($scope.purchaseOrderDetails[poindx].CostType == 'ODC') {
                    $scope.purchaseOrderDetails[poindx].EmployeeIDlist = [];
                    for (var empindx in $scope.purchaseOrderDetails[poindx].EmployeeID) {
                        $scope.purchaseOrderDetails[poindx].EmployeeIDlist.push({ "id": $scope.purchaseOrderDetails[poindx].EmployeeID[empindx]});
                    }
                }
            }

            $scope.example14settings = {
                scrollableHeight: '100px', scrollable: true,
                enableSearch: false
            };

            console.log('PO LISt wla data-->');
            console.log($scope.purchaseOrderDetails);

            $scope.purchaseOrderDetails22 = [{
                "CostType": "U",
                "ProjectElementId": 3,
                "PhaseDescription": "  Manage Quality and Risk",
                "BudgetCategory": "Access Control",
                "BudgetSubCategory": "Test Access Control",
                "CostLineItemID": "16210030301002810308",
                "Name": "DUCT DETECTOR",
                "Description": "DETECTOR - DUCT DETECTOR",
                "UniqueIdentityNumber": "BM00153",
                "UnitOfMeasurement": "Each",
                "UnitPrice": "167.99",
                "ClassRefFullName": "16.Construction",
                "CustomerRefFullName": "BC00002.Los Angeles Worldwide Airport:16.Construction:003.TestProj:01.testElement:00.2810.0001",
                "PurchaseOrderID": 13,
                "PurchaseOrderNumber": "P003010006",
                "EmployeeID": [0],
                "EmployeeName": "",
                "AmountOrQuantity": 126,
                "UtilizedAmountOrQuantity": 126,
                "BalancedAmountOrQuantity": 1,
                "RequestedAmountOrQuantity": 1,
                "TotalCost": 21167.53,
                "TCost": 167.99
            },
                {
                    "CostType": "U",
                    "ProjectElementId": 3,
                    "PhaseDescription": " Schedule",
                    "BudgetCategory": "Access Control System Hardware",
                    "BudgetSubCategory": "Access Control Network Controllers",
                    "CostLineItemID": "16210030301042814303",
                    "Name": "Material 100",
                    "Description": "Combiner - Test Material",
                    "UniqueIdentityNumber": "BM01040",
                    "UnitOfMeasurement": "Each",
                    "UnitPrice": "225",
                    "ClassRefFullName": "16.Construction",
                    "CustomerRefFullName": "BC00002.Los Angeles Worldwide Airport:16.Construction:003.TestProj:01.testElement:04.2814.1100",
                    "PurchaseOrderID": 13,
                    "PurchaseOrderNumber": "P003010006",
                    "EmployeeID": [0],
                    "EmployeeName": "",
                    "AmountOrQuantity": 51,
                    "UtilizedAmountOrQuantity": 49,
                    "BalancedAmountOrQuantity": 22,
                    "RequestedAmountOrQuantity": 20,
                    "TotalCost": 11475,
                    "TCost": 4500
                },
                {
                    "CostType": "L",
                    "ProjectElementId": 3,
                    "PhaseDescription": "  Manage Quality and Risk",
                    "BudgetCategory": "Access Control",
                    "BudgetSubCategory": "Test Access Control",
                    "CostLineItemID": "16210030301002810403",
                    "Name": "Other",
                    "Description": "Cyber security data protection/encryption providers - Other",
                    "UniqueIdentityNumber": "BS00015",
                    "UnitOfMeasurement": "",
                    "UnitPrice": "",
                    "ClassRefFullName": "16.Construction",
                    "CustomerRefFullName": "BC00002.Los Angeles Worldwide Airport:16.Construction:003.TestProj:01.testElement:00.2810.0001",
                    "PurchaseOrderID": 13,
                    "PurchaseOrderNumber": "P003010006",
                    "EmployeeID": [0],
                    "EmployeeName": "",
                    "AmountOrQuantity": 11500,
                    "UtilizedAmountOrQuantity": 0,
                    "BalancedAmountOrQuantity": 11500,
                    "RequestedAmountOrQuantity": 0,
                    "TotalCost": 11500,
                    "TCost": 0
                },
                {
                    "CostType": "L",
                    "ProjectElementId": 3,
                    "PhaseDescription": " Schedule",
                    "BudgetCategory": "Access Control System Hardware",
                    "BudgetSubCategory": "Access Control Network Controllers",
                    "CostLineItemID": "16210030301042814401",
                    "Name": "Solar Turbine",
                    "Description": "Consulting, Control System, Energy Management and Optimization - cogen instrumentation and control system",
                    "UniqueIdentityNumber": "BS00007",
                    "UnitOfMeasurement": "",
                    "UnitPrice": "",
                    "ClassRefFullName": "16.Construction",
                    "CustomerRefFullName": "BC00002.Los Angeles Worldwide Airport:16.Construction:003.TestProj:01.testElement:04.2814.1100",
                    "PurchaseOrderID": 13,
                    "PurchaseOrderNumber": "P003010006",
                    "EmployeeID": [0],
                    "EmployeeName": "",
                    "AmountOrQuantity": 2760,
                    "UtilizedAmountOrQuantity": 0,
                    "BalancedAmountOrQuantity": 2760,
                    "RequestedAmountOrQuantity": 0,
                    "TotalCost": 2760,
                    "TCost": 0
                },
                {
                    "CostType": "ODC",
                    "ProjectElementId": 3,
                    "PhaseDescription": " Schedule",
                    "BudgetCategory": "Access Control System Hardware",
                    "BudgetSubCategory": "Access Control Network Controllers",
                    "CostLineItemID": "16210030301042814202",
                    "Name": "Ferry",
                    "Description": "By sea",
                    "UniqueIdentityNumber": "BO00005",
                    "UnitOfMeasurement": "",
                    "UnitPrice": "",
                    "ClassRefFullName": "16.Construction",
                    "CustomerRefFullName": "BC00002.Los Angeles Worldwide Airport:16.Construction:003.TestProj:01.testElement:04.2814.1100",
                    "PurchaseOrderID": 13,
                    "PurchaseOrderNumber": "P003010006",
                    "EmployeeID": [0],
                    "EmployeeName": "",
                    "AmountOrQuantity": 126,
                    "UtilizedAmountOrQuantity": 105,
                    "BalancedAmountOrQuantity": 63,
                    "RequestedAmountOrQuantity": 42,
                    "TotalCost": 126,
                    "TCost": 42
                }];

            for (var podetail in $scope.purchaseOrderDetails) {
                if ($scope.purchaseOrderDetails[podetail].TCost > 0)
                    $scope.total = $scope.total + $scope.purchaseOrderDetails[podetail].TCost;
            }
            console.log('Total: ' + $scope.total);

            $scope.selectRow = function (row) {
                $scope.selectedRow = row;
            }
            $scope.onMouseLeave = function (row) {
                $scope.selectedRow = null;
            }
            $scope.goBack = function (param) {
                checkFields();
                // $scope.$close(param);
            }
            
            function checkFields() {
             
                // var filteredList = [];
                var isChanges = false;

                angular.forEach($scope.purchaseOrderDetailsData, function (item) {
                    var filteredList = [];
                    for (var x = 0; x < $scope.purchaseOrderDetails.length; x++) {

                        if ($scope.purchaseOrderDetails[x].CostLineItemID == item.CostLineItemID) {
                            if ($scope.purchaseOrderDetails[x].RequestedAmountOrQuantity != item.RequestedAmountOrQuantity) {
                                isChanges = true;
                            }
                          
                            if ($scope.purchaseOrderDetails[x].CostType == 'ODC') {
                                for (var y = 0; y < $scope.purchaseOrderDetails[x].EmployeeID.length; y++) {
                                    if ($scope.purchaseOrderDetails[x].EmployeeID[y] != item.EmployeeID[y]) {
                                        console.log("Employee ID==>");
                                        console.log(item.EmployeeID[y]);
                                        console.log($scope.purchaseOrderDetails[x].EmployeeID[y]);
                                        isChanges = true;
                                    }
                                    

                                }


                            }
                        }
                    }

                    
                });

                if ($scope.purchaseOrderData.Description != $scope.purchaseOrder.Description) {
                    isChanges = true;
                }

                if (isChanges) {

                    dhtmlx.confirm("Unsaved data will be lost. Want to Continue?", function (result) {
                        if (result) {
                            if (result) {
                                $scope.$close('close');
                            } else {
                                return;
                            }
                            isChanges = false;
                        }
                    });


                } else {
                    $scope.$close('close');
                }


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
            Employee.get({ OrganizationID: $scope.organizationID }, function (EmployeesData) {
                //$scope.newEmployees = [];
               
                console.log("Employee List");
                $scope.employeesList = EmployeesData.result;
                console.log($scope.employeesList);
                $scope.empdata = [];          
                for (var empno in $scope.employeesList) {
                    $scope.empdata.push({
                        "id": $scope.employeesList[empno].ID, "label": $scope.employeesList[empno].Name
                    });

                }
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
                console.log("Set Employee==>");
                console.log(empID);
                console.log(rowdata);
                for (var chk in $scope.purchaseOrderDetails) {
                    if (rowdata.CostLineItemID == $scope.purchaseOrderDetails[chk].CostLineItemID)
                        $scope.purchaseOrderDetails[chk].EmployeeID = empID;
                }
            }


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
                    if (POdtl.CostLineItemID == $scope.purchaseOrderDetails[dtlp].CostLineItemID) {
                        if (POdtl.RequestedAmountOrQuantity == '')
                            $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity = 0;
                        else
                            $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity = POdtl.RequestedAmountOrQuantity;
                    }

                    if (($scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity != undefined || $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity != 0)
                        && !($scope.purchaseOrderDetails[dtlp].BalancedAmountOrQuantity < $scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity)) {

                        var qtyprice = parseFloat($scope.purchaseOrderDetails[dtlp].UnitPrice);
                        if (isNaN(qtyprice))
                            qtyprice = 1;
                        $scope.purchaseOrderDetails[dtlp].TCost = ($scope.purchaseOrderDetails[dtlp].RequestedAmountOrQuantity) * qtyprice;
                    }
                    else
                        $scope.purchaseOrderDetails[dtlp].TCost = 0;
                    $scope.total = $scope.total + $scope.purchaseOrderDetails[dtlp].TCost;
                    console.log('Qty. Cost --' + $scope.purchaseOrderDetails[dtlp].TCost);
                }

                console.log('Total Cost--' + $scope.total);

                // To display error 
                for (var dtl in $scope.purchaseOrderDetails) {
                    // if (POdtl.CostLineItemID == $scope.purchaseOrderDetails[dtl].CostLineItemID) {
                    if ($scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity == undefined) {
                        console.log("undefined==>" + $scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity)
                        $scope.balanceerror = false;

                    }

                    if ($scope.purchaseOrderDetails[dtl].BalancedAmountOrQuantity < ($scope.purchaseOrderDetails[dtl].RequestedAmountOrQuantity)) {

                        $scope.balanceerror = true;
                        $scope.curcostitemID = $scope.purchaseOrderDetails[dtl].CostLineItemID;
                        dhtmlx.alert("Requested Quantity cannot be more than available quantity for Cost Code:" + $scope.curcostitemID);
                        console.log('Error:' + $scope.curcostitemID);
                        break;

                    }
                }
                //   }



            }


            $scope.save = function () {
                var purchaseOrderList = [];
                
                angular.forEach($scope.purchaseOrderDetails, function (item) {
                    if (item.RequestedAmountOrQuantity > 0 && item.RequestedAmountOrQuantity != undefined) {
                        if (item.CostType == 'ODC') {
                            item.EmployeeID = [];
                            for (var empno in item.EmployeeIDlist)
                                item.EmployeeID.push(item.EmployeeIDlist[empno].id);

                        }

                        purchaseOrderList.push(item);
                    }
                });
                console.log('PurchaseOrderList-->');
                console.log(purchaseOrderList);
                var data = {
                    purchaseOrder: $scope.purchaseOrder,
                    purchaseOrderDetails: purchaseOrderList
                }

                data.purchaseOrder.operation = "update";
                //data.purchaseOrder.PurchaseOrderNumber = "";
                //data.purchaseOrder.PurchaseOrderBaseNumber = "";
                console.log('Purchase Order Details-->');
                console.log(data);


                if (data.purchaseOrder.Status = "Rejected") {
                    data.purchaseOrder.Status = "Created";
                }




               

                PurchaseOrderDetail.persist().save(data)
                    .$promise.then(function sucess(response) {
                        console.log("Response result data==>");
                        console.log(response.result);
                        dhtmlx.alert('Purchase Order :' + ' ' + purchaseOrderList[0].PurchaseOrderNumber + '\n' + 'has been updated successfully.');

                    }, function error(response) {

                    });
                $scope.$close('close');
                $state.reload();
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