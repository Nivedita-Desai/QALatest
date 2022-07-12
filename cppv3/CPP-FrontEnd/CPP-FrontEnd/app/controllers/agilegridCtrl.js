angular.module('cpp.controllers').
    controller('AgilegridCtrl', ['$http', 'Page', '$state', 'UpdateLocation', '$uibModal', 'Location', '$scope', '$rootScope', 'ProjectTitle', 'TrendStatus', '$location', '$timeout',
    function ($http, Page, $state, UpdateLocation, $uibModal, Location, $scope, $rootScope, ProjectTitle, TrendStatus, $location, $timeout) {
        var okToExit = true;
        Page.setTitle('Location');
        ProjectTitle.setTitle('');
        TrendStatus.setStatus('');
        $scope.checkedRow = [];

        $scope.gridOPtions = {};
        $scope.myExternalScope = $scope;

        console.log('luan test');
        var formdata = new FormData();
        $('#uploadBtnProject').unbind('click').on('click', function ($files) {
            //alert('Ready to Uplaod. Missing reference $http');
            //return;
            console.log('get files', $files);
            var docTypeID = '4';
            var files = fileUpload.files;
 
            if (files.length == 0 || !files.length || !docTypeID) {
                dhtmlx.alert('Please chose a doc type and select a file.');
                return;
            }
            if (files[0].size / 1024 / 1024 > 128) {
                dhtmlx.alert('File size exceed 128MB. Please select a smaller size file.');
                return;
            }
            $('#uploadBtnProject').prop('disabled', true);
            $('#spinRow').show();

            angular.forEach(fileUpload.files, function (value, key) {
                //$scope.selectedFileName = $files[0].name;
                formdata.append(key, value);
            	//$('#uploadBtnProject').prop('disabled', false);
            });

            var request = {
                method: 'POST',
                url: serviceBasePath + '/uploadFiles/Post/' + "349" + '/' + docTypeID,
                data: formdata, //fileUpload.files, //$scope.
                ignore: true,
                headers: {
                    'Content-Type': undefined
                }
            };

            /* Needs $http */
            // SEND THE FILES.
            $http(request).then(function success(d){
                console.log(d);
                var gridUploadedDocument = modal.find('.modal-body #gridUploadedDocument tbody');
                gridUploadedDocument.empty();

                //wbsTree.getDocument().getDocumentByProjID().get({ ProjectID: _selectedProjectID }, function (response) {
                //    wbsTree.setDocumentList(response.result);
                //    for (var x = 0; x < _documentList.length; x++) {
                //        gridUploadedDocument.append('<tr><td style="width: 20px"><input type="checkbox" name="record"></td><td style="display:none">' + _documentList[x].DocumentID + '</td><td><a href="' + serviceBasePath + '/Request/DocumentByDocID/' + _documentList[x].DocumentID + '" download>' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><tr>');
                //    }
                $http.get(serviceBasePath + "Request/Document/" + "349")
                    .then(function success(response) {
                        console.log(response);
                        wbsTree.setDocumentList(response.data.result);
                        for (var x = 0; x < _documentList.length; x++) {
                            gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px"><input type="checkbox" name="record"></td><td><a href="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '">' + _documentList[x].DocumentName + '</a></td><td>' + _documentList[x].DocumentTypeName + '</td><td>' + moment(_documentList[x].CreatedDate).format('MM/DD/YYYY') + '</td><tr>');
                        }

                        var deleteDocBtn = modal.find('.modal-body #delete-doc');
                        deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                    },function error(){}).
                    finally(function () {
                        console.log($rootScope);
                        // $rootScope.buttonDisabled =false;
                    })

                dhtmlx.alert(d.data);
            },function error(d){
                dhtmlx.alert(d.ExceptionMessage);
            }).finally(function (){
                //Clear selected files
                fileUpload.value = "";
                formdata = new FormData();
                $('#uploadBtnProject').prop('disabled', false);
                $('#spinRow').hide();
            })
                    
            ;
                  
        });
        var url = serviceBasePath + 'response/Location/';
        Location.get({}, function (response) {
            console.log(response)
            $scope.checkList = [];
            $scope.TerritoryCollection = response.result;
            console.log($scope.TerritoryCollection);
            $scope.orgTerritoryCollection = angular.copy(response.result);
            addIndex($scope.TerritoryCollection);
            angular.forEach($scope.TerritoryCollection, function (item, index) {
                $scope.checkList[index + 1] = false;
                item.checkbox = false;
            });
            console.log($scope.TerritoryCollection);
            $scope.gridOptions.data = $scope.TerritoryCollection;
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

        var isFresh = true;
        var currentPoint = "";

        $scope.addRow = function () {
            console.log($scope.TerritoryCollection);
            var x = Math.max.apply(Math, $scope.TerritoryCollection.map(function (o) {

                return o.displayId;
            }))

            if (x < 0) {
                console.log(x);
                x = 0;
            }

            $scope.checkList[++x] = false;
            $scope.TerritoryCollection.splice(x, 0, {
                displayId: x,
                LocationID: '',
                LocationName: '',
                AddressLine: '',
                City: '',
                State: '',
                ZipCode: '',
                Country: '',
                LocationDescription: '',
                // UniqueIdentityNumber: '',
                checkbox: false,
                new: true
            });
            /*
            if (isFresh) {
                UniqueIdentityNumber.get({ NumberType: 'Location' }, function (response) {
                    $scope.TerritoryCollection[$scope.TerritoryCollection.length - 1].UniqueIdentityNumber = response.result;
                    isFresh = false;
                    currentPoint = response.result;
                });
            } else {
                currentPoint = "BC" + ((parseInt(currentPoint.substr(2)) + 1)).toString().padStart(5, '0');
                $scope.TerritoryCollection[$scope.TerritoryCollection.length - 1].UniqueIdentityNumber = currentPoint;
            }
            */

            console.log($scope.TerritoryCollection[$scope.TerritoryCollection.length - 1]);
            $scope.gridApi.core.clearAllFilters();//Nivedita-T on 16/11/2021
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
            rowHeight: 40,
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
                    cellClass: 'c-col-id',
                    width: 50,
                    //cellTemplate:'<div ng-class="c-col-id" style="margin-top:15%;" ng-click="clicked(row,col)">{{row.getProperty(col.field)}}</div>'

                }
                ,
                {
                    field: 'LocationName',
                    name: 'Name',
                    enableCellEditOnFocus: true,
                    widith: 300
                    //   enableCellEdit :true,
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    // cellFilter : 'mapStatus'


                }, {
                    field: 'AddressLine',
                    name: 'Address',
                    widith: 300
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                }, {
                    field: 'City',
                    name: 'City',
                    widith: 300
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                }, {
                    field: 'State',
                    name: 'State',
                    widith: 300
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                }, {
                    field: 'ZipCode',
                    name: 'Zip',
                    widith: 300
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                }, {
                    field: 'Country',
                    name: 'Country',
                    widith: 300
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                }, {
                    field: 'LocationDescription',
                    name: 'Description',
                    widith: 300
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'


                }, /*{
                    field: 'UniqueIdentityNumber',
                    name: 'Unique Identifier',
                    width: 200,
                    //  editableCellTemplate: $scope.cellInputEditableTemplate,
                    //cellFilter :'mapStatus'
                    

                },*/ {
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
            okToExit = false;
            var isReload = false;
            var isChanged = true;
            var isFilled = true;
            var listToSave = [];
            angular.forEach($scope.TerritoryCollection, function (value, key, obj) {

                if (value.LocationName == "" || value.AddressLine == "" || value.City == "" || value.State == "" || value.ZipCode == ""/*|| value.UniqueIdentityNumber == ""*/) {
                    dhtmlx.alert({
                        text: "Please fill data to all required fields before save (Row " + value.displayId + ")",
                        width: "400px"
                    });
                    okToExit = false;
                    isFilled = false;
                    return;
                }

                /*
                if (!(/(BC[0-9]{5})/.test(value.UniqueIdentityNumber) && value.UniqueIdentityNumber.length == 7)) {
                    dhtmlx.alert({
                        text: "Unique identifier must be in the format of BCxxxxx (Row " + value.displayId + ")",
                        width: "400px"
                    });
                    okToExit = false;
                    isFilled = false;
                    return;
                }
                */

                if (isFilled == false) {
                    return;
                }
                //New Item
                if (value.new === true) {
                    okToExit = false;
                    isReload = true;
                    var dataObj = {
                        Operation: '1',
                        LocationName: value.LocationName,
                        AddressLine: value.AddressLine,
                        City: value.City,
                        State: value.State,
                        ZipCode: value.ZipCode,
                        Country: value.Country,
                        LocationDescription: value.LocationDescription,
                        //  UniqueIdentityNumber: value.UniqueIdentityNumber
                    }
                    listToSave.push(dataObj);
                    okToExit = true;

                }
                else {//Update Item if there is changes
                    okToExit = false;
                    isChanged = true;
                    angular.forEach($scope.orgTerritoryCollection, function (orgItem) {
                        if (value.LocationID === orgItem.LocationID &&
                            value.LocationName === orgItem.LocationName &&
                            value.AddressLine === orgItem.AddressLine &&
                            value.City === orgItem.City &&
                            value.State === orgItem.State &&
                            value.ZipCode === orgItem.ZipCode &&
                            value.Country === orgItem.Country &&
                            value.LocationDescription === orgItem.LocationDescription  /* &&
                             value.UniqueIdentityNumber === orgItem.UniqueIdentityNumber*/ ) {
                            isChanged = false;
                            okToExit = true;
                        }
                    });
                    if (isChanged == true) {
                        var dataObj = {
                            Operation: '2',
                            LocationID: value.LocationID,
                            LocationName: value.LocationName,
                            AddressLine: value.AddressLine,
                            City: value.City,
                            State: value.State,
                            ZipCode: value.ZipCode,
                            Country: value.Country,
                            LocationDescription: value.LocationDescription,
                            // UniqueIdentityNumber: value.UniqueIdentityNumber
                        }
                    } else {
                        var dataObj = {
                            Operation: '4',
                            LocationID: value.LocationID,
                            LocationName: value.LocationName,
                            AddressLine: value.AddressLine,
                            City: value.City,
                            State: value.State,
                            ZipCode: value.ZipCode,
                            Country: value.Country,
                            LocationDescription: value.LocationDescription,
                            //   UniqueIdentityNumber: value.UniqueIdentityNumber
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
                    isFresh = true;

                    response.data.result.replace(/[\r]/g, '\n');

                    if (response.data.result) {
                        dhtmlx.alert(response.data.result);
                    } else {
                        dhtmlx.alert('No changes to be saved.');
                    }

                    if (isTest == true) {
                        //   var newUrl = $location.path();
                        console.log(newUrl);
                        // onRouteChangeOff();
                        window.location.href = "#" + newUrl;

                    }

                    $scope.orgTerritoryCollection = angular.copy($scope.TerritoryCollection);
                    okToExit = true;
                    $state.reload();
                }, function error(response) {
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
            $scope.listToDelete = [];
            console.log($scope.TerritoryCollection);
            angular.forEach($scope.TerritoryCollection, function (item) {
                isChecked = false;
                if (item.checkbox == true) {
                    if (item.new === true) {
                        unSavedChanges = true;
                        newList.push(item);

                    } else {
                        ischecked = true;
                        var dataObj = {
                            Operation: '3',
                            LocationID: item.LocationID,
                            LocationName: item.LocationName,
                            AddressLine: item.AddressLine,
                            City: item.City,
                            State: item.State,
                            ZipCode: item.ZipCode,
                            Country: item.Country,
                            LocationDescription: item.LocationDescription,
                            //   UniqueIdentityNumber: item.UniqueIdentityNumber,
                            displayId: item.displayId
                        }
                        listToSave.push(dataObj);
                        $scope.listToDelete.push(dataObj);
                        //dhtmlx.alert("Record Deleted.");
                    }
                }


            });

            if (newList.length != 0) {
                for (var i = 0; i < newList.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.TerritoryCollection, function (item, index) {
                        if (item.displayId == newList[i].displayId) {
                            item.checkbox = false;

                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(newList[i].displayId, 1);
                        $scope.TerritoryCollection.splice(ind, 1);
                    }
                }

            }
            if (listToSave.length != 0) {

                for (var i = 0; i < listToSave.length; i++) {
                    var ind = -1;
                    angular.forEach($scope.TerritoryCollection, function (item, index) {
                        if (item.displayId == listToSave[i].displayId) {
                            item.checkbox = false;

                            ind = index;
                        }
                    });
                    if (ind != -1) {
                        $scope.checkList.splice(listToSave[i].displayId, 1);
                        $scope.TerritoryCollection.splice(ind, 1);
                    }
                }
            }

            //if(isChecked == true || unSavedChanges == true){
            //    angular.forEach(scope.SubcontractorTypeCollection,function(item){})
            //}else{
            //    alert("No row selected for delete");
            //}
            //$state.reload();        //Temporary Solution
        }
        $scope.checkForChanges = function () {
            var unSavedChanges = false;
            var originalCollection = $scope.orgTerritoryCollection;
            var currentCollection = $scope.TerritoryCollection;
            if (currentCollection.length != originalCollection.length) {
                unSavedChanges = true;
                return unSavedChanges;
            } else {
                angular.forEach(currentCollection, function (currentObject) {
                    for (var i = 0, len = originalCollection.length; i < len; i++) {
                        if (unSavedChanges) {
                            return unSavedChanges; // no need to look through the rest of the original array
                        }
                        if (originalCollection[i].LocationID == currentObject.LocationID) {
                            var originalObject = originalCollection[i];
                            // compare relevant data
                            if (originalObject.LocationName !== currentObject.LocationName ||
                                originalObject.LocationDescription !== currentObject.LocationDescription ||
                                originalObject.AddressLine !== currentObject.AddressLine ||
                                originalObject.City !== currentObject.City ||
                                originalObject.State !== currentObject.State ||
                                originalObject.ZipCode !== currentObject.ZipCode ||
                                originalObject.Country !== currentObject.Country
                                /* ||  originalObject.UniqueIdentityNumber !== currentObject.UniqueIdentityNumber */) {
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
