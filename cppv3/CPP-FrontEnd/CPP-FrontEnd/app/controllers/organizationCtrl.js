angular.module('cpp.controllers'). 
    controller('OrganizationCtrl',['$scope','$rootScope','Organization','$uibModal','$http','Page','ProjectTitle','TrendStatus',
        function($scope,$rootScope,Organization,$uibModal,$http, Page,ProjectTitle,TrendStatus){
            Page.setTitle('Organization');
            TrendStatus.setStatus('');
            ProjectTitle.setTitle('');
            $scope.selectedOrg;
            Organization.lookup().get({},function(organizationData){
                $scope.organizationList = organizationData.result;
                $scope.setOrganization(($scope.organizationList[0]));
            });
            var newOrEdit = "";
            var map;
            var projList;
            var projectPolygonList = [];
            $scope.setOrganization = function(org) {

                $scope.selectedOrg = org;
                console.log(org);
                var infoWindow;
                //google.maps.Polygon.prototype.getBounds = function () {
                //    var bounds = new google.maps.LatLngBounds();
                //    var paths = this.getPaths();
                //    var path;

                //    for (var i = 0; i < paths.getLength(); i++) {
                //        path = paths.getAt(i);
                //        for (var ii = 0; ii < path.getLength(); ii++) {
                //            bounds.extend(path.getAt(ii));
                //        }
                //    }
                //    return bounds;
                //};

                initialize();
            };
                function initialize() {
                    var mapOptions = {
                        zoom: 4
                        //center: new google.maps.LatLng(37.09024, -95.712891),
                        //mapTypeId: google.maps.MapTypeId.TERRAIN
                    };

                    var myPolygon;

                    //map = new google.maps.Map(document.getElementById('map'),
                    //    mapOptions);
// Create a button for GIS toggle projects location
                    var projectoCntrolDiv = document.createElement('div');
                    var projectControl = new ProjectControl(projectoCntrolDiv, map);

                    projectoCntrolDiv.index = 1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(projectoCntrolDiv);
                    //

                    // Define the LatLng coordinates for the polygon.
                    var Coords = [];
                    console.log($scope.selectedOrg);
                    if(!$scope.selectedOrg){
                        $scope.selectedOrg.LatLong = '';
                    }
                    if($scope.selectedOrg.LatLong === "" ){
                        // Do nothing
                    }else {
                        var coordSplitList = $scope.selectedOrg.LatLong.split(" ");
                        for(var i = 0; i < coordSplitList.length; i++)
                        {
                            //Coords.push(new google.maps.LatLng(parseFloat(coordSplitList[i].split(",")[0]),parseFloat(coordSplitList[i].split(",")[1])));
                        }
                        console.log(Coords);


                        // Construct the polygon.
                        //myPolygon = new google.maps.Polygon({
                        //    paths: Coords,
                        //    strokeColor: '#FF0000',
                        //    strokeOpacity: 0.8,
                        //    strokeWeight: 3,
                        //    fillColor: '#FF0000',
                        //    fillOpacity: 0.35
                        //});
                        //console.log(myPolygon);
                        //myPolygon.setMap(map);
                        //map.fitBounds(myPolygon.getBounds());
                        //  map.getBoundsZoomLevel(myPolygon.getBounds());

                        // Add a listener for the click event.
                        //google.maps.event.addListener(myPolygon, 'click', showArrays);

                    }

                    //infoWindow = new google.maps.InfoWindow();
                }

                /* * @this {google.maps.Polygon} */
                function showArrays(event) {
                    // Since this polygon has only one path, we can call getPath() to return the
                    // MVCArray of LatLngs.

                    var item = this.obj;


                    var contentString = '<b>Project</b><br>' +
                        'Project Name: '+ item.ProjectName +
                        '<br>';

                    // Iterate over the vertices.
                    console.log(event.latLng);
                    // Replace the info window's content and position.
                    infoWindow.setContent(contentString);
                    infoWindow.setPosition(event.latLng);

                    infoWindow.open(map);
                }

                //google.maps.event.addDomListener(window, 'load', initialize);

            function ProjectControl(controlDiv, map) {
                // Set CSS for the control border.
                var controlUI = document.createElement('div');
                controlUI.style.backgroundColor = '#fff';
                controlUI.style.border = '2px solid #fff';
                controlUI.style.borderRadius = '3px';
                controlUI.style.boxShadow = '0 2px 6px rgba(0,0,0,.3)';
                controlUI.style.cursor = 'pointer';
                controlUI.style.marginBottom = '22px';
                controlUI.style.textAlign = 'center';
                controlUI.style.marginTop = "10px";
                controlUI.style.marginRight = "10px";
                controlUI.title = 'Click to display projects';
                controlUI.style.backgroundColor="lightblue";
                controlDiv.appendChild(controlUI);

                // Set CSS for the control interior.
                var controlText = document.createElement('div');
                controlText.style.color = 'rgb(25,25,25)';
                controlText.style.fontFamily = 'Roboto,Arial,sans-serif';
                controlText.style.fontSize = '16px';
                controlText.style.lineHeight = '38px';
                controlText.style.paddingLeft = '5px';
                controlText.style.paddingRight = '5px';
                controlText.innerHTML = 'Project';
                controlUI.appendChild(controlText);

                // Setup the click event listeners: simply set the map to Chicago.
                controlUI.addEventListener('click', function() {
                    $http.get(serviceBasePath + "Request/ProjLocation/" + $scope.selectedOrg.OrganizationID)
                        .then(function(response){

                            if(projList == null) {
                                projList = response.data.result;

                                angular.forEach(projList, function (item) {
                                    var lat = item.LatLong;
                                    var Coords = [];
                                    var coordSplitList = lat.split(" ");
                                    for (var i = 0; i < coordSplitList.length; i++) {
                                        //Coords.push(new google.maps.LatLng(parseFloat(coordSplitList[i].split(",")[0]), parseFloat(coordSplitList[i].split(",")[1])));
                                    }

                                    var polygonObj = new google.maps.Polygon({
                                        paths: Coords,
                                        strokeColor: 'blue',
                                        strokeOpacity: 0.8,
                                        strokeWeight: 2,
                                        fillColor: 'blue',
                                        fillOpacity: 0.35,
                                        zIndex: 9999,
                                        obj: item
                                    });
                                    polygonObj.setMap(map);
                                    polygonObj.addListener("mouseover", function () {
                                        //console.log(this);
                                        //showInfo(item,this);
                                        //   $(this).tooltip({title:item.ProjectName,placement:"right"}).tooltip('show');
                                        this.setOptions({fillColor: "#00FF00"});
                                    });

                                    polygonObj.addListener("mouseout", function () {
                                        //infoWindow.close();
                                        this.setOptions({fillColor: "blue"});
                                    });
                                    polygonObj.obj = item;
                                    polygonObj.addListener('click', showArrays, false);
                                    projectPolygonList.push(polygonObj);
                                });
                            }else{
                                //    projectPolygonListprojList
                                projList=null;

                                angular.forEach(projectPolygonList,function(item){
                                    item.setMap(null);
                                });
                                projectPolygonList=[];
                            }
                        });

                });

            }

            //add new Organization
            $scope.newOrganization = function () {
                var scope = $rootScope.$new();
                newOrEdit = "new";
                scope.params = {
                    selectedOrg: null,
                    newOrEdit: newOrEdit
                };
                console.log(scope.params);
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    backdrop: 'static',
                    keyboard: false,
                    templateUrl: 'app/views/modal/organization_modal.html',
                    controller: 'OrganizationModalCtrl',
                    size: 'lg'
                });
                $rootScope.modalInstance.result.then(function (response) {
                    console.log(response);

                    Organization.lookup().get({}, function (organizationData) {
                        $scope.organizationList = organizationData.result;
                        if (response == 'new success') {
                            $scope.selectedOrg = $scope.organizationList[0];
                        }
                    });

                });

            };

            //Delete a Organization Item
            $scope.deleteOrganization = function () {
                // var url = 'http://192.168.0.19:1832/response/activityCategory/';
                //var url = 'http://localhost:29986/api/response/activityCategory/';
                var index = $scope.organizationList.indexOf($scope.selectedOrg);
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = { confirm: $scope.confirm };

                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,
                    templateUrl: 'app/views/Modal/confirmation_dialog.html',
                    controller: 'ConfirmationCtrl',
                    size: 'sm'
                });

                $rootScope.modalInstance.result.then(function (data) {
                    if (scope.params.confirm === 'yes') {
                        Organization.persist().save({
                            "Operation": 3,
                            "OrganizationID": $scope.selectedOrg.OrganizationID,
                            "OrganizationName": $scope.selectedOrg.OrganizationName,
                            "ShapeType": $scope.selectedOrg.ShapeType,
                            "OrganizationAddress": $scope.selectedOrg.OrganizationAddress,
                            "GeocodedLocation": $scope.selectedOrg.GeocodedLocation,
                            "RGBValue": $scope.selectedOrg.RGBValue,
                            "LatLong": $scope.selectedOrg.LatLong


                        }).$promise.then(function success(response) {
                            console.log(response);
                            if (response.result.indexOf("successfully") >= 0) {
                                dhtmlx.alert("Record Deleted.");
                                $scope.organizationList.splice(index, 1);
                                $scope.selectedOrg = null;
                                initialize();
                            }
                            else {
                                dhtmlx.alert("Failed to be deleted due to dependencies");
                            }
                        }, function error(response) {
                            dhtmlx.alert("Failed to delete. Please contact your administrator!");
                        });


                    }

                });


            };

            //open a modal to edit data
            $scope.editOrganization = function(){
                var scope = $rootScope.$new();
                newOrEdit = "edit";
                scope.params = {selectedOrg:$scope.selectedOrg, newOrEdit:newOrEdit};
                console.log(scope.params);
                $rootScope.modalInstance = $uibModal.open({
                    scope: scope,

                    backdrop: 'static',
                    keyboard: false,
                    templateUrl: 'app/views/modal/organization_modal.html',
                    controller:'OrganizationModalCtrl',
                    size: 'lg'
                });
                    console.log($scope.modalInstance);
                // after close the popup modal

                $rootScope.modalInstance.result.then(function () {
                    Organization.lookup().get({},function(organizationData){
                        $scope.organizationList = organizationData.result;

                    });
                    $scope.selectedOrg=null;


                });
            };


        }]);