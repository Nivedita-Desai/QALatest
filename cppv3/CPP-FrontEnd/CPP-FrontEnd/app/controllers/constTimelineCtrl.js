
angular.module('cpp.controllers').
    controller('ConstTimelineCtrl', ['localStorageService','$http','$scope','$rootScope','Page','myLocalStorage',
        function (localStorageService,$http,$scope, $rootScope, Page,myLocalStorage) {
            Page.setTitle('Timeline');
            var tm;
            var currentItem, currentDs;
            var aList, cList; // list of organized items
            var con = false, initial = true;
            var authData = localStorageService.get('authorizationData');
            var orgID = myLocalStorage.get('userSelectedOrgId');

            var currentYear = new Date().getFullYear();
            var currentItem, currentDs;
            var variableYears = 5;
            var tlsize = 300;
            var tl = tlsize + "px";

            // +- variableYears
            $scope.years = [];
            for(var k = currentYear-variableYears; k < currentYear+variableYears; k++){
                $scope.years[(k-(currentYear-variableYears))] = {
                    id: k,
                    name: k
                };
            }

            // months
            $scope.months = [
                {id: "01", name: "January"},
                {id: "02", name: "February"},
                {id: "03", name: "March"},
                {id: "04", name: "April"},
                {id: "05", name: "May"},
                {id: "06", name: "June"},
                {id: "07", name: "July"},
                {id: "08", name: "August"},
                {id: "09", name: "September"},
                {id: "10", name: "October"},
                {id: "11", name: "November"},
                {id: "12", name: "December"}
            ];


            // non recursive mergesort, sorts the list chronologically
            var sort = function(a){
                var rght = 0, rend = 0, i = 0, j = 0, m = 0;
                var b = [];
                for(var k = 1; k < a.length; k *= 2){
                    for(var left = 0; left+k < a.length; left += k*2){
                        rght = left + k;
                        rend = rght + k;
                        if(rend > a.length) { rend = a.length; }
                        m = left; i = left; j = rght;
                        while(i < rght && j < rend){
                            if(a[i].start <= a[j].start){ //business logic
                                b[m] = a[i]; i++;
                            } else {
                                b[m] = a[j]; j++;
                            }
                            m++;
                        }
                        while(i < rght){
                            b[m] = a[i];
                            i++; m++;
                        }
                        while(j < rend){
                            b[m] = a[j];
                            j++; m++;
                        }
                        for(m = left; m < rend; m++){
                            a[m] = b[m];
                        }
                    }
                }
                rght = null; rend = null; i = null; j = null; m = null; b = null;
                return a;
            };

            // receives direction and dataset, returns the next TimeMapItem from ds; true=forward
            var setNext = function(direction,ds) {
                var forward = direction;
                var currTitle = currentItem.getTitle();
                var pos = 0, next;
                var list = (con)
                    ? cList
                    : aList;

                // find current position in list
                for(var i = 0; i < list.length; i++){
                    if(list[i].title == currTitle){
                        pos = i;
                    }
                }

                // increment to next position
                if(forward) {
                    pos = pos + 1;
                } else {
                    pos = pos - 1;
                }

                // make sure increment is valid
                if(pos >= list.length && forward){
                    pos = 1;
                } else if (pos <= 0 && !forward) {
                    pos = ds.items.length-1;
                }

                // get next item
                for(var j = 1; j < ds.items.length; j++){
                    if(ds.items[j].getTitle() == list[pos].title){
                        next = ds.items[j];
                    }
                }

                // return TimeMapItem
                return next;
            };

            // receives a direction and returns the next TimeMapItem; true=forward
            var getNext = function(direction){

                //get the current dataset
                currentDs = (!con)
                    ? tm.datasets.AllProjects
                    : tm.datasets.ConProjects;

                // if there is a selected item, set current to it
                if(tm.getSelected()) {
                    initial = false;
                    currentItem = tm.getSelected();
                }

                // if not initial, set temp to next, else next to first item
                if(!initial){
                    currentItem = setNext(direction,currentDs);
                } else {
                    currentItem = currentDs.items[0];
                    initial = false;
                }

                // go to next item
                tm.setSelected(currentItem);
                tm.getSelected().openInfoWindow();
                tm.scrollToDate(currentItem.getStart(),0,0);
            };

            // filter button listener
            $(document).ready(function(){
//<<<<<<< HEAD
//                // filter between all or construction
//                $('#filt-btn').click(function () {
//                    con = !con;
//                    $(this).toggleClass("down");
//                    if(con){
//=======
                // toggle between all and construction data sets
                $('input:radio').on('click',function(e){
                    if(e.currentTarget.value == "con"){

                        tm.hideDataset("AllProjects");
                        tm.showDataset("ConProjects");
                        tm.refreshTimeline();
                    } else {
                        tm.hideDataset("ConProjects");
                        tm.showDataset("AllProjects");
                        tm.refreshTimeline();
                    }
                });

            });

            // go to a selected date - default will go to current date
            $("#select_date").on('click',function(){
                var year = $('#year'),
                    yd = year.val(),
                    y = ( yd ? yd : new Date().getFullYear() );
                var month = $('#month'),
                    md = month.val(),
                    m = ( md ? md : "01" );
                var d = y + "-" + m + "-" + "01";
                (!yd && !md)
                    ? tm.scrollToDate(new Date(),1,1) // scroll to current date
                    : tm.scrollToDate(d,1,1); // scroll to selected date
                //return drop down menus to default
                month.val(''); year.val('');

                year = null; yd = null; y = null;
                month = null; md = null; m = null;
                d = null;
            });

            // navigation buttons
            $("#p-item").on('click',function(){ getNext(0); });
            $("#n-item").on('click',function(){ getNext(1); });

            // test buttons
            $("#test1").on('click',function() {

                $scope.tl = {
                    MaxDate : tm.timeline._bands[0].getMaxVisibleDate(),
                    MinDate : tm.timeline._bands[0].getMinVisibleDate()
                };

                console.log("FROM: \t" +    tm.timeline._bands[0].getMinVisibleDate());
                console.log("NOW: \t" +     tm.timeline._bands[0].getCenterVisibleDate());
                console.log("TO: \t" +      tm.timeline._bands[0].getMaxVisibleDate());


            });
            $("#test2").on('click',function(){ });
            $("#test3").on('click',function(){ });

            // initialize timeline
            $(function() {
                // custom loader to hold start and end dates of full project
                TimeMap.loaders.AllLoader = function(options) {
                    var loader = new TimeMap.loaders.remote(options);
                    loader.parse = JSON.parse;
                    loader.preload = function(data) {
                        data.result[0].options.AStartDate = data.result[0].options.Note;
                        data.result[0].options.AEndDate = data.result[0].options.OrganizationAddress;
                        aList = sort(data.result);
                        return data;
                    };
                    return loader;
                };

                // custom loader to create Construction data set
                TimeMap.loaders.ConLoader = function(options){
                    var loader2 = new TimeMap.loaders.remote(options);
                    loader2.parse = JSON.parse;
                    loader2.preload = function(data){
                        data.result[0].options.AStartDate = data.result[0].options.Note;
                        data.result[0].options.AEndDate = data.result[0].options.OrganizationAddress;
                        //set construction times and remove those w/o construction
                        for(var i = 1; i < data.result.length; i++){
                            // if there is a construction phase
                            if(data.result[i].options.Construction){
                                // switch the start and end dates with the construction dates
                                data.result[i].start = data.result[i].options.CStartDate;
                                data.result[i].end = data.result[i].options.CEndDate;
                            } else {
                                // if not, erase them from the timeline
                                // move all items up
                                for(var j = i; j < data.result.length-1; j++){
                                    data.result[j] = data.result[j+1];
                                }
                                // pop last item
                                data.result.pop();
                            }
                        }
                        cList = sort(data.result);
                        return data;
                    };
                    return loader2;
                };
                console.log(orgID);
                // initialize TimeMap
                tm = TimeMap.init({
                    mapId: "map",               // Id of map div element (required)
                    timelineId: "timeline",     // Id of timeline div element (required)a
                    options: {
                        syncBands: true,
                        mapZoom: 0,
                        mapType: "satellite",
                        showMapTypeCtrl: true,
                        showMapCtrl: true,
                        eventIconPath: "../images/",
                        infoTemplate:
                            '<div>' +
                            '<strong>{{title}}</strong><br>' +
                            '<p style="color:#A9A9A9">' +
                            '<i>{{AStartDate}}</i><br>' +
                            '<i>{{AEndDate}}</i>' +
                            '</p>' +
                            '</div>'
                    },
                    datasets: [
                        {
                            id: "AllProjects",
                            type: "AllLoader",
                            options: {
                                url: serviceBasePath + "Request/ProjectLocation/" + orgID,
                                headers: { 'Authorization': 'Bearer '+authData.token }
                            }
                        },
                        {
                            id: "ConProjects",
                            type: "ConLoader",
                            options: {
                                url: serviceBasePath + "Request/ProjectLocation/" + orgID,
                                headers: { 'Authorization': 'Bearer '+authData.token }
                            }
                        }
                    ],
                    bandInfo: [
                        {
                            width:          "70%",
                            trackGap:       0.1,
                            intervalPixels: 100,
                            intervalUnit:   Timeline.DateTime.MONTH
                        },
                        {
                            width:          "30%",
                            trackHeight:    0,
                            intervalPixels: 200,
                            showEventText:  false,
                            intervalUnit:   Timeline.DateTime.YEAR
                        }
                    ],
                    dataLoadedFunction: function(){
                        tm.hideDataset("ConProjects");
                        tm.refreshTimeline();
                    }
                });
            });

        }]);
