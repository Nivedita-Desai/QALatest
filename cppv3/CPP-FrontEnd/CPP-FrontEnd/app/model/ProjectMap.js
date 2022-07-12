ProjectMap = (function(){    
				
    var obj = function ProjectMap() {
        if (!this instanceof ProjectMap) {
            return new ProjectMap();
        }
	        //This variable gets all coordinates of polygone and save them. Finally you should use this array because it contains all latitude and longitude coordinates of polygon.
			var coordinates = "";

	        //This variable saves polygon.
		    // var polygons = [];
			// default cordination

			var myLatlang = null;//new google.maps.LatLng(34.02000, -118.15300);
			var myPolygon = '';
              var _scope = null;
        var _rootscope = null;
            obj.prototype.getCoordinates = function() {
                return coordinates;
            }   
            obj.prototype.setCoordinates = function(icoordinates) {
                if (icoordinates) {
                    coordinates = icoordinates;
                }
            }


            obj.prototype.getScope = function() {
                return _scope;
            }
            obj.prototype.setScope = function(scope) {
                if (scope) {
                    _scope = scope;
                }
            }

            obj.prototype.getRootScope = function() {
                return _scope;
            }
            obj.prototype.setRootScope = function(rootscope) {
                if (rootscope) {
                    _rootScope = rootscope;
                }
            }

            //This function save latitude and longitude to the polygons[] variable after we call it.
			function save_coordinates_to_array(polygon,m)
			{
                console.log(m);
                if(m)
                myPolygon = m;
				//Save polygon to 'polygons[]' array to get its coordinate.
				if(myPolygon == '')
				    myPolygon = polygon;
				else
				{
					myPolygon.setMap(null);
					myPolygon = polygon;
				}
				//This variable gets all bounds of polygon.
				var polygonBounds = polygon.getPath();
				coordinates = "";
				for(var i = 0 ; i < polygonBounds.length ; i++)
				{
					//coordinates.push(polygonBounds.getAt(i).lat(), polygonBounds.getAt(i).lng());
					coordinates += polygonBounds.getAt(i).lat() + "," + polygonBounds.getAt(i).lng()+" ";
				}
				// set only one drawing
			}
        
            /** @this {google.maps.Polygon} */
            function showArrays(event) {

                // Since this polygon has only one path, we can call getPath()
                // to return the MVCArray of LatLngs.
                var vertices = this.getPath();
                
                var projName = "";
                if(selectedNode){
                    projName = selectedNode.ProjectName;
                }
                var contentString = '<b>'+ projName +'</b><br>' +
                    'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
                    '<br>';

                // Iterate over the vertices.
                for (var i =0; i < vertices.getLength(); i++) {
                    var xy = vertices.getAt(i);
                    contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' +
                    xy.lng();
                }

                // Replace the info window's content and position.
                infoWindow.setContent(contentString);
                infoWindow.setPosition(event.latLng);

                infoWindow.open(map);
            }

            obj.prototype.initProjectEditMap = function (locationLatLong, organizationList) {
                $('#project-map-canvas-edit').html("");
                var myLatlang = new google.maps.LatLng(40.562655, -101.707602); //default
                var zoomLevel = 4;
                var mapOptions = {
					zoom: zoomLevel,
					center: myLatlang,
					mapTypeId: google.maps.MapTypeId.TERRAIN,
					zoomControl: true,
					zoomControlOptions: {
						style: google.maps.ZoomControlStyle.SMALL,
						position: google.maps.ControlPosition.RIGHT_CENTER // TBD: Position Can be changes
					}
				};

                var map = new google.maps.Map(document.getElementById('project-map-canvas-edit'),
					mapOptions);
                
                var Coords = [];
                var orgScoped = false;
                var orgName = "";
                var markers = [];

                //Search by location box

                    var emptyDiv = document.createElement('div');
                    emptyDiv.style.width = "200px";
                    emptyDiv.style.height = "100px";
                    emptyDiv.index = 0;
                    map.controls[google.maps.ControlPosition.TOP_CENTER].push(emptyDiv);



                    var input = document.createElement('input');
                    input.className = "controls";
                input.type = "text";
                    input.style.marginTop="10px";
                    input.style.width = "40%";
                    input.style.zIndex = "0";
                    input.style.fontSize = "15px";
                    input.style.position = "absolute";
                    input.style.left = "113px";
                    input.placeholder = "Enter a Location";
                    var searchBox = new google.maps.places.SearchBox(input);
                    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);


                    var autocomplete = new google.maps.places.Autocomplete(input,
                        {componentRestrictions: {country: 'us'}});
                    autocomplete.bindTo('bounds', map);
                    searchBox.addListener('places_changed', function () {

                        var places = searchBox.getPlaces();
                        console.log(places);
                        if (places.length == 0) {
                            return;
                        }
                        else {
                            //$scope.selectedOrg.OrganizationAddress = places.formatted_address;
                            //$scope.selectedOrg.OrganizationAddress = "Hello";
                        }
                        // Clear out the old markers.
                        markers.forEach(function (marker) {
                            marker.setMap(null);
                        });
                        markers = [];
                        var bounds = new google.maps.LatLngBounds();
                        places.forEach(function (place) {
                            var icon = {
                                url: place.icon,
                                size: new google.maps.Size(71, 71),
                                origin: new google.maps.Point(0, 0),
                                anchor: new google.maps.Point(17, 34),
                                scaledSize: new google.maps.Size(25, 25)
                            };

                            // Create a marker for each place.
                            markers.push(new google.maps.Marker({
                                map: map,
                                icon: icon,
                                title: place.name,
                                position: place.geometry.location
                            }));

                            if (place.geometry.viewport) {
                                // Only geocodes have viewport.
                                bounds.union(place.geometry.viewport);
                            } else {
                                bounds.extend(place.geometry.location);
                            }
                        });
                        map.fitBounds(bounds);
                    });

				if(locationLatLong==null){
                    var OrgPolygonLatLong;
                    var orgId = $("#selectOrg").val();
                    if( orgId == null || orgId == ""){
                        orgId = organizationList[0].OrganizationID;
                    }
                    for (  var i=0 ; i < organizationList.length; i++)
                    {
                        if(organizationList[i].OrganizationID == parseInt(orgId)){
                            locationLatLong = organizationList[i].LatLong;
                            orgName = organizationList[i].OrganizationName;
                            break;
                        }
                    }
                    orgScoped = true;
                }
                if(locationLatLong!=null){
                    console.log(locationLatLong);
                    var coordSplitList =  locationLatLong.split(" ");
                    for(var i = 0; i < coordSplitList.length; i++)
                    {
                        if(coordSplitList[i]!=null && coordSplitList[i]!=""){
                            var coordsAr = coordSplitList[i].split(",");
                            Coords.push(new google.maps.LatLng(parseFloat(coordsAr[0]),parseFloat(coordsAr[1])));
                        }
                    }
                }

                // Construct the polygon.
                var myPolygon = new google.maps.Polygon({
                    paths: Coords,
                    strokeColor: '#FF0000',
                    strokeOpacity: 0.8,
                    strokeWeight: 3,
                    fillColor: '#FF0000',
                    fillOpacity: 0.35
                });

                if(orgScoped){
                    myPolygon.setMap(map);
                    map.fitBounds(myPolygon.getBounds());
                    //map.getBoundsZoomLevel(myPolygon.getBounds());
                    // Add a listener for the click event.
                    google.maps.event.addListener(myPolygon, 'click', showArrays);
                    //myLatlang = myPolygon.getBounds().getCenter();
                    //map.setCenter(myLatlang);
                    //map.setZoom(12);
                    //google.maps.event.trigger(map, 'resize');
                    //var marker = new google.maps.Marker({
                    //    position: myLatlang,
                    //    map: map,
                    //    title: orgName
                    //  });
                }else{
                    myPolygon.setMap(map);
                    map.fitBounds(myPolygon.getBounds());
                    //map.getBoundsZoomLevel(myPolygon.getBounds());
                    // Add a listener for the click event.
                    google.maps.event.addListener(myPolygon, 'click', showArrays);
                }
                //window.setTimeout(function(){
                //    var mapControls = $('.gmnoprint');
                //    var control = mapControls[13];
                //    console.log(control);
                //    $(control).css({"margin-top":"10px", "left":"400px"});
                //    console.debug("MapControls", mapControls);
                //},1000);

				var drawingManager = new google.maps.drawing.DrawingManager(
					{
						drawingControl: true,
						drawingControlOptions: {
							position: google.maps.ControlPosition.TOP_CENTER,
                            style: {"margin-top":"40px"},
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

                console.debug("Drawing manger", drawingManager);
				//Create a drawing manager panel that lets you select polygon, polyline, circle, rectangle or etc and then draw it.
				drawingManager.setMap(map);


				//var input = document.getElementById('address-bar');
				//map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
				//var autocomplete = new google.maps.places.Autocomplete(input);
				//autocomplete.bindTo('bounds', map);
/*				
				if (modal_mode == 'Update' && coordinates!=null) 
				{
					var Coords = [];
					var coordSplitList =  coordinates.split(" ");
					for(var i = 0 ; i < coordSplitList.length ; i++)
					{
						Coords.push(new google.maps.LatLng(parseFloat(coordSplitList[i].split(",")[0]),parseFloat(coordSplitList[i].split(",")[1])));
					}
				  //  console.log(Coords);


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
				   // map.getBoundsZoomLevel(myPolygon.getBounds());

					// Add a listener for the click event.
				//    google.maps.event.addListener(myPolygon, 'click', showArrays);

				  //  infoWindow = new google.maps.InfoWindow();
				}
*/
				//This event fires when creation of polygon is completed by user.
				google.maps.event.addDomListener(drawingManager, 'polygoncomplete', function (polygon) {
					//This line make it possible to edit polygon you have drawed.
                    if(disablePolygonComplete){
                        disablePolygonComplete = false;
                        return;
                    }
                    console.log(polygon);
					polygon.setEditable(true);
					polygon.setDraggable(true);
                    console.debug(myPolygon);

					//Call function to pass polygon as parameter to save its coordinated to an array.
					save_coordinates_to_array(polygon,myPolygon);

					//This event is inside 'polygoncomplete' and fires when you edit the polygon by moving one of its anchors.
					google.maps.event.addListener(polygon.getPath(), 'set_at', function () {
						save_coordinates_to_array(polygon);
					});

					//This event is inside 'polygoncomplete' too and fires when you edit the polygon by moving on one of its anchors.
					google.maps.event.addListener(polygon.getPath(), 'insert_at', function () {
						save_coordinates_to_array(polygon);
					});
				});
                var cancelDrawingShape = false;
                var disablePolygonComplete = false;
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
                google.maps.event.addDomListener(document,'keyup',function(e,polygon){
                    var code = (e.keyCode ? e.keyCode : e.which);

                    if (code === 46) {
                        cancelDrawingShape = true;
                        drawingManager.setDrawingMode(null);
                       // polygon.setMap(null);
                    }
                });
                google.maps.event.addListener(drawingManager, 'overlaycomplete', function (e) {
                    var lastDrawnShape = e.overlay;
                    console.log(e);
                    console.debug(lastDrawnShape);
                    if (cancelDrawingShape) {
                        cancelDrawingShape = false;
                        disablePolygonComplete = true;
                        lastDrawnShape.setMap(null); // Remove drawn but unwanted shape
                    }

                });

			}
                        
            var infoWindow;

            //google.maps.Polygon.prototype.getBounds = function() {
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
            //}
				
            obj.prototype.initProjectMap = function(selectedNode, organizationList) {
                var projectMap = this;
                console.log(projectMap.getScope());
                console.log(projectMap.getRootScope());
                console.log(selectedNode);
                console.log(organizationList);

                var myLatlang = null;//new google.maps.LatLng(40.562655, -101.707602); //default
                var zoomLevel = 4;
                var mapOptions = {
                    zoom: zoomLevel,
                    //center: new google.maps.LatLng(24.886436490787712, -70.2685546875),
                    center: myLatlang,
                    mapTypeId: null//google.maps.MapTypeId.TERRAIN
                };

                var myPolygon;
                //var map = new google.maps.Map(document.getElementById('project-map-canvas'),
                //    mapOptions);
                var map = null;
                // Define the LatLng coordinates for the polygon.
                var Coords = [];
                var coordSplitList = null;
                var orgScoped = false;
                var orgName = "";
                if(selectedNode!=null && selectedNode.LatLong!=null && selectedNode.LatLong!=""){
                    coordSplitList =  selectedNode.LatLong.split(" ");
                }else{
                    var OrgPolygonLatLong;
                    var orgId = $("#selectOrg").val();
                    if( orgId == null || orgId == ""){
                        orgId = organizationList[0].OrganizationID;
                    }
                    for (  var i=0 ; i < organizationList.length; i++)
                    {
                        if(organizationList[i].OrganizationID == parseInt(orgId)){

                            OrgPolygonLatLong = organizationList[i].LatLong;
                            orgName = organizationList[i].OrganizationName;
                            break;
                        }
                    }
                    console.log(OrgPolygonLatLong);
                    if(OrgPolygonLatLong!="" && typeof OrgPolygonLatLong != 'undefined'){
                        coordSplitList =  OrgPolygonLatLong.split(" ");
                    }
                    orgScoped = true;
                }

                if(coordSplitList!=null){
                    for(var i = 0; i < coordSplitList.length; i++)
                    {
                        if(coordSplitList[i]!=null && coordSplitList[i]!=""){
                            var coordsAr = coordSplitList[i].split(",");
                       //     Coords.push(new google.maps.LatLng(parseFloat(coordsAr[0]),parseFloat(coordsAr[1])));
                        }
                    }

                    //console.log(Coords);

                    // Construct the polygon.
                    //myPolygon = new google.maps.Polygon({
                    //    paths: Coords,
                    //    strokeColor: '#FF0000',
                    //    strokeOpacity: 0.8,
                    //    strokeWeight: 3,
                    //    fillColor: '#FF0000',
                    //    fillOpacity: 0.35
                    //});

                    if(orgScoped){
                        //myPolygon.setMap(map);
                        //map.fitBounds(myPolygon.getBounds());
                        //map.getBoundsZoomLevel(myPolygon.getBounds());
                        // Add a listener for the click event.
                       // google.maps.event.addListener(myPolygon, 'click', showArrays);
                    //Use the following for showing the center of the organization without boundaries
                        //myLatlang = myPolygon.getBounds().getCenter();
                        //map.setCenter(myLatlang);
                        //map.setZoom(10);
                        //google.maps.event.trigger(map, 'resize');
                        //var marker = new google.maps.Marker({
                        //    position: myLatlang,
                        //    map: map,
                        //    title: orgName
                        //  });
                    }else{
                        myPolygon.setMap(map);
                        //map.fitBounds(myPolygon.getBounds());
                        //map.getBoundsZoomLevel(myPolygon.getBounds());
                        // Add a listener for the click event.
                     //   google.maps.event.addListener(myPolygon, 'click', showArrays);
                    }

                }else{
                    //map.setCenter(myLatlang);
                    //map.setZoom(4);
                   // google.maps.event.trigger(map, 'resize');
                }
             //   infoWindow = new google.maps.InfoWindow();
            }
        


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
				while(0 !== temp)
				{
					var shape = shapes.pop();
					shape.setMap(null);
					temp--;
				}
			}
    }
    
    return obj;
}());