angular.module('cpp.controllers').
    controller('DocumentTypeCtrl', ['$http', 'Page', '$state', 'UpdateDocumentType', '$uibModal', 'DocumentType', '$scope', '$rootScope', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
    function ($http, Page, $state, UpdateDocumentType, $uibModal, DocumentType, $scope, $rootScope, ProjectTitle, TrendStatus, $location, $timeout) {
        var okToExit = true;
        Page.setTitle('Document Types');
        ProjectTitle.setTitle('');
        TrendStatus.setStatus('');
        $scope.checkedRow = [];

        $scope.gridOPtions = {};
        $scope.myExternalScope = $scope;

        console.log('luan test');

        var url = serviceBasePath + 'response/DocumentType/';
        DocumentType.get({}, function (response) {
            console.log(response)
            $scope.checkList = [];
            $scope.DocumentTypeCollection = response.result;
            console.log($scope.DocumentTypeCollection);
            $scope.orgDocumentTypeCollection = angular.copy(response.result);
            addIndex($scope.DocumentTypeCollection);
            angular.forEach($scope.DocumentTypeCollection, function (item, index) {
                $scope.checkList[index + 1] = false;
                item.checkbox = false;
            });
            console.log($scope.DocumentTypeCollection);
            $scope.gridOptions.data = $scope.DocumentTypeCollection;
        });
        var addIndex = function (data) {
            var i = 1;
            angular.forEach(data, function (value, key, obj) {
                value.displayId = i;
                i = i + 1;
                if (value.Schedule === "0001-01-01T00:00:00") {
                    value.Schedule = "";
                }
            });
        }
        $scope.cellInputEditableTemplate = '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
        $scope.cellSelectEditableTemplate = '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id.PositionDescription for id in positionCollection track by id.PositionID" ng-blur="updateEntity(row)" />';
        $scope.cellCheckEditTableTemplate = '<input class="c-approval-matrix-check"  type="checkbox" ng-input="COL-FIELD" ng-model="COL_FIELD"/>';
        $scope.cellSelect = '<select ng-model="row.entity.text" data-ng-options="d as d.id for d in tempList">';
        $scope.addRow = function () {
            var x = Math.max.apply(Math, $scope.DocumentTypeCollection.map(function (o) {

                return o.displayId;
            }))

            if (x < 0) {
                console.log(x);
                x = 0;
            }

            $scope.checkList[++x] = false;
            $scope.DocumentTypeCollection.splice(x, 0, {
                displayId: x,
                DocumentTypeName: '',
                DocumentTypeDescription: '',
                checkbox: false,
                new: true
            });

            $scope.gridApi.core.clearAllFilters();//Nivedita-T on 17/11/2021
            $timeout(function () {
                console.log($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
                
                $scope.gridApi.core.scrollTo($scope.gridOptions.data[$scope.gridOptions.data.length - 1], $scope.gridOptions.columnDefs[0]);
            }, 1);
        }


        $scope.gridOptions = {
            enableColumnMenus: false,
            enableCellEditOnFocus: true,
            enableFiltering: true,
            //enableRowSelection:false,
            //enableCellSelection: true,
            //selectedItems: $scope.mySelections,

            //multiSelect: false,
            rowHeight:40,
            width: 400,
            //afterSelectionChange: function (rowItem, event) {
            //    console.log($scope.mySelections);
            //    $scope.selectedIDs = [];
            //    console.log(rowItem);
            //    angular.forEach($scope.mySelections, function ( item ) {
            //        $scope.selectedIDs.push( item.id )
            //    });
            //
            //},
            columnDefs: [
                {
                    field: 'displayId',
                    name: 'ID',
                    enableCellEdit: false,
                    //cellClass: 'c-col-id',
                    width: 50,
                    cellClass: 'c-col-Num' //Manasi

                    //cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'

                }
                ,
                {
                    field: 'DocumentTypeName',
                    name: 'Type*',
                    enableCellEditOnFocus: true,
                    //   enableCellEdit :true,
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    // cellFilter : 'mapStatus'

                }, {
                    field: 'DocumentTypeDescription',
                    name: 'Description*',

                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                },
                {
                    field: 'checkBox',
                    name: '',
                    //
                    enableCellEdit: false,
                    enableFiltering: false,
                    width: 35,
                    cellTemplate: '<input type="checkbox" ng-model="checkList[row.entity.displayId]" class = "c-col-check" ng-click="grid.appScope.check(row,col)" style="text-align: center;vertical-align: middle;">'

                }
            ]
        }

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;

            $scope.gridApi.edit.on.beginCellEdit($scope, function (rowEntity, colDef) {
                $('div.ui-grid-cell form').find('input').css('height', '40px');
                $('div.ui-grid-cell form').find('select').css('height', '40px');
                $('div.ui-grid-cell form').css('margin', '0px');
                $('div.ui-grid-cell form').find('input').putCursorAtEnd();
                $('div.ui-grid-cell form').find('select').putCursorAtEnd();
                $('div.ui-grid-cell form').find('select').focus();
                //    .on("focus", function () { // could be on any event
                //    //$('div.ui-grid-cell form').find('input').putCursorAtEnd();
                //    //console.log($('div.ui-grid-cell form').find('input').putCursorAtEnd());
                //});
            });
        };

        $scope.cellClicked = function (row, col) {
            // do with that row and col what you want
            alert('hey!');
        }

        jQuery.fn.putCursorAtEnd = function () {
            console.log(this);
            return this.each(function () {
                //alert('we in there');
                // Cache references
                var $el = $(this),
                    el = this;

                // Only focus if input isn't already
                if (!$el.is(":focus")) {
                    $el.focus();
                }

                // If this function exists... (IE 9+)
                if (el.setSelectionRange) {

                    // Double the length because Opera is inconsistent about whether a carriage return is one character or two.
                    var ghettoLengthFix = 9999; //luan custom
                    var len = ghettoLengthFix * 2;

                    // Timeout seems to be required for Blink
                    setTimeout(function () {
                        el.setSelectionRange(len, len);
                        console.log('set to end');
                    }, 1);

                } else {
                    $el.focus();
                    // As a fallback, replace the contents with itself
                    // Doesn't work in Chrome, but Chrome supports setSelectionRange
                    $el.val($el.val());

                }

                // Scroll to the bottom, in case we're in a tall textarea
                // (Necessary for Firefox and Chrome)
                this.scrollTop = 999999;

            });

        };

        $scope.check = function (row, col) {
            if (row.entity.checkbox == false) {
                row.entity.checkbox = true;
                $scope.checkList[row.entity.displayId] = true;
                row.config.enableRowSelection = true;
            } else {
                $scope.checkList[row.entity.displayId] = false;
                row.entity.checkbox = false;
            }
            $scope.checkedRow.push(row);
        }
        $scope.clicked = function (row, col) {
            console.log(row);
            $scope.orgRow = row;
            $scope.col = col;
            $scope.row = row.entity;
            console.log(col);
        }
        $scope.save = function () {
            $("#save_document_type").attr("disabled", true);
            okToExit = false;
            var isReload = false;
            var isChanged = true;
            var isFilled = true;
            var listToSave = [];
            angular.forEach($scope.DocumentTypeCollection, function (value, key, obj) {

                if (value.DocumentTypeName == "" || value.DocumentTypeDescription == "") {
                    $("#save_document_type").attr("disabled", false);
                    dhtmlx.alert({
                        text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                        width: "400px"
                    });
                    okToExit = false;
                    isFilled = false;
                    return;
                }

                if (isFilled == false) {
                    return;
                }
                //New Item
                if (value.new === true) {
                    okToExit = false;
                    isReload = true;
                    var dataObj = {
                        Operation: '1',
                        DocumentTypeName: value.DocumentTypeName,
                        DocumentTypeDescription: value.DocumentTypeDescription

                    }
                    listToSave.push(dataObj);
                    okToExit = true;

                }
                else {//Update Item if there is changes
                    okToExit = false;
                    isChanged = true;
                    angular.forEach($scope.orgDocumentTypeCollection, function (orgItem) {
                        if (value.DocumentTypeID === orgItem.DocumentTypeID &&
                            value.DocumentTypeName === orgItem.DocumentTypeName &&
                            value.DocumentTypeDescription === orgItem.DocumentTypeDescription) {
                            isChanged = false;
                            okToExit = true;
                        }
                    });
                    if (isChanged == true) {
                        var dataObj = {
                            Operation: '2',
                            DocumentTypeName: value.DocumentTypeName,
                            DocumentTypeDescription: value.DocumentTypeDescription,
                            DocumentTypeID: value.DocumentTypeID

                        }
                    } else {
                        var dataObj = {
                            Operation: '4',
                            DocumentTypeName: value.DocumentTypeName,
                            DocumentTypeDescription: value.DocumentTypeDescription,
                            DocumentTypeID: value.DocumentTypeID

                        }
                    }
                    listToSave.push(dataObj)
                    okToExit = true;
                }


            });
            angular.forEach($scope.listToDelete, function (item) {
                listToSave.push(item);
            });
            if (isFilled == false) {
                return;
            }
            else {
                $http({
                    url: url,
                    //url: 'http://localhost:29986/api/response/phasecode',
                    method: "POST",
                    data: JSON.stringify(listToSave),
                    headers: { 'Content-Type': 'application/json' }
                }).then(function success(response) {
                    response.data.result.replace(/[\r]/g, '\n');
                    $("#save_document_type").attr("disabled", false);
                    if (response.data.result) {
                        $("#save_document_type").attr("disabled", false);
                        dhtmlx.alert(response.data.result);
                    } else {
                        $("#save_document_type").attr("disabled", false);
                        dhtmlx.alert('No changes to be saved.');
                    }

                    if (isTest == true) {
                        $("#save_document_type").attr("disabled", false);
                        //   var newUrl = $location.path();
                        console.log(newUrl);
                        // onRouteChangeOff();
                        window.location.href = "#" + newUrl;
                    }

                    $scope.orgDocumentTypeCollection = angular.copy($scope.DocumentTypeCollection);
                    console.log("Add new Succesfully");
                    okToExit = true;
                    $state.reload();
                }, function error(response) {
                    $("#save_document_type").attr("disabled", false);
                    dhtmlx.alert("Failed to save. Please contact your Administrator.");
                });
            }
            if (isReload == true || isChanged == true) {
                //$state.reload();
            }
        }
        $scope.delete = function () {
            //dhtmlx.alert("Deletion is not avaiable at this time.");
            //return;

            var isChecked = false;
            var unSavedChanges = false;
            var listToSave = [];
            var newList = [];
            var selectedRow = false;
            $scope.listToDelete = [];
            console.log($scope.DocumentTypeCollection);
            angular.forEach($scope.DocumentTypeCollection, function (item) {
                isChecked = false;
                if (item.checkbox == true) {
                    selectedRow = true;
                    if (item.new === true) {
                        unSavedChanges = true;
                        newList.push(item);

                    } else {
                        ischecked = true;
                        var dataObj = {
                            Operation: '3',
                            DocumentTypeName: item.DocumentTypeName,
                            DocumentTypeDescription: item.DocumentTypeDescription,
                            DocumentTypeID: item.DocumentTypeID,
                            displayId: item.displayId
                        }
                        listToSave.push(dataObj);
                        $scope.listToDelete.push(dataObj);
                       // dhtmlx.alert("Record Deleted.");
                    }
                }


            });
            if (!selectedRow) {
                dhtmlx.alert("Please select a record to delete.");
            }

            if (newList.length != 0) {
                for (var i = 0; i < newList.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.DocumentTypeCollection, function (item, index) {
                        if (item.displayId == newList[i].displayId) {
                            item.checkbox = false;

                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(newList[i].displayId, 1);
                        $scope.DocumentTypeCollection.splice(ind, 1);
                    }
                }

            }
            if (listToSave.length != 0) {

                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.DocumentTypeCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;

                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.DocumentTypeCollection.splice(ind, 1);
                    }
                }
            }

            //if(isChecked == true || unSavedChanges == true){
            //    angular.forEach(scope.DocumentTypeCollection,function(item){})
            //}else{
            //    alert("No row selected for delete");
            //}
            //$state.reload();        //Temporary Solution
        }
        $scope.checkForChanges = function () {
            var unSavedChanges = false;
            var originalCollection = $scope.orgDocumentTypeCollection;
            var currentCollection = $scope.DocumentTypeCollection;
            if (currentCollection.length != originalCollection.length) {
                unSavedChanges = true;
                return unSavedChanges;
            } else {
                angular.forEach(currentCollection, function (currentObject) {
                    for (var i = 0, len = originalCollection.length; i < len; i++) {
                        if (unSavedChanges) {
                            return unSavedChanges; // no need to look through the rest of the original array
                        }
                        if (originalCollection[i].DocumentTypeID == currentObject.DocumentTypeID) {
                            var originalObject = originalCollection[i];
                            // compare relevant data
                            if (originalObject.DocumentTypeName !== currentObject.DocumentTypeName ||
                                originalObject.DocumentTypeDescription !== currentObject.DocumentTypeDescription) {
                                // alert if a change has not been saved
                                unSavedChanges = true;
                                return unSavedChanges;
                            }
                            break; //no need to check any further, go to next object in new collection
                        }
                    }
                });
            }
            return unSavedChanges;
        };
        var isTest = false;
        var newUrl = "";
        onRouteChangeOff = $scope.$on('$locationChangeStart', function (event) {
            newUrl = $location.url();
            // alert(newUrl)
            if (!$scope.checkForChanges()) return;
            $scope.confirm = "";
            var scope = $rootScope.$new();
            scope.params = { confirm: $scope.confirm };
            $rootScope.modalInstance = $uibModal.open({
                scope: scope,
                templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                controller: 'exitConfirmation',
                size: 'md',
                backdrop: true
            });
            $rootScope.modalInstance.result.then(function (data) {
                console.log(scope.params.confirm);
                if (scope.params.confirm === "exit") {
                    onRouteChangeOff();
                    //alert(newUrl);
                    $location.path(newUrl);
                }
                else if (scope.params.confirm === "save") {
                    isTest = true;
                    $scope.save();
                    //alert("");
                    //if(okToExit){
                    //    onRouteChangeOff();
                    //    $location.path(newUrl);
                    //}
                    //onRouteChangeOff();
                    //$location.path(newUrl);
                }
                else if (scope.params.confirm === "back") {
                    //do nothing
                }
            });
            event.preventDefault();
        });

    }]);
