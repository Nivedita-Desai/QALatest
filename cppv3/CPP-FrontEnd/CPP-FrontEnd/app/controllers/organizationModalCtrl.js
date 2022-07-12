angular.module('cpp.controllers').
    controller('OrganizationModalCtrl', ['$timeout','$http', '$scope', '$rootScope', '$uibModal', 'Organization', '$uibModalInstance','$location','$timeout',
        function ($timeout,$http, $scope, $rootScope, $uibModal, Organization, $uibModalInstance, $location,$timeout) {

            //  Page.setTitle('Organization');
            $timeout(function(){
                var modal = angular.element(document.getElementById("orgModal"));
                modal = $(modal).closest('.modal-dialog');
                $(modal).css('top','');
                $(modal).css('left','');
                $(modal).draggable({
                    handle: ".modal-header"
                });
            },1000);

            $scope.phaseControlList = [];//store list of phase Controls
            var loc = $location.path();
            var map;
            if(loc == "/app/wbs"){
                $scope.isOrgScreen = false;
            }else{
                $scope.isOrgScreen = true;
            }

            if ($scope.params.newOrEdit === "edit" && $scope.params) {
                $scope.selectedOrg = angular.copy($scope.params.selectedOrg);
                $scope.originalOrg = angular.copy($scope.params.selectedOrg);
                console.log($scope.originalOrg);
                $scope.saveChanges = function () {
                    Organization.persist().save({
                        "Operation": 2,
                        "OrganizationID": $scope.selectedOrg.OrganizationID,
                        "OrganizationName": $scope.selectedOrg.OrganizationName,
                        "ShapeType": $scope.selectedOrg.ShapeType,
                        "OrganizationAddress": $scope.selectedOrg.OrganizationAddress,
                        "GeocodedLocation": $scope.selectedOrg.GeocodedLocation,
                        "RGBValue": $scope.selectedOrg.RGBValue,
                        "LatLong": $scope.selectedOrg.LatLong,
                        "Note": $scope.selectedOrg.Note,
                        "CreatedDate": $scope.selectedOrg.CreatedDate,
                        "UpdatedDate": $scope.selectedOrg.UpdatedDate
                        
                    }).$promise.then(function (response) {
                        if (response.result) {
                            dhtmlx.alert(response.result);
                            $uibModalInstance.close();
                        } else {
                            dhtmlx.alert('No changes saved');
                        }

                        //if (response.result === 'Success') {
                        //    $scope.$close({isCancel:false});
                        //} else {
                        //    dhtmlx.alert("Failed to update Organization");
                        //}
                        })
                }
            }
            else if ($scope.params.newOrEdit === 'new' && $scope.params) {
                $scope.selectedOrg = {
                    OrganizationID: '',
                    OrganizationName: '',
                    ShapeType: '',
                    OrganizationAddress: '',
                    GeocodedLocation: '',
                    RGBValue: '',
                    LatLong: '',
                    Note: ''
                }
                $scope.originalOrg = angular.copy($scope.selectedOrg);
                console.log($scope.originalOrg);
                $scope.saveChanges = function () {
                    Organization.persist().save({
                        "Operation": 1,
                        "OrganizationName": $scope.selectedOrg.OrganizationName,
                        "ShapeType": $scope.selectedOrg.ShapeType,
                        "OrganizationAddress": $scope.selectedOrg.OrganizationAddress,
                        "GeocodedLocation": $scope.selectedOrg.GeocodedLocation,
                        "RGBValue": $scope.selectedOrg.RGBValue,
                        "LatLong": $scope.selectedOrg.LatLong,
                        "Note": $scope.selectedOrg.Note,
                        "CreatedDate": $scope.selectedOrg.CreatedDate,
                        "UpdatedDate": $scope.selectedOrg.UpdatedDate

                    }).$promise.then(function (response) {
                        console.log(response);
                        if (response.result) {
                            dhtmlx.alert(response.result);
                            $uibModalInstance.close('new success');
                        } else {
                            dhtmlx.alert('No changes saved');
                        }

                        //if (response.result === 'Success') {
                        //    $scope.$close({isCancel:false});
                        //} else {
                        //    dhtmlx.alert('Failed to add new Organization');
                        //}
                        })
                }
            }


            function update() {
                if ($scope.params.newOrEdit === "edit" && $scope.params) {
                    Organization.persist().save({
                        "Operation": 2,
                        "OrganizationID": $scope.selectedOrg.OrganizationID,
                        "OrganizationName": $scope.selectedOrg.OrganizationName,
                        "ShapeType": $scope.selectedOrg.ShapeType,
                        "OrganizationAddress": $scope.selectedOrg.OrganizationAddress,
                        "GeocodedLocation": $scope.selectedOrg.GeocodedLocation,
                        "RGBValue": $scope.selectedOrg.RGBValue,
                        "LatLong": $scope.selectedOrg.LatLong,
                        "Note": $scope.selectedOrg.Note


                    }).$promise.then(function (response) {
                            if (response.result === 'Success') {
                                console.log(response);
                                $scope.$close({isCancel:false});
                            } else {
                                alert("Failed to update Organization");
                            }
                        })
                }
                else if ($scope.params.newOrEdit === 'new' && $scope.params) {


                    console.log($scope.selectedOrg);
                    Organization.persist().save({
                        "Operation": 1,
                        "OrganizationName": $scope.selectedOrg.OrganizationName,
                        "ShapeType": $scope.selectedOrg.ShapeType,
                        "OrganizationAddress": $scope.selectedOrg.OrganizationAddress,
                        "GeocodedLocation": $scope.selectedOrg.GeocodedLocation,
                        "RGBValue": $scope.selectedOrg.RGBValue,
                        "LatLong": $scope.selectedOrg.LatLong


                    }).$promise.then(function (response) {
                            //  console.log(response);

                            if (response.result === 'Success') {
                                $scope.$close({isCancel : true});
                            } else {
                                alert('Failed to add new Organization');
                            }
                        })

                }
            }

            $scope.goBack = function () {
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = {confirm: $scope.confirm};
                var isChange = isOrgChange();
                if (isChange == true) {
                    $rootScope.modalInstance = $uibModal.open({
                        scope: scope,
                        templateUrl: 'app/views/Modal/exit_confirmation_modal.html',
                        controller: 'exitConfirmation',
                        size: 'md',
                        backdrop: true
                    });

                    $rootScope.modalInstance.result.then(function (data) {
                        if (scope.params.confirm === 'exit') {
                            $uibModalInstance.close({isCancel:true});

                        }
                        else if (scope.params.confirm === "save") {
                            update();

                        } else if (scope.params.confirm === "back") {

                        }
                    });
                } else {
                    $uibModalInstance.close({isCancel : true});
                }

            }
            function isOrgChange() {
                var result = true;
                //if($scope.selectedOrg.OrganizationName == $scope.originalOrg.OrganizationName &&
                //    $scope.selectedOrg.OrganizationAddress == $scope.originalOrg.OrganizationAddress &&
                //     $scope.selectedOrg.Note == $scope.originalOrg.Note){
                //    result = false;
                //}
                if (JSON.stringify($scope.selectedOrg) == JSON.stringify($scope.originalOrg)) {
                    result = false;
                }
                return result;
            }

            $scope.init = function () {

//This variable gets all coordinates of polygone and save them. Finally you should use this array because it contains all latitude and longitude coordinates of polygon.
                var coordinates = "";

//This variable saves polygon.
                var myLatlang = null;//;/new google.maps.LatLng(37.09024, -95.712891);
                var myPolygon = '';
                var projList;
                var projectPolygonList=[];
//This function save latitude and longitude to the polygons[] variable after we call it.
                function save_coordinates_to_array(polygon) {
                    //Save polygon to 'polygons[]' array to get its coordinate.
                    if (myPolygon == '')
                        myPolygon = polygon;
                    else {
                        myPolygon.setMap(null);
                        myPolygon = polygon;
                    }
                    //This variable gets all bounds of polygon.
                    var polygonBounds = polygon.getPath();
                    coordinates = "";
                    for (var i = 0; i < polygonBounds.length; i++) {
                        //coordinates.push(polygonBounds.getAt(i).lat(), polygonBounds.getAt(i).lng());
                        coordinates += polygonBounds.getAt(i).lat() + "," + polygonBounds.getAt(i).lng() + " ";
                    }
                    // set only one drawing
                    $scope.selectedOrg.LatLong = coordinates;
                }

                window.setTimeout(function () {
                    initialize()
                }, 1000);
            //Create a button to show list of projects within organization

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
                                    //phaseControl(map);
                                    //console.log(map.controls[google.maps.ControlPosition.TOP_RIGHT]);
                                    angular.forEach(projList, function (item) {

                                        var lat = item.LatLong;
                                        var Coords = [];
                                        var coordSplitList = lat.split(" ");
                                        for (var i = 0; i < coordSplitList.length; i++) {
                                            //Coords.push(new google.maps.LatLng(parseFloat(coordSplitList[i].split(",")[0]), parseFloat(coordSplitList[i].split(",")[1])));
                                        }

                                        //var polygonObj = new google.maps.Polygon({
                                        //    paths: Coords,
                                        //    strokeColor: 'blue',
                                        //    strokeOpacity: 0.8,
                                        //    strokeWeight: 2,
                                        //    fillColor: 'blue',
                                        //    fillOpacity: 0.35,
                                        //    zIndex: 9999,
                                        //    obj: item
                                        //});
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
                                    //var topRightControls = map.controls[google.maps.ControlPosition.TOP_RIGHT];
                                    angular.forEach(projectPolygonList,function(item){
                                        item.setMap(null);
                                    });
                                }
                            });
                    });
                }

                function showInfo(item,event) {
                    // Since this polygon has only one path, we can call getPath() to return the
                    // MVCArray of LatLngs.
                    var contentString = '<b>Project</b><br>' +
                        'Project Name: '+ item.ProjectName +
                        '<br>';
                    infoWindow.setContent(contentString);
                    infoWindow.setPosition(event.latLngs);
                    infoWindow.open(map);
                }

                function showArrays(event) {
                    // Since this polygon has only one path, we can call getPath() to return the
                    // MVCArray of LatLngs.
                    var item = this.obj;
                    var contentString = '<b>Project</b><br>' +
                        'Project Name: '+ item.ProjectName +
                        '<br>';
                    infoWindow.setContent(contentString);
                    infoWindow.setPosition(event.latLng);
                    infoWindow.open(map);
                }
                function initialize() {
                    var cancelDrawingShape = false;
                    var disablePolygonComplete = false;
                    var mapOptions = {
                        zoom: 4,
                        center: myLatlang,
                        //mapTypeId: google.maps.MapTypeId.TERRAIN,
                        //zoomControlOptions: {
                        //    style: google.maps.ZoomControlStyle.SMALL,
                        //    position: google.maps.ControlPosition.LEFT_CENTER // TBD: Position Can be changes
                        //}
                    };
                    //offset for button in top_center
                    // map = new google.maps.Map(document.getElementById('map-canv'),
                    //    mapOptions);
                    //var draggingBtn = document.getElementsByClassName("gmnoprint");
                    //map.setCenter(myLatlang);
                    //google.maps.event.trigger(map, 'resize');
                //    var drawingManager = new google.maps.drawing.DrawingManager(
                //        {
                //            drawingControl: true,
                //            drawingControlOptions: {
                //                position: google.maps.ControlPosition.TOP_CENTER  ,
                //                style: {"margin-top":"10px"},
                //                drawingModes: [
                //                    //google.maps.drawing.OverlayType.MARKER,
                //                    //google.maps.drawing.OverlayType.CIRCLE,
                //                    google.maps.drawing.OverlayType.POLYGON
                //                    //google.maps.drawing.OverlayType.POLYLINE,
                //                    //google.maps.drawing.OverlayType.RECTANGLE
                //                ]
                //            }
                //        }
                //    );
                //console.log(drawingManager);
                    //console.log(google.maps);
                // Create a button for GIS toggle projects location
                    var projectoCntrolDiv = document.createElement('div');
                    var projectControl = new ProjectControl(projectoCntrolDiv, map);
                    projectoCntrolDiv.index = 1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(projectoCntrolDiv);
                //Create a drawing manager panel that lets you select polygon, polyline, circle, rectangle or etc and then draw it.
                    //drawingManager.setMap(map);
                    var input = (document.getElementById('address-bar'));
                    //var searchBox = new google.maps.places.SearchBox(input);
                    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
                    //map.addListener('bounds_changed', function () {
                    //    searchBox.setBounds(map.getBounds());
                    //});
                    var markers = [];
                    // Listen for the event fired when the user selects a prediction and retrieve
                    // more details for that place.
                    //var autocomplete = new google.maps.places.Autocomplete(input,
                    //    {componentRestrictions: {country: 'us'}});
                    //autocomplete.bindTo('bounds', map);
                    //searchBox.addListener('places_changed', function () {
                    //    var places = searchBox.getPlaces();
                    //    console.log(places);
                    //    if (places.length == 0) {
                    //        return;
                    //    }
                    //    else {
                    //        //$scope.selectedOrg.OrganizationAddress = places.formatted_address;
                    //        //$scope.selectedOrg.OrganizationAddress = "Hello";
                    //    }
                    //    // Clear out the old markers.
                    //    markers.forEach(function (marker) {
                    //        marker.setMap(null);
                    //    });
                    //    markers = [];
                    //    var bounds = new google.maps.LatLngBounds();
                    //    places.forEach(function (place) {
                    //        var icon = {
                    //            url: place.icon,
                    //            size: new google.maps.Size(71, 71),
                    //            origin: new google.maps.Point(0, 0),
                    //            anchor: new google.maps.Point(17, 34),
                    //            scaledSize: new google.maps.Size(25, 25)
                    //        };

                    //        // Create a marker for each place.
                    //        markers.push(new google.maps.Marker({
                    //            map: map,
                    //            icon: icon,
                    //            title: place.name,
                    //            position: place.geometry.location
                    //        }));

                    //        if (place.geometry.viewport) {
                    //            // Only geocodes have viewport.
                    //            bounds.union(place.geometry.viewport);
                    //        } else {
                    //            bounds.extend(place.geometry.location);
                    //        }
                    //    });
                    //    map.fitBounds(bounds);
                    //});

                    if ($scope.params.newOrEdit === "edit" && $scope.params) {
                        var Coords = [];
                        if ($scope.selectedOrg.LatLong == "") {
                        } else {

                            var coordSplitList = $scope.selectedOrg.LatLong.split(" ");
                            for (var i = 0; i < coordSplitList.length; i++) {
                                Coords.push(new google.maps.LatLng(parseFloat(coordSplitList[i].split(",")[0]), parseFloat(coordSplitList[i].split(",")[1])));
                            }
                             console.log(Coords);


                            // Construct the polygon.
                            myPolygon = new google.maps.Polygon({
                                paths: Coords,
                                strokeColor: '#FF0000',
                                strokeOpacity: 0.8,
                                strokeWeight: 3,
                                fillColor: '#FF0000',
                                fillOpacity: 0.35
                            });

                            myPolygon.setMap(map);
                            map.fitBounds(myPolygon.getBounds());
                            console.log(myPolygon);
                        }

                    }

                    //This event fires when creation of polygon is completed by user.
                    //google.maps.event.addDomListener(drawingManager, 'polygoncomplete', function (polygon) {
                    //    //This line make it possible to edit polygon you have drawed.
                    //    //
                    //    if(disablePolygonComplete){
                    //        disablePolygonComplete = false;
                    //        return;
                    //    }
                    //    polygon.setEditable(true);
                    //    polygon.setDraggable(true);
                    //    //Call function to pass polygon as parameter to save its coordinated to an array.
                    //  save_coordinates_to_array(polygon);

                    //    //This event is inside 'polygoncomplete' and fires when you edit the polygon by moving one of its anchors.
                    //    google.maps.event.addListener(polygon.getPath(), 'set_at', function () {
                    //        save_coordinates_to_array(polygon);
                    //    });

                    //    //This event is inside 'polygoncomplete' too and fires when you edit the polygon by moving on one of its anchors.
                    //    google.maps.event.addListener(polygon.getPath(), 'insert_at', function () {
                    //        save_coordinates_to_array(polygon);
                    //    });
                    //});

                    //google.maps.event.addDomListener(drawingManager, 'rectanglecomplete', function (rectangle) {
                    //    //This line make it possible to edit polygon you have drawed.

                    //    rectangle.setEditable(true);
                    //    polygon.setDraggable(true);

                    //    //Call function to pass polygon as parameter to save its coordinated to an array.
                    //    var bounds = rectangle.getBounds();
                    //    var NE = bounds.getNorthEast();
                    //    var SW = bounds.getSouthWest();
                    //    coordinates = NE.A + "," + NE.F + "," + SW.A + "," + SW.F;


                    //    //This event is inside 'polygoncomplete' and fires when you edit the polygon by moving one of its anchors.
                    //    google.maps.event.addListener(rectangle, 'bounds_changed', function () {
                    //        var bounds = rectangle.getBounds();
                    //        var NE = bounds.getNorthEast();
                    //        var SW = bounds.getSouthWest();
                    //        coordinates = NE[0] + "," + NE[1] + "," + SW[0] + "," + SW[1];
                    //    });

                    //    //This event is inside 'polygoncomplete' too and fires when you edit the polygon by moving on one of its anchors.
                    //    /*google.maps.event.addListener(polygon.getPath(), 'insert_at', function () {
                    //     save_coordinates_to_array(polygon);
                    //     });*/
                    //});

                    //google.maps.event.addDomListener(document,'keyup',function(e,polygon){
                    //    var code = (e.keyCode ? e.keyCode : e.which);

                    //    if (code === 46) {
                    //        cancelDrawingShape = true;
                    //        drawingManager.setDrawingMode(null);
                    //        // polygon.setMap(null);
                    //    }
                    //});
                    //google.maps.event.addListener(drawingManager, 'overlaycomplete', function (e) {
                    //    var lastDrawnShape = e.overlay;
                    //    console.log(e);
                    //    console.debug(lastDrawnShape);
                    //    if (cancelDrawingShape) {
                    //        cancelDrawingShape = false;
                    //        disablePolygonComplete = true;
                    //        lastDrawnShape.setMap(null); // Remove drawn but unwanted shape
                    //    }

                    //});

                }

                document.onkeydown = function (event) {
                    if (46 == event.keyCode) {
                        if (null != selected_shape) {
                            selected_shape.setMap(null);
                            infowindow.close();
                            shapes.pop();
                            selected_shape = null;
                        }
                    }
                }

                //    google.maps.event.addDomListener(window, 'load', initialize);

                //call on page load
                //google.maps.event.addDomListener(window, 'page:load', initialize);

                function OnCustomColorChanged(selectedColor, selectedColorTitle, colorPickerIndex) {
                    var r = colorNameToHex(selectedColorTitle);
                    selected_shape.set('fillColor', r);
                }

                function colorNameToHex(colour) {

                    var colours = {
                        "black": "#292421",
                        "red": "#B0171F",
                        "sand": "#A49381",
                        "charcoal": "#626878",
                        "navy blue": "#2E436E",
                        "pink": "#FF00FF"
                    };

                    if (typeof colours[colour.toLowerCase()] != 'undefined')
                        return colours[colour.toLowerCase()];

                    return false;
                }



                function clearScreen() {
                    var temp = shapes.length;
                    while (0 !== temp) {
                        var shape = shapes.pop();
                        shape.setMap(null);
                        temp--;
                    }
                }

                //Sidebar Code Start


                //window.onload = function ()
                //{
                //    enable_text(false);
                //    var newBtnObj = document.getElementById("newbtn-section")
                //    if (newBtnObj != null) {
                //        newBtnObj.style.visibility = "hidden";
                //    }
                //
                //}
                //;


                function enable_text(status) {

                    var usrAdrObj = document.getElementById('user-addr');
                    if (usrAdrObj) {
                        if (true === status)
                            usrAdrObj.value = document.getElementById('address-bar').value;
                        else
                            usrAdrObj.value = null;
                    }
                }

            }
            $uibModalInstance.opened.then($scope.init);
            function phaseControl(map){
                //PLN BTN
                $timeout(function(){
                    var plnControlDiv = document.createElement('div');
                    plnControlDiv.style.marginTop = "50px";
                    plnControlDiv.style.setProperty("margin-right","-75px","important");// = "0px";
                    var plnControl = new PlanningControl(plnControlDiv,map);
                    plnControlDiv.index=1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(plnControlDiv);
                    $scope.phaseControlList.push(plnControlDiv);
                },100);

                //SD BTN
                $timeout(function(){
                    var sdControlDiv = document.createElement('div');
                    sdControlDiv.style.marginTop = "100px";
                    sdControlDiv.style.setProperty("margin-right","-75px","important");
                    var sdControl = new SDControl(sdControlDiv,map);
                    sdControlDiv.index=1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(sdControlDiv);
                    $scope.phaseControlList.push(sdControlDiv);
                },150);


                //DB BTN
                $timeout(function(){
                    var dbControlDiv = document.createElement('div');
                    dbControlDiv.style.marginTop = "150px";
                    dbControlDiv.style.setProperty("margin-right","-75px","important");
                    var dbControl = new DBControl(dbControlDiv,map);
                    dbControlDiv.index=1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(dbControlDiv);
                    $scope.phaseControlList.push(dbControlDiv);
                },200);

                //CON BTN
                $timeout(function(){
                    var conControlDiv = document.createElement('div');
                    conControlDiv.style.marginTop = "200px";
                    conControlDiv.style.setProperty("margin-right","-75px","important");
                    var conControl = new CONControl(conControlDiv,map);
                    conControlDiv.index=1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(conControlDiv);
                    $scope.phaseControlList.push(conControlDiv);
                },250);

                //CLO BTN
                $timeout(function(){
                    var cloControlDiv = document.createElement('div');
                    cloControlDiv.style.marginTop = "250px";
                    cloControlDiv.style.setProperty("margin-right","-75px","important");
                    var cloControl = new CLOControl(cloControlDiv,map);
                    cloControlDiv.index=1;
                    //map.controls[google.maps.ControlPosition.TOP_RIGHT].push(cloControlDiv);
                    $scope.phaseControlList.push(cloControlDiv);
                },300);

                //
            }

            function CLOControl(controlDiv, map) {
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
                controlUI.style.width= "66.5313px";
                controlUI.title = 'Closeout';
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
                controlText.innerHTML = 'CLO';
                controlUI.appendChild(controlText);

                controlUI.addEventListener("click",function(){
                });
                // Setup the click event listeners: simply set the map to Chicago.

            }

            function CONControl(controlDiv, map) {
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
                controlUI.style.width= "66.5313px";
                controlUI.title = 'Construction';
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
                controlText.innerHTML = 'CON';
                controlUI.appendChild(controlText);

                controlUI.addEventListener("click",function(){
                });
                // Setup the click event listeners: simply set the map to Chicago.

            }

            function DBControl(controlDiv, map) {
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
                controlUI.style.width= "66.5313px";
                controlUI.title = 'Design & Bidding';
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
                controlText.innerHTML = 'D&B';
                controlUI.appendChild(controlText);

                controlUI.addEventListener("click",function(){
                    alert("Design and Bidding");
                });
                // Setup the click event listeners: simply set the map to Chicago.

            }

            function SDControl(controlDiv, map) {
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
                controlUI.style.width= "66.5313px";
                controlUI.title = 'Schematic Design';
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
                controlText.innerHTML = 'SD';
                controlUI.appendChild(controlText);

                controlUI.addEventListener("click",function(){
                    alert("Schematic Design");
                });
                // Setup the click event listeners: simply set the map to Chicago.

            }
            function PlanningControl(controlDiv, map) {
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
                controlUI.style.width= "66.5313px";
                controlUI.title = 'Planning Phase';
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
                controlText.innerHTML = 'PLN';
                controlUI.appendChild(controlText);


                controlUI.addEventListener("click",function(){
                    alert("Planning");
                });
                // Setup the click event listeners: simply set the map to Chicago.

            }
            $scope.deleteOrganization = function(){
                // var url = 'http://192.168.0.19:1832/response/activityCategory/';
                //var url = 'http://localhost:29986/api/response/activityCategory/';
              //  var index = $scope.organizationList.indexOf($scope.selectedOrg);
                console.log($location.path());
                if($location.path() === "/app/wbs") {
                    //$('.delete_message')
                    $('#delete_button').on('click',function(){
                    });
                    return;
                };
                $scope.confirm = "";
                var scope = $rootScope.$new();
                scope.params = {confirm:$scope.confirm};

                $rootScope.modalInstance = $uibModal.open({
                    scope:scope,
                    templateUrl: 'app/views/Modal/confirmation_dialog.html',
                    controller:'ConfirmationCtrl',
                    size :'sm'
                });

                $rootScope.modalInstance.result.then(function(data){
                    if(scope.params.confirm ==='yes'){
                        Organization.persist().save({
                            "Operation": 3,
                            "OrganizationID" :$scope.selectedOrg.OrganizationID,
                            "OrganizationName" :$scope.selectedOrg.OrganizationName,
                            "ShapeType" :$scope.selectedOrg.ShapeType,
                            "OrganizationAddress" :$scope.selectedOrg.OrganizationAddress,
                            "GeocodedLocation" :$scope.selectedOrg.GeocodedLocation,
                            "RGBValue" :$scope.selectedOrg.RGBValue,
                            "LatLong" :$scope.selectedOrg.LatLong


                        }).$promise.then(function(response){
                            console.log(response);
                                 if(response.result.indexOf("successfully") >= 0){
                                  //  $scope.organizationList.splice(index,1);
                                    dhtmlx.alert(response.result);
                                    $uibModalInstance.close();

                                    $scope.selectedOrg = null;
                                    $scope.init();
                                }
                                else{
                                    dhtmlx.alert("Failed to be deleted due to dependencies.");
                                }
                            });

                    }

                });


            }
        }]);