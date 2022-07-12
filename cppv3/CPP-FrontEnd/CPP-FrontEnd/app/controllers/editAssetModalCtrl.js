// Edit Asset Modal Controller

angular.module('cpp.controllers')
    .controller('editAssetModalCtrl',
    ['$scope','$rootScope','$uibModal','$http',
        function($scope,$rootScope,$uibModal,$http){
            console.log($scope.params);

            // variables
            var log = {
                Operation: 1,
                ID: 0, // modify in back end
                Description: 'Update/Change',
                StartDate: new Date(),
                EndDate: new Date(),
                Cost: 0,
                AssetID: $scope.params.asset.ID,
                Note: ''
            };

            // parameters
            $scope.asset = $scope.params.asset;
            $scope.isComp = $scope.params.isComp;
            $scope.isNew = $scope.params.isNew;
            $scope.assetCollection = $scope.params.assetCollection;
            $scope.facilityCollection = $scope.params.facilityCollection;
            $scope.statusCollection = $scope.params.statusCollection;
            $scope.parentCollection = $scope.params.parentCollection;
            $scope.locationCollection = [
                {id: "01", Location:"a"},
                {id: "02", Location:"b"}
            ];


            // make a copy of original item
            $scope.editedItem = angular.copy($scope.asset);
            $scope.ad = moment($scope.editedItem.AcquisitionDate).format("MM/DD/YYYY");

            // save item
            $scope.save = function(){
                $scope.editedItem.Operation = 2;
                $scope.editedItem.Status = $("#statusCollection").val();
                $scope.editedItem.AcquisitionDate = $("#asset_ad").val();
                angular.forEach($scope.facilityCollection,function(item){
                    if(item.Name == $("#facilityCollection").val()){
                        $scope.editedItem.Facility_ID = item.ID;
                    }
                });

                var ob = (!$scope.isComp) ?
                {
                    "Operation": 2,
                    "ID": $scope.editedItem.ID,

                    "Tag": $scope.editedItem.Tag,
                    "Name": $scope.editedItem.Name,
                    "Cost": $scope.editedItem.Cost,
                    "AcquisitionDate": $("#asset_ad").val(),
                    "Note": $scope.editedItem.Note,

                    "Facility_ID": $scope.editedItem.Facility_ID,
                    "EOL": $scope.editedItem.EOL,
                    "Status": $scope.editedItem.Status
                }
                :
                {
                    "Operation": 2,
                    "ID": $scope.editedItem.ID,

                    "Tag": $scope.editedItem.Tag,
                    "Name": $scope.editedItem.Name,
                    "Cost": $scope.editedItem.Cost,
                    "AcquisitionDate": $("#asset_ad").val(),
                    "Note": $scope.editedItem.Note,

                    "asset": null,
                    "AssetID": $scope.params.asset.AssetID
                };


                if($scope.isNew){
                    $scope.params.asset = ob;
                    $scope.params.confirm = "Success";
                    $scope.params.newItem = ob;
                    $scope.exit();
                }
                else {
                    var url = serviceBasePath+'response/';
                    url += ($scope.isComp)?"Component/":"Asset";
                    console.log(ob);
                    console.log(url);
                    $http({
                        url:url,
                        method:"POST",
                        data: ob,
                        headers:{'Content-Type':'application/json'}
                    }).then(function(response){
                        if(response.data.result == "Success"){
                            $scope.exit();
                            console.log(":D");
                        }
                    });
                }


            };

            // check for unsaved changes before close
            $scope.cancel = function(){
                // if changes were made, ask if they want to save
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
                        console.log(scope.params.confirm);
                        // exit without saving
                        if (scope.params.confirm === "exit") { $scope.exit(); }
                        // save then exit
                        else if (scope.params.confirm === "save") { $scope.save(); }
                        // go back
                        else {  }
                    });
                }
                // if no changes were made, then exit
                else {
                    $scope.exit();
                }
            };

            // add to logString
            $scope.addToString = function(a,b,c,ct){
                console.log(ct);
                // example string
                // Asset's [fieldname] was changed from [original.field] to [new.field]
                if(ct==0) {
                    log.Note += "Item's " + a;
                }else {
                    log.Note += " / Item's " + a;
                }

                if(b == ""){
                    log.Note += " was added: " + c;
                }
                else if(c == ""){
                    log.Note += " was removed.";
                }
                else {
                    log.Note += " was modified from \"" + b + "\" to \"" + c + "\"";
                }

            };

            // exits
            $scope.exit = function(){
                $scope.$close();
            };

            // checks all fields for changes and creates a log
            var changes = function() {
                log.Note = ''; // clear note
                var c = false;
                var ct = 0;

                if ($scope.editedItem.Name != $scope.asset.Name) {
                    $scope.addToString("name", $scope.asset.Name, $scope.editedItem.Name,ct++);
                    c = true;
                }

                if (!c && $scope.editedItem.Cost != $scope.asset.Cost) {
                    $scope.addToString("cost", $scope.asset.Cost, $scope.editedItem.Cost,ct++);
                    c = true;
                }

                if (!c && $scope.editedItem.Note != $scope.asset.Note) {
                    $scope.addToString("note", $scope.asset.Note, $scope.editedItem.Note,ct++);
                    c = true;
                }

                if (!c && $scope.editedItem.Tag != $scope.asset.Tag) {
                    $scope.addToString("tag", $scope.asset.Tag, $scope.editedItem.Tag,ct++);
                    c = true;
                }

                if (!c && $scope.editedItem.FacilityName != $("#facilityCollection").val()) {
                    $scope.editedItem.FacilityName = $("#facilityCollection").val();
                    $scope.addToString("facility", $scope.editedItem.FacilityName, $("#facilityCollection").val(),ct++);
                    c = true;
                }

                if (!c && $scope.editedItem.FacilityLocation != $("#locationCollection").val()) {
                    $scope.editedItem.FacilityLocation = $("#locationCollection").val()
                    $scope.addToString("location", $scope.editedItem.FacilityLocation, $("#locationCollection").val(),ct++);
                    c = true;
                }

                if (!c && moment($scope.editedItem.AcquisitionDate).format("MM/DD/YYYY") != $scope.ad) {
                    $scope.editedItem.AcquisitionDate = $scope.ad;
                    $scope.addToString("acquisition date", moment($scope.editedItem.AcquisitionDate).format("MM/DD/YYYY"), $scope.ad,ct++);
                    c = true;
                }

                if (!c && $scope.editedItem.Status != $("#statusCollection").val()) {
                    $scope.editedItem.Status = $("#statusCollection").val();
                    $scope.addToString("status", $scope.originalItem.Status, $("#statusCollection").val(),ct++);
                    c = true;
                }

                //if($scope.isComp){
                //    if($scope.originalParent.Name != $("#parentCollection").val()){
                //        for(var i = 0; i < $scope.assetCollection.length; i++){
                //            if($scope.assetCollection[i].Name == $("#parentCollection").val()){
                //                $scope.addToString("parent",$scope.originalParent.Name,$("#parentCollection").val(),ct++);
                //                $scope.parent = $scope.assetCollection[i];
                //                c = true;
                //            }
                //        }
                //    }
                //}

                return c;
            };


            $scope.test = function(){
                console.log($scope.asset);
            };

        }
    ]);
