angular.module('cpp.controllers').
    //Access Control

    controller('AccessControlCtrl',['$scope','$rootScope','Role','$uibModal','$http','Page','ProjectTitle','TrendStatus','$location',
        function ($scope, $rootScope, Role,$uibModal,$http, Page,ProjectTitle,TrendStatus,$location)
        {
            Page.setTitle('Application Security');

            var roleCollection;
            TrendStatus.setStatus('');
            ProjectTitle.setTitle('');
            $scope.ACItem = null;
            var newOrEdit = '';
            Role.get({}, function (RoleData) {
                roleCollection = RoleData.result;
                $scope.rowCollection = roleCollection;
                $scope.orgRowCollection = angular.copy(RoleData.result);
                $scope.setAccessControl($scope.rowCollection[0]);
            });

            //select a table row
            $scope.setAccessControl = function (row) {
                $scope.ACItem = row;
            };

            //Add new item and update database
            $scope.newAccessControl = function () {
                var scope = $rootScope.$new();
                newOrEdit = 'New';
                $scope.ACItem = null;
                scope.params = {ACItem: $scope.ACItem, newOrEdit: newOrEdit}

                $rootScope.modalInstance = $uibModal.open({
                    templateUrl: 'app/views/modal/access_control_modal.html',
                    scope: scope,
                    backdrop: 'static',
                    controller: 'AccessControlModalCtrl',
                    size: 'md'
                });

                $rootScope.modalInstance.result.then(function (response) {
                    Role.get({}, function (RoleData) {
                        roleCollection = RoleData.result;
                        $scope.rowCollection = roleCollection;

                    });
                });
            };

            //Delete access control item and update database
            $scope.toggle = function(index,obj){
                var tempACL = "";
                console.log(obj);
                console.log(index);
                if(obj.AccessControlList[index]==='0'){
                    tempACL += obj.AccessControlList.substring(0,index);
                    tempACL += '1';
                    tempACL += obj.AccessControlList.substring(index + 1, obj.AccessControlList.length);
                   obj.AccessControlList = tempACL;
                    console.log(obj);
                }
                else{
                    tempACL += obj.AccessControlList.substring(0,index);
                    tempACL += '0';
                    tempACL += obj.AccessControlList.substring(index + 1, obj.AccessControlList.length);
                    obj.AccessControlList = tempACL;
                    console.log(obj);
                }
            }
            $scope.deleteAccessControl = function () {
                var url = serviceBasePath+'response/role/';
               // var url = 'http://localhost:29986/api/response/role/'

                var listToSave = [];
                var dataObj = {
                    "Operation": 3,
                    "Role": $scope.ACItem.Role,
                    "AccessControlList": $scope.ACItem.AccessControlList
                }
                listToSave.push(dataObj);
                var scope = $rootScope.$new();
                $scope.confirm = "";
                scope.params = {
                    confirm: $scope.confirm
                };
                $rootScope.modalInstance = $uibModal.open({
                    templateUrl: 'app/views/Modal/confirmation_dialog.html',
                    size: 'sm',
                    controller: 'ConfirmationCtrl',
                    scope: scope

                });
                console.log($scope.confirm);
                $rootScope.modalInstance.result.then(function (data) {
                    if (scope.params.confirm === 'yes') {

                        $http.post(url, listToSave).then(function success(response) {
                            response.data.result.replace(/[\r]/g, '\n');

                            if (response.data.result) {
                                dhtmlx.alert(response.data.result);
                            } else {
                                dhtmlx.alert('No changes to be saved.');
                            }

                            Role.get({}, function success(RoleData) {
                                roleCollection = RoleData.result;
                                $scope.rowCollection = roleCollection;
                            });
                        },function error(response){
                            dhtmlx.alert("Failed to delete! Please contact your administrator.");
                        })

                    }
                });
            };

            //open a modal to edit data
            $scope.saveAccessControl = function () {
                $("#save_Access_Control").attr("disabled", true);
                var dummyData ="";
                var objArray = [];
                    var baseUrl = serviceBasePath + 'response/role/';
               // alert("save");

                    console.log($scope.rowCollection, $scope.orgRowCollection);
                    angular.forEach($scope.rowCollection, function (value, key, obj) {
                        //Detect if an entry has changes.
                        var isChange = true;
                        angular.forEach($scope.orgRowCollection, function (orgValue) {
                            if (orgValue.AccessControlList == value.AccessControlList &&
                                orgValue.Role == value.Role) {
                                isChange = false;
                            }
                        });

                        var dataObj = {};
                        if (isChange) {
                            dataObj = {
                                "Operation": '2',
                                "Role": value.Role,
                                "AccessControlList": value.AccessControlList
                            }
                        } else {
                            dataObj = {
                                "Operation": '4',
                                "Role": value.Role,
                                "AccessControlList": value.AccessControlList
                            }
                        }

                        objArray.push(dataObj);
                });
                console.log(objArray);
                $http.post(baseUrl, objArray).then(function success(response) {
                    $("#save_Access_Control").attr("disabled", false);
                    console.log(response);
                    response.data.result.replace(/[\r]/g, '\n');

                    if (response.data.result) {
                        $("#save_Access_Control").attr("disabled", false);
                        dhtmlx.alert(response.data.result);
                    } else {
                        $("#save_Access_Control").attr("disabled", false);
                        dhtmlx.alert('No changes to be saved.');
                    }

                    $scope.orgRowCollection = angular.copy($scope.rowCollection);
                }, function error(response) {
                    $("#save_Access_Control").attr("disabled", false);
                    console.log(response);
                    dhtmlx.alert("Failed to save. Please contact your administrator!");
                });

            }
            $scope.editAccessControl = function(){
                var scope = $rootScope.$new();
                newOrEdit = 'Edit';
                scope.params= {ACItem:$scope.ACItem,
                    newOrEdit:newOrEdit};
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    backdrop: 'static',
                    keyboard: false,
                    templateUrl: 'app/views/modal/access_control_modal.html',
                    controller:'AccessControlModalCtrl',
                    size: 'md'
                });

                // after close the popup modal
                $rootScope.modalInstance.result.then(function () {
                    Role.get({}, function (RoleData) {
                        roleCollection = RoleData.result;
                        $scope.rowCollection = roleCollection;

                    });
                });

            }


            $scope.checkForChanges = function(){
                var unSavedChanges = false;
                var originalCollection = $scope.orgRowCollection;
                var currentCollection = $scope.rowCollection;
                if (currentCollection.length != originalCollection.length){
                    unSavedChanges = true;
                    return unSavedChanges;
                } else {
                    angular.forEach(currentCollection,function(currentObject){
                        for(var i = 0, len = originalCollection.length; i < len; i++) {
                            if(unSavedChanges){
                                return unSavedChanges; // no need to look through the rest of the original array
                            }
                            if(originalCollection[i].Role == currentObject.Role) {
                                var originalObject = originalCollection[i];
                                // compare relevant data
                                if(originalObject.AccessControlList !== currentObject.AccessControlList)
                                {
                                    // alert if a change has not been save
                                    //alert("unsaved change on " + originalObject.Role);
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
            onRouteChangeOff = $scope.$on('$locationChangeStart', function(event){
                var newUrl = $location.path();
                if(!$scope.checkForChanges()) return;
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = {confirm:$scope.confirm};
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                    controller: 'exitConfirmation',
                    size: 'md',
                    backdrop: true
                });
                $rootScope.modalInstance.result.then(function(data){
                    console.log(scope.params.confirm);
                    if(scope.params.confirm === "exit"){
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if(scope.params.confirm === "save"){
                        $scope.saveAccessControl();
                        onRouteChangeOff();
                        $location.path(newUrl);
                    }
                    else if(scope.params.confirm === "back"){
                        //do nothing
                    }
                });
                event.preventDefault();
            });

        }]);
