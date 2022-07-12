// New Asset Controller

angular.module('cpp.controllers')
    .controller('newAssetModalCtrl',
    ['$scope','$rootScope','$uibModal','$http',
        function($scope,$rootScope,$uibModal,$http) {
            console.log("Creating a new asset");
            console.log($scope.params);

            // get parameters
            $scope.isComponent = $scope.params.isComponent;
            $scope.statusCollection = $scope.params.collections.statusC;
            $scope.facilityCollection = $scope.params.collections.facilityC;

            // new asset to be added
            $scope.asset = {
                Operation: 1,
                assetComponents: [],
                assetHistories: [],
                "assetSupplier": null,
                "facilityAssets": null,
                ID: 0,
                "Asset_Type_ID": null,
                "Asset_Supplier_ID": null,
                Facility_ID: 0,
                Tag: "",
                Name: "",
                AcquisitionDate: "",
                Cost: null,
                EOL: "",
                Status: "",
                Note: ""
            };

            // data lists to populate grids
            $scope.CompList = $scope.asset.assetComponents;
            $scope.HistList = $scope.asset.assetHistories;
            $scope.AttcList = [];

            // set views
            $scope.clistempty = ($scope.CompList.length == 0);
            $scope.hlistempty = ($scope.HistList.length == 0);
            $scope.dlistempty = ($scope.AttcList.length == 0);

            // grid definitions... short for details page, full for view all page
            var compGridDef = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewCompDetails()"></div>'
                },
                {
                    field: 'Name',
                    displayName: 'Name'
                },
                {
                    field: 'Cost',
                    displayName: 'Cost',
                    cellFilter: 'currency'
                },
                {
                    field: 'edit',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-edit" ng-dblclick="updateComp()"></div>'
                },
                {
                    field: 'delete',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-delete" ng-dblclick="removeComp()"></div>'
                }
            ];
            var histGridDef = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>'
                },
                {
                    field:'Description',
                    displayName: 'Description'
                },
                {
                    field:'StartDate',
                    displayName: 'Start',
                    cellFilter: 'date:\'MM/dd/yyyy\''

                },
                {
                    field:'EndDate',
                    displayName: 'End',
                    cellFilter: 'date:\'MM/dd/yyyy\''
                },
                {
                    field:'Cost',
                    displayName: 'Cost',
                    cellFilter: 'currency'
                },
                {
                    field: 'edit',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-edit" ng-dblclick="updateHist()"></div>'
                },
                {
                    field: 'delete',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-delete" ng-dblclick="removeHist()"></div>'
                }

            ];
            var fullCGridDef = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewCompDetails()"></div>'
                },
                {
                    field: 'Tag',
                    displayName: 'Tag'
                },
                {
                    field: 'Name',
                    displayName: 'Name'
                },
                {
                    field: 'AcquisitionDate',
                    displayName: 'Acquisition Date',
                    cellFilter: 'date:\'MM/dd/yyyy\''
                },
                {
                    field: 'Cost',
                    displayName: 'Cost',
                    cellFilter: 'currency'
                },
                {
                    field: 'Note',
                    displayName: 'Note'
                },
                {
                    field: 'edit',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-edit" ng-dblclick="updateComp()"></div>'
                },
                {
                    field: 'delete',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-delete" ng-dblclick="removeComp()"></div>'
                }
            ];
            var fullHGridDef = [
                {
                    field: 'info',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-info" ng-dblclick="viewHistDetails()"></div>'
                },
                {
                    field:'Description',
                    displayName: 'Description'
                },
                {
                    field:'StartDate',
                    displayName: 'Start',
                    cellFilter: 'date:\'MM/dd/yyyy\''

                },
                {
                    field:'EndDate',
                    displayName: 'End',
                    cellFilter: 'date:\'MM/dd/yyyy\''
                },
                {
                    field:'Cost',
                    displayName: 'Cost',
                    cellFilter: 'currency'
                },
                {
                    field: 'Note',
                    displayeName: 'Note'
                },
                {
                    field: 'edit',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-edit" ng-dblclick="updateHist()"></div>'
                },
                {
                    field: 'delete',
                    displayName: '',
                    enableCellSelection: true,
                    width: 30,
                    cellTemplate: '<div class="icon-delete" ng-dblclick="removeHist()"></div>'
                }
            ];

            // used to organize selected items
            $scope.myCSelections = [];
            $scope.myHSelections = [];

            // grids - component, history
            $scope.CompGrid = {
                data: 'CompList',
                enableRowSelection: true,
                enableCellSelection: false,
                selectedItems: $scope.myCSelections,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                rowHeight: 35,
                afterSelectionChange: function(rowItem){
                    $scope.myCSelections = rowItem;
                },
                columnDefs: compGridDef
            };
            $scope.HistGrid = {
                data: 'HistList',
                enableRowSelection: true,
                enableCellSelection: false,
                selectedItems: $scope.myHSelections,
                enableCellEditOnFocus: false,
                enableCellEdit: false,
                multiSelect: false,
                rowHeight: 35,
                afterSelectionChange: function(rowItem) {
                    $scope.myHSelections = rowItem;
                },
                columnDefs: histGridDef
            };

            // add new item
            $scope.addComp = function() { addNew("comp"); };
            $scope.addHist = function() { addNew("hist"); };
            $scope.addAttc = function() { addNew("attc"); };
            var addNew = function(type){
                // variables
                var s = "Add New ";
                var tUrl, ctrl, sz;
                var scope = $rootScope.$new();

                // set variables
                if(type == "comp"){
                    s += "component";
                    tUrl = 'app/views/Modal/new_comp_modal.html';
                    ctrl = "newCompModalCtrl";
                    sz = 'lg';
                }
                else if (type == "hist"){
                    s += "history log";
                    tUrl = 'app/views/Modal/new_history_log_modal.html';
                    ctrl = "newHistoryModalCtrl";
                    sz = 'md';
                }
                else if (type == "attc"){
                    s += "attachment";
                    // temporary
                    alert("ERROR: ADD functionality not yet available for attachments");
                    return;
                }
                else {
                    alert("An error has occurred");
                    return;
                }

                // print action
                console.log(s);

                // set scope parameters
                scope.params = {
                    asset: $scope.asset,
                    confirm: "",
                    newItem: {},
                    isNew: true
                };

                // open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: true,
                    keyboard: false,
                    scope: scope,
                    templateUrl: tUrl,
                    size: sz,
                    controller: ctrl
                });
                $rootScope.modalInstance.result.then(function(){
                    console.log(scope.params);
                    console.log(type);

                    if(scope.params.confirm == "Success") {
                        if (type == "hist") {
                            $scope.HistList.push(scope.params.newItem);
                            $scope.hlistempty = ($scope.HistList.length == 0);
                        }
                        else if(type == "comp"){
                            $scope.CompList.push(scope.params.newItem);
                            $scope.clistempty = ($scope.CompList.length == 0);
                        }
                        else {
                            alert("ERROR: Some error has occurred");
                        }
                    }
                });
            };

            // view all items
            $scope.viewAllHist = function() { viewAll("hist"); };
            $scope.viewAllComp = function() { viewAll("comp"); };
            $scope.viewAllAttc = function() { viewAll("attc"); };
            var viewAll = function(type){
                var s = "View All ";

                var dl, gd, nm, add;
                var scope = $rootScope.$new();
                if(type=="comp"){
                    s += "components";
                    dl = $scope.CompList;
                    gd = fullCGridDef;
                    nm = "Component List";
                    add = "Component";
                }
                else if(type=="hist"){
                    s += "history logs";
                    dl = $scope.HistList;
                    gd = fullHGridDef;
                    nm = "History Log";
                    add = "Log";
                }
                else if(type=="attc"){
                    s += "attachments";
                    alert("ERROR: View all functionality not yet available for attachments");
                    return;
                }
                else {
                    alert("ERROR: Some error has occurred");
                    return;
                }

                console.log(s);

                // set scope parameters
                scope.params = {
                    list: dl,
                    col: gd,
                    nm: nm,
                    asset: {Name: "New"},
                    add: add,
                    isNew: true
                };

                // open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: 'app/views/Modal/va_modal.html',
                    size: 'lg',
                    controller: 'VAModalCtrl'
                });
            };

            // view item details
            $scope.viewHistDetails = function() { viewDetails("hist"); };
            $scope.viewCompDetails = function() { viewDetails("comp"); };
            $scope.viewAttcDetails = function() { viewDetails("attc"); };
            var viewDetails = function(type){
                var s = "View ";

                var tUrl, ctrl, p1;
                var scope = $rootScope.$new();

                if(type == "comp"){
                    s+= "component";
                    tUrl = 'app/views/Modal/comp_detail_modal.html';
                    ctrl = "CompDetailModalCtrl";
                    p1 = $scope.myCSelections;
                }
                else if(type == "hist"){
                    s+= "history log";
                    tUrl = 'app/views/Modal/log_detail_modal.html';
                    ctrl = "LogDetailModalCtrl";
                    p1 = $scope.myHSelections;
                }
                else if(type == "attc"){
                    s+= "attachment";
                    alert("ERROR: DETAIL functionality not yet available for attachments");
                    return;
                }
                else {
                    alert("Err: Detail type \" " + type + " \" does not exist.");
                    return;
                }

                s+= " details";
                console.log(s);

                // set parameters
                scope.params = {
                    dt : p1,
                    asset : {Name: "New"},
                    isNew: true
                };

                // open modal
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: tUrl,
                    size: 'md',
                    controller: ctrl
                });
            };


            // update items
            $scope.updateComp = function() { update("comp"); };
            $scope.updateHist = function() { update("hist"); };
            $scope.updateAttc = function() { update("attc"); };
            var update = function(type){
                // variables
                var s = "Updating ",
                    scope = $rootScope.$new(),
                    tUrl = 'app/views/Modal/editAssetModal.html',
                    ctrl = "editAssetModalCtrl";

                // set variables and params
                if(type=="comp"){
                    $scope.list = $scope.CompList;
                    s+="component";
                    scope.params = {
                        asset: $scope.myCSelections.entity,
                        isComp: true,
                        isNew: true,
                        assetCollection: $scope.assetCollection,
                        facilityCollection: $scope.facilityCollection,
                        statusCollection: $scope.statusCollection,
                        parentCollection: $scope.parentCollection,
                        newItem: {}
                    }
                }
                else if(type=="hist"){
                    $scope.list = $scope.HistList;
                    s+="history log";
                    alert("ERROR: UPDATE functionality not yet available for history logs");
                    return;
                }
                else if(type=="attc"){
                    s+="attachment";
                    alert("ERROR: UPDATE functionality not yet available for attachments");
                    return;
                }
                else {
                    alert("An error has occured");
                    return;
                }

                console.log(s);
                console.log($scope.myCSelections.entity);

                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: tUrl,
                    size: 'md',
                    controller: ctrl
                });
                $rootScope.modalInstance.result.then(function(){
                    console.log(scope.params);
                    if(scope.params.confirm  == "Success"){
                        angular.forEach($scope.list,function(item){
                            if(item == $scope.selectedItem){
                                item = scope.params.newItem;
                            }
                        });
                    }
                });
            };

            // remove an item
            $scope.removeHist = function() { remove("hist", $scope.myHSelections.entity); };
            $scope.removeComp = function() { remove("comp", $scope.myCSelections.entity); };
            $scope.removeAttc = function() { remove("attc", ""); };
            var remove = function(type,item){
                var s = "Delete ";

                if(type=="comp"){
                    s += "component";
                    $scope.CompList.splice($scope.CompList.indexOf(item),1);
                    $scope.clistempty = $scope.CompList.length == 0;
                }
                else if(type=="hist"){
                    s += "history log";
                    $scope.HistList.splice($scope.HistList.indexOf(item),1);
                    $scope.hlistempty = $scope.HistList.length == 0;
                }
                else if(type=="attc"){
                    s += "attachment";
                    alert("ERROR: Some error has occurred");
                    return;
                }
                else{
                    alert("ERROR: Some error has occurred");
                    return;
                }

                console.log(s);
            };

            // return true if any NECESSARY changes have been made
            var changes = function(){
                return ($scope.asset.Name != ""
                && $scope.asset.Tag != ""
                && $scope.asset.AcquisitionDate != ""
                && $scope.asset.Cost != ""
                && $scope.asset.Cost != 0)
            };

            // save
            $scope.save = function() {
                console.log("Saving New Asset");

                // bind data
                $scope.asset.Status = $("#statusSelect").val();
                $scope.asset.Facility_ID = $("#facilitySelect").val();

                // if there are no changes, do not save empty item
                if(!changes()){
                    alert("no changes have been detected");
                    return;
                }

                // send object to backend
                var url = serviceBasePath+"response/Asset";
                console.log($scope.asset);

                $http({
                    url: url,
                    method: "POST",
                    data:$scope.asset,//item,
                    headers: {'Content-Type': 'application/json'}
                }).then(function(response){
                    if(response.data.result==="Success"){
                        console.log("Successfully added new asset");
                        $scope.exit();
                    }
                });

            };

            // cancel
            $scope.cancel = function() {
                // if there are changes, prompt user, else exit
                if(changes()){
                    var scope = $rootScope.$new();
                    scope.params = {confirm: ""};
                    $rootScope.modalInstance = $uibModal.open({
                        scope: scope,
                        templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                        controller: 'exitConfirmation',
                        size: 'md',
                        backdrop: true
                    });
                    $rootScope.modalInstance.result.then(function() {
                        // exit without save
                        if (scope.params.confirm === "exit") { $scope.exit(); }
                        // save then exit
                        else if (scope.params.confirm === "save") { $scope.save(); }
                        // go back to make changes
                        else {  }
                    });
                }
                else { $scope.exit(); }
            };

            // close
            $scope.exit = function() { $scope.$close(); };


            $scope.test = function(){ console.log( $("#statusSelect").val()); };
            $scope.test2 = function(){ console.log($scope.asset); };
        }])
    .directive('blurToCurrency', function($filter){
            return {
                scope: { amount  : '=' },
                link: function(scope, el, attrs){
                    el.val($filter('currency')(scope.amount));
                    el.bind('focus', function(){ el.val(scope.amount); });
                    el.bind('input', function(){ scope.amount = el.val(); scope.$apply(); });
                    el.bind('blur',  function(){ el.val($filter('currency')(scope.amount)); });
                }
            }
        });
