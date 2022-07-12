angular.module('cpp.controllers').    
    controller('LocationCtrl', ['$http','$scope','$rootScope','Page',
        function ($http,$scope, $rootScope, Page) {

            Page.setTitle('Project Location');

            //This variable gets all coordinates of polygone and save them. Finally you should use this array                   because it contains all latitude and longitude coordinates of polygon.
            var coordinates = "";

            //This variable saves polygon.
            var polygons = [];

            var myLatlang = new google.maps.LatLng(34.02000, -118.15300);

            //This function save latitude and longitude to the polygons[] variable after we call it.
            function save_coordinates_to_array(polygon)
            {
                //Save polygon to 'polygons[]' array to get its coordinate.
                polygons.push(polygon);

                //This variable gets all bounds of polygon.
                var polygonBounds = polygon.getPath();
                coordinates = "";
                for(var i = 0 ; i < polygonBounds.length ; i++)
                {
                    //coordinates.push(polygonBounds.getAt(i).lat(), polygonBounds.getAt(i).lng());
                    coordinates += polygonBounds.getAt(i).lat() + "," + polygonBounds.getAt(i).lng();
                }
            }

            initialize();

            function initialize()
            {
                var mapOptions = {
                    center: myLatlang,
                    zoom: 10,
                    //For Small Zoom Control Options
                    zoomControl: true,
                    zoomControlOptions: {
                        style: google.maps.ZoomControlStyle.SMALL,
                        position: google.maps.ControlPosition.RIGHT_CENTER // TBD: Position Can be changes
                    }
                };

                var map = new google.maps.Map(document.getElementById('map-canvas'),
                    mapOptions);

                //Create a drawing manager panel that lets you select polygon, polyline, circle, rectangle or etc and then draw it.
                var drawingManager = new google.maps.drawing.DrawingManager(
                    {
                        drawingControl: true,
                        drawingControlOptions: {
                            position: google.maps.ControlPosition.TOP_CENTER,
							drawingModes: [
								//google.maps.drawing.OverlayType.MARKER,
								//google.maps.drawing.OverlayType.CIRCLE,
								google.maps.drawing.OverlayType.POLYGON
								//google.maps.drawing.OverlayType.POLYLINE,
								//google.maps.drawing.OverlayType.RECTANGLE
							  ]
                        }
                    }
                );
                drawingManager.setMap(map);

                var input = (
                    document.getElementById('address-bar'));

                map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

                var autocomplete = new google.maps.places.Autocomplete(input);
                autocomplete.bindTo('bounds', map);

                //This event fires when creation of polygon is completed by user.
                google.maps.event.addDomListener(drawingManager, 'polygoncomplete', function (polygon) {
                    //This line make it possible to edit polygon you have drawed.
                    polygon.setEditable(true);
                    polygon.setDraggable(true);
                    //Call function to pass polygon as parameter to save its coordinated to an array.
                    save_coordinates_to_array(polygon);

                    //This event is inside 'polygoncomplete' and fires when you edit the polygon by moving one of its anchors.
                    google.maps.event.addListener(polygon.getPath(), 'set_at', function () {
                        save_coordinates_to_array(polygon);
                    });

                    //This event is inside 'polygoncomplete' too and fires when you edit the polygon by moving on one of its anchors.
                    google.maps.event.addListener(polygon.getPath(), 'insert_at', function () {
                        save_coordinates_to_array(polygon);
                    });
                });

                google.maps.event.addDomListener(drawingManager, 'rectanglecomplete', function (rectangle) {
                    //This line make it possible to edit polygon you have drawed.
                    rectangle.setEditable(true);
                    polygon.setDraggable(true);

                    //Call function to pass polygon as parameter to save its coordinated to an array.
                    var bounds = rectangle.getBounds();
                    var NE = bounds.getNorthEast();
                    var SW = bounds.getSouthWest();
                    coordinates = NE.A + "," + NE.F + "," + SW.A + "," + SW.F;


                    //This event is inside 'polygoncomplete' and fires when you edit the polygon by moving one of its anchors.
                    google.maps.event.addListener(rectangle, 'bounds_changed', function () {
                        var bounds = rectangle.getBounds();
                        var NE = bounds.getNorthEast();
                        var SW = bounds.getSouthWest();
                        coordinates = NE[0] + "," + NE[1] + "," + SW[0] + "," + SW[1];
                    });

                    //This event is inside 'polygoncomplete' too and fires when you edit the polygon by moving on one of its anchors.
                    /*google.maps.event.addListener(polygon.getPath(), 'insert_at', function () {
                        save_coordinates_to_array(polygon);
                    });*/
                });
            }

            document.onkeydown = function(event) {
                if(46 == event.keyCode){
                    if(null != selected_shape){
                        selected_shape.setMap(null);
                        infowindow.close();
                        shapes.pop();
                        selected_shape = null;
                    }
                }
            }

            google.maps.event.addDomListener(window, 'load', initialize);


            function OnCustomColorChanged(selectedColor, selectedColorTitle, colorPickerIndex) {

                var r = colorNameToHex(selectedColorTitle);

                selected_shape.set('fillColor', r);

            }

            function colorNameToHex(colour){

                var colours = {"black":"#292421","red":"#B0171F","sand":"#A49381","charcoal":"#626878","navy blue":"#2E436E","pink":"#FF00FF"};

                if (typeof colours[colour.toLowerCase()] != 'undefined')
                    return colours[colour.toLowerCase()];

                return false;
            }

            function clearScreen(){
                var temp = shapes.length;
            }

            function clearScreen(){
                var temp = shapes.length;
                while(0 !== temp)
                {
                    var shape = shapes.pop();
                    shape.setMap(null);
                    temp--;
                }
            }

            //Sidebar Code Start

            var obj;

            //window.onload = function ()
            {
                enable_text(false);
				var newBtnObj = document.getElementById("newbtn-section");
				if(newBtnObj){
					newBtnObj.style.visibility = "hidden";
				}

                //To Populate Program
                $http.get(serviceBasePath+'Request/Program').then(function(test){

                    console.log(test);
                    var numberOfRows = test.data.result.length;
                    obj = test.data.result[0];

                    var select = document.getElementById("program");

                    for(var i = 0; i < numberOfRows; i++) {
                        var option = document.createElement('option');
                        option.text = option.value = test.data.result[i].ProgramName;
                        select.add(option, i);
						
                    }
                });

                //To Populate ProgramElement
                $http.get(serviceBasePath+'Request/ProgramElement/').then(function(test){

                    var numberOfRows = test.data.result.length;
                    obj = test.data.result[0];
                    //console.log(obj);
                    //document.write("Length is :" + numberOfRows);

                    var select = document.getElementById("programElement");

                    //console.log(numberOfRows);
                    for(var i = 0; i < numberOfRows; i++) {
                        var option = document.createElement('option');
                        option.text = option.value = test.data.result[i].ProgramElementName;
                        select.add(option, i);
                    }

                });

                //To Populate Project
                $http.get(serviceBasePath+'Request/Project/').then(function(test){
              //  $.getJSON(serviceBasePath+'Request/Project/', function(test) {

                    var numberOfRows = test.data.result.length;
                    obj = test.data.result[0];
                    //console.log(obj);
                    //document.write("Length is :" + numberOfRows);
                    var temp = test.data.result.length;
                    document.getElementById('project').options.length = 0;
                    //document.getElementById('projID').options.length = 0;
                    var select = document.getElementById("project");
                    for(var i = 0; i < temp ; i++) {
                        var option = document.createElement('option');
                        //var optionID = document.createElement('option');
                        option.text = test.data.result[i].ProjectName;
                        option.value = test.data.result[i].ProjectID;
                        select.add(option, 0);
                        //selectID.add(optionID, 0);
                    }
                }); 
            };

            //function progElementProjFilter(){
            $('#program').on('change', function () {
                //clearScreen();

                var base_url = serviceBasePath+'Request/' ;
                var programSelectedIndex = document.getElementById("program").selectedIndex;
                programSelectedIndex++;

                //Filter ProgramElements
                var total_url = base_url + 'ProgramElement/' + programSelectedIndex;
                    //this is a non examine type of t
                $.getJSON( total_url, function(test) {

                    var select = document.getElementById("programElement");
                    var temp = test.result.length;
                    document.getElementById('programElement').options.length = 0;

                    for(var i = 0; i < temp; i++) {
                        var option = document.createElement('option');
                        option.text = option.value = test.result[i].ProgramElementName;
                        select.add(option, i);
                    }

                });

                //Filter Projects
                var programElemSelectedIndex = document.getElementById("programElement").selectedIndex;
                //programElemSelectedIndex = programElemSelectedIndex + programSelectedIndex;

                var total_url = base_url + 'Project/' + programSelectedIndex;// + '/' + programElemSelectedIndex;

                $.getJSON( total_url, function(test) {

                    var select = document.getElementById("project");
                    //var selectID = document.getElementById("projID");
                    var temp = test.result.length;
                    document.getElementById('project').options.length = 0;
                    //document.getElementById('projID').options.length = 0;

                    for(var i = 0; i < temp ; i++) {
                        var option = document.createElement('option');
                        //var optionID = document.createElement('option');
                        option.text = test.result[i].ProjectName;
                        option.value = test.result[i].ProjectID;
                        select.add(option, 0);
                        //selectID.add(optionID, 0);
                    }
                });
            });

            //function projFilter(){
            $('#programElement').on('change', function () {
                //clearScreen();

                var base_url = serviceBasePath+'Request/Project/' ;
               // var base_url = 'http:/localhost:29986/api/Request/Project/' ;
                var programSelectedIndex = document.getElementById("program").selectedIndex;
                programSelectedIndex++;
                var programElemSelectedIndex = document.getElementById("programElement").selectedIndex;
                programElemSelectedIndex = programElemSelectedIndex + programSelectedIndex;

                //Filter ProgramElements
                var total_url = base_url + programSelectedIndex + '/' + programElemSelectedIndex;

                $.getJSON( total_url, function(test) {

                    var select = document.getElementById("project");
                    var selectID = document.getElementById("projID");

                    var temp = test.result.length;
                    document.getElementById('project').options.length = 0;
                    //document.getElementById('projID').options.length = 0;

                    for(var i = 0; i < temp ; i++) {
                        var option = document.createElement('option');
                        //var optionID = document.createElement('option');
                        option.text = test.result[i].ProjectName;
                        option.value = test.result[i].ProjectID;
                        select.add(option, 0);
                        //selectID.add(optionID, 0);
                    }
                });
            });

            //Sidebar Code End

            //var newBtnClick = function() {
            $('#new_location').on('click', function () {

                document.getElementById("newbtn-section").style.visibility = "visible";
                document.getElementById('address-bar').value = "";
                document.getElementById('user-addr').value = "";
                //drawingManager.setMap(map);
            });

            //function saveBtnClick() {
            $('#save_location').on('click', function () {
                if(coordinates == "")
                    alert("Please Select at least a Shape first!");

                else {
                    //1. ShapeType 2.LocationString 3.ProjectID 4.Color

                    var x = document.getElementById("project").value;


                }

            });

            //function cancelBtnClick() {
            $('#cancel_location').on('click', function () {
                clearScreen();
                drawingManager.setMap(null);
                document.getElementById("newbtn-section").style.visibility = "hidden";
                document.getElementById('address-bar').value = "";
            });

            function enable_text(status) {
				var usrAdrObj = document.getElementById('user-addr');
				if(usrAdrObj){
					if (true === status)
						usrAdrObj.value = document.getElementById('address-bar').value;
					else
						usrAdrObj.value = null;
				}
            }



        }]);