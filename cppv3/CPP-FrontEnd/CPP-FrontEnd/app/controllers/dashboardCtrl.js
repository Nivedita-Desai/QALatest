/**
 * Created by ikhong on 6/1/2016.
 */
angular.module('cpp.controllers').
    controller('DashboardCtrl',['User','$http','Organization','UserName','authService','$scope','$rootScope','$location','myLocalStorage','localStorageService','$menuItems','$state','$window','TrendStatus','ProjectTitle','Page',
        function (User,$http,Organization,UserName,authService,$scope, $rootScope,$location,myLocalStorage,localStorageService,$menuItems,$state,$window,TrendStatus,ProjectTitle,Page) {
            ProjectTitle.setTitle('');
            TrendStatus.setStatus('');
            // display greeting with user's name
            User.get({}, function (Users) {
                var result = Users.result;
                angular.forEach(result,function(user){
                    if(user.UserID == localStorageService.get('authorizationData').userName){
                        Page.setTitle("Hello, " + user.FirstName + " " + user.LastName + "!");
                    }
                })
            });

            // selectors
            $scope.tccmain = "Org";
            $scope.tccsub = "Prg";
            $scope.selectedOrganization = "";
            $scope.selectedProject = "";
            $scope.filterAlertsText = "";

                // lists
            $scope.organizationList = [];
            $scope.programList = [];
            $scope.projectList = [];
            $scope.trendList = [];
            $scope.alertList = [
                // include the related individuals and the related org, program, element, and project
                // show this info in a modal

            ]; // to be used later -- test data

            // organization list -- get info+relationships from backend
            Organization.lookup().get({},function(organizationData){
                $scope.organizationList = organizationData.result;
                $scope.selectOrganization($scope.organizationList[0].OrganizationID);
            });

            // selects the organization info into view
            $scope.selectOrganization = function(OrganizationID){

                //select the organization
                $scope.filterOrg = OrganizationID;
                $scope.totalCostChart = 1;

                //get the organization tree info + set the project and program list
                $http.get(serviceBasePath+"/Request/WBS/"+OrganizationID)
                    .then(function (data) {
                    	console.log(data);
                        $scope.selectedOrganization = data.data;
                        $scope.projectList = [];
                        for(var i = 0; i < $scope.selectedOrganization.children.length; i++){
                            var program = $scope.selectedOrganization.children[i];
                            for(var j = 0; j < program.children.length; j++){
                                var programelement = program.children[j];
                                for(var k = 0; k < programelement.children.length; k++){
                                    var project = programelement.children[k];
                                    project.Program = program;
                                    project.ProgramElement = programelement;
                                    $scope.projectList.push(project);
                                }
                            }
                        }

                        $scope.programList = $scope.selectedOrganization.children;
                        angular.forEach($scope.programList,function(prg){
                            console.log(prg);
                            //prg.children = null;
                            prg.programCategories = null;
                            prg.programFunds = null;
                        });
                        $scope.drawPieChart($scope.selectedOrganization);
                        $scope.updateSubOptions();


                        // get alerts from back end and attach them to project and user
                        $scope.alertList = [
                            //{
                            //    ID: '4',
                            //    AlertTitle: 'Project Name, Trend #',
                            //    AlertDescription: 'Project X, Trend Y needs review',
                            //    AlertType: 'Review',
                            //    AlertDateTime: '06/09/2016',
                            //    AlertOrganization:       $scope.selectedOrganization,
                            //    AlertProgram:            $scope.selectedOrganization.children[0],
                            //    AlertProgramElement:     $scope.selectedOrganization.children[0].children[1],
                            //    AlertProject:            $scope.selectedOrganization.children[0].children[1].children[0],
                            //    AlertTrend:              {}
                            //},
                            //{
                            //    ID: '3',
                            //    AlertTitle: 'Project Name, Trend #',
                            //    AlertDescription: 'Project X, Trend Y has been accepted',
                            //    AlertType: 'Acceptance',
                            //    AlertDateTime: '06/09/2016',
                            //    AlertOrganization:       $scope.selectedOrganization,
                            //    AlertProgram:            $scope.selectedOrganization.children[1],
                            //    AlertProgramElement:     $scope.selectedOrganization.children[1].children[1],
                            //    AlertProject:            $scope.selectedOrganization.children[1].children[1].children[1],
                            //    AlertTrend:              {}
                            //},
                            //{
                            //    ID: '2',
                            //    AlertTitle: 'Project Name, Trend #',
                            //    AlertDescription: 'Project X, Trend Y has been denied',
                            //    AlertType: 'Denial',
                            //    AlertDateTime: '05/22/2016',
                            //    AlertOrganization:       $scope.selectedOrganization,
                            //    AlertProgram:            $scope.selectedOrganization.children[0],
                            //    AlertProgramElement:     $scope.selectedOrganization.children[0].children[0],
                            //    AlertProject:            $scope.selectedOrganization.children[0].children[0].children[1],
                            //    AlertTrend:              {}
                            //},
                            //{
                            //    ID: '1',
                            //    AlertTitle: 'Project Name, Trend #',
                            //    AlertDescription: 'Project X, Trend Y needs review',
                            //    AlertType: 'Review',
                            //    AlertDateTime: '01/29/2016',
                            //    AlertOrganization:       $scope.selectedOrganization,
                            //    AlertProgram:            $scope.selectedOrganization.children[1],
                            //    AlertProgramElement:     $scope.selectedOrganization.children[1].children[1],
                            //    AlertProject:            $scope.selectedOrganization.children[1].children[1].children[1],
                            //    AlertTrend:              {}
                            //}
                        ];
                        //angular.forEach($scope.alertList,function(object){
                        //    var tList = [];
                        //    // get trend info -- back end calculations might delay this a bit
                        //    $http.get(serviceBasePath+"Request/Dashboard/GetTrends/"+object.AlertProject.ProjectID)
                        //        .then(function(result){
                        //            tList = result.data.result[result.data.result.length-1];
                        //            if(result.data.result.length==1) {
                        //                tList.name = "Baseline";
                        //            }
                        //            else {
                        //                tList.name = "Trend " + result.data.result.length-1;
                        //            }
                        //                tList.CurrentCost
                        //                    = tList.TrendStatusID == 1
                        //                    ? tList.PostTrendCost
                        //                    : "";
                        //
                        //            object.AlertTrend = tList;
                        //        });
                        //});


                        // get alerts from back end and attach them to project
                        //$scope.alertList = [
                        //    {
                        //        ID: '4',
                        //        AlertTitle: 'Project Name, Trend #',
                        //        AlertDescription: 'Project X, Trend Y needs review',
                        //        AlertType: 'Review',
                        //        AlertDateTime: '06/09/2016',
                        //        AlertOrganization:       $scope.selectedOrganization,
                        //        AlertProgram:            $scope.selectedOrganization.children[0],
                        //        AlertProgramElement:     $scope.selectedOrganization.children[0].children[1],
                        //        AlertProject:            $scope.selectedOrganization.children[0].children[1].children[0],
                        //        AlertTrend:              {}
                        //    },
                        //    {
                        //        ID: '3',
                        //        AlertTitle: 'Project Name, Trend #',
                        //        AlertDescription: 'Project X, Trend Y has been accepted',
                        //        AlertType: 'Acceptance',
                        //        AlertDateTime: '06/09/2016',
                        //        AlertOrganization:       $scope.selectedOrganization,
                        //        AlertProgram:            $scope.selectedOrganization.children[1],
                        //        AlertProgramElement:     $scope.selectedOrganization.children[1].children[1],
                        //        AlertProject:            $scope.selectedOrganization.children[1].children[1].children[1],
                        //        AlertTrend:              {}
                        //    },
                        //    {
                        //        ID: '2',
                        //        AlertTitle: 'Project Name, Trend #',
                        //        AlertDescription: 'Project X, Trend Y has been denied',
                        //        AlertType: 'Denial',
                        //        AlertDateTime: '05/22/2016',
                        //        AlertOrganization:       $scope.selectedOrganization,
                        //        AlertProgram:            $scope.selectedOrganization.children[0],
                        //        AlertProgramElement:     $scope.selectedOrganization.children[0].children[0],
                        //        AlertProject:            $scope.selectedOrganization.children[0].children[0].children[1],
                        //        AlertTrend:              {}
                        //    },
                        //    {
                        //        ID: '1',
                        //        AlertTitle: 'Project Name, Trend #',
                        //        AlertDescription: 'Project X, Trend Y needs review',
                        //        AlertType: 'Review',
                        //        AlertDateTime: '01/29/2016',
                        //        AlertOrganization:       $scope.selectedOrganization,
                        //        AlertProgram:            $scope.selectedOrganization.children[1],
                        //        AlertProgramElement:     $scope.selectedOrganization.children[1].children[1],
                        //        AlertProject:            $scope.selectedOrganization.children[1].children[1].children[1],
                        //        AlertTrend:              {}
                        //    }
                        //];
                        angular.forEach($scope.alertList,function(object){
                            var tList = [];
                            // get trend info -- back end calculations might delay this a bit
                            $http.get(serviceBasePath+"Request/Dashboard/GetTrends/"+object.AlertProject.ProjectID)
                                .then(function(result){
                                    tList = result.data.result[result.data.result.length-1];
                                    if(result.data.result.length==1) {
                                        tList.name = "Baseline";
                                    }
                                    else {
                                        tList.name = "Trend " + result.data.result.length-1;
                                    }
                                        tList.CurrentCost
                                            = tList.TrendStatusID == 1
                                            ? tList.PostTrendCost
                                            : "";
//
                                    object.AlertTrend = tList;
                                });
                        });

                        angular.forEach($scope.projectList,function(prj){
                            prj.Alerts = [];
                            angular.forEach($scope.alertList,function(alt){
                                if(prj.ProjectID == alt.AlertProject.ProjectID){
                                    prj.Alerts.push(alt);
                                    //
                                }
                            });
                        });
                        $scope.setProjectDetails($scope.projectList[0]);
                        $scope.plist = angular.copy($scope.projectList);
                        console.log($scope.projectList);
                        console.log($scope.programList);
                    });
            };

            // double click on a project in project list
            $scope.getSelectedProject = function(project){
                window.location.href="#/app/current-project/"+project.ProjectID+"/"+ project.OrganizationID + "/" + "week" ;
            };

            // single click on a project -- update pie graph / alert info / line graph
            $scope.setProjectDetails = function(project){
                $scope.trendList = [];
                $scope.selectedProject = project;
                // get trend info -- back end calculations might delay this a bit
                $http.get(serviceBasePath+"Request/Dashboard/GetTrends/"+project.ProjectID)
                    .then(function(result){
                        $scope.trendList = result.data.result;
                        for(var c = 0; c < $scope.trendList.length; c++){
                            if(c == 0){
                                $scope.trendList[c].name = "Baseline";
                            }
                            else {
                                var num = (c < 10) ? "0"+c : c;
                                $scope.trendList[c].name = "Trend " + num;
                            }
                            $scope.trendList[c].CurrentCost
                                = $scope.trendList[c].TrendStatusID == 1
                                ? $scope.trendList[c].PostTrendCost
                                : "";
                        }
                        $scope.selectedProject.children = $scope.trendList;
                        $scope.drawPieChart($scope.selectedProject);
                        $scope.setLineChartData($scope.selectedProject);
                    });

                $scope.filterAlertsText = $scope.selectedProject.name;
            };

//------------------------------------ ALERTS ------------------------------------//

            // double click an alert
            $scope.getAlertInfo = function(item){
                alert(item.AlertDescription);
            };

            $scope.filterAlerts = function(){
                $scope.filtAlerts = '';
            };

//------------------------------------ PIE CHART ------------------------------------//

            // updates the pie graph second selector
            $scope.updateSubOptions = function(){
                if($scope.tccmain == "Org"){
                    $scope.tccsub = [
                        { ID: "Prg", name: "Program" },
                        { ID: "Prj", name: "Project" }
                    ];
                }
                else if ($scope.tccmain == "Prg"){
                    $scope.tccsub = angular.copy($scope.programList);
                    angular.forEach($scope.tccsub,function(object){
                        object.ID = object.ProgramID;
                    });
                }
                else if ($scope.tccmain == "Prj"){
                    $scope.tccsub=[];
                    $scope.tccsub = angular.copy($scope.projectList);
                    angular.forEach($scope.tccsub,function(object){
                        object.ID = object.ProjectID;
                    });
                }
                //sets pie graph to the initial item in the new list
                $scope.tccsubselect = $scope.tccsub[0].ID;
                $scope.selectPieGraphInfo($scope.tccmain,$scope.tccsubselect);
            };

            // selects the info to set into the pie graph using the two selectors then draws the pie graph
            $scope.selectPieGraphInfo = function(p1,p2){
                if(p1=="Org"){
                    if(p2=="Prj"){
                        var org = angular.copy($scope.selectedOrganization);
                        org.children = $scope.projectList;
                        $scope.drawPieChart(org);
                    }
                    else {
                        $scope.tccsubselect = "Prg";
                        $scope.drawPieChart($scope.selectedOrganization);
                    }
                }
                else if(p1=="Prg"){
                    if(p2){
                        angular.forEach($scope.programList,function(program){
                            if(program.ProgramID == p2){
                                $scope.drawPieChart(program);
                            }
                        });
                    }
                    else {
                        $scope.tccsubselect = $scope.programList[0].ProgramID;
                        $scope.drawPieChart($scope.programList[0]);
                    }
                }
                else if(p1=="Prj"){
                    if(p2){
                        angular.forEach($scope.projectList,function(project){
                            if(project.ProjectID == p2){
                                $scope.setProjectDetails(project);
                            }
                        });
                    }
                    else{
                        $scope.tccsubselect = $scope.projectList[0].ProjectID;
                        $scope.setProjectDetails($scope.projectList[0]);
                    }
                }
            };

            // draw the pie chart
            $scope.drawPieChart = function(object){
                //add the pie chart data here
                var piechart_data = new google.visualization.DataTable();
                piechart_data.addColumn('string','name');
                piechart_data.addColumn('number','cost');
                angular.forEach(object.children,function(object){
                    piechart_data.addRow(
                        [object.name,Number(object.CurrentCost)]
                    )
                });

                // piechart options
                var piechart_options = {
                    'legend':'left',
                    'title':object.name,
                    'is3D':true
                };

                // instantiate chart
                var piechart = new google.visualization.PieChart(document.getElementById('piechart'));

                // define event handlers
                function selectHandler(){
                    var item = piechart.getSelection()[0];
                    var value = (piechart_data.getValue(item.row,1));
                    console.log(value);
                }

                // add any event handlers
                google.visualization.events.addListener(piechart,'select', selectHandler);

                //draw the chart
                piechart.draw(piechart_data, piechart_options);
            };

//------------------------------------ LINE CHART ------------------------------------//

            // get info for the line chart
            $scope.setLineChartData = function(object){
                var data = [];
                var budgetData = [];
                var editedData = [];
                data.push(['Month','Budget']);

                //get the budget throughout the projects span
                $http.get(serviceBasePath+'Request/DashboardReport/BVR/'+object.ProjectID)
                    .then(function(result){
                        budgetData = JSON.parse(result.data);

                        var monthdiff = 0;
                        var year1 = 0;
                        var year2 = 0;
                        var month1 = 0;
                        var month2 = 0;
                        var totalCost = 0;

                        //push starting date
                        var sm = parseInt(budgetData[0].month);
                        var sy = parseInt(budgetData[0].year);
                        if(sm==1){ sm = 12; sy--; }
                        else{ sm--; }
                        editedData.push({cost:totalCost,month:sm,year:sy});

                        totalCost = parseInt(budgetData[0].cost);

                        // time for Ivan's super cool algorithm!
                        for(var i = 1; i < budgetData.length; i++){
                            // calculate month difference
                            year1       = parseInt(budgetData[i-1].year);
                            month1      = parseInt(budgetData[i-1].month);
                            year2       = parseInt(budgetData[i].year);
                            month2      = parseInt(budgetData[i].month);
                            monthdiff   = Math.abs(((year2-year1)*12) - Math.abs(month2-month1)) - 1;

                            //push first item
                            editedData.push({cost:totalCost,month:month1,year:year1});

                            // calculate total cost
                            totalCost += parseInt(budgetData[i].cost);

                            //if difference, add all missing items
                            if(monthdiff > 0){

                                // get current month and year
                                var currMonth = month1;
                                var currYear = year1;

                                //add the missing months
                                for(var k = 0; k < monthdiff; k++){
                                    //get next month
                                    if(currMonth==12){
                                        currMonth = 1;
                                        currYear++;
                                    }
                                    else{
                                        currMonth++;
                                    }
                                    // add month
                                    editedData.push({cost:totalCost,month:currMonth,year:currYear})
                                }
                            }
                        }

                        //get the last month
                        var lMonth = parseInt(budgetData[budgetData.length-1].month);
                        var lYear = parseInt(budgetData[budgetData.length-1].year);

                        //push last month
                        editedData.push({cost:totalCost,month:lMonth,year:lYear});

                        var found = false;
                        $scope.budgetToDate = 0;
                        angular.forEach(editedData,function(editedDataItem){
                            if(!found && editedDataItem.month == new Date().getMonth()+1 && editedDataItem.year == new Date().getFullYear()){
                                $scope.budgetToDate = editedDataItem.cost;
                                found = true;
                            }
                            switch(editedDataItem.month){
                                case 1:  data.push(["Jan " + editedDataItem.year, editedDataItem.cost]); break;
                                case 2:  data.push(["Feb " + editedDataItem.year, editedDataItem.cost]); break;
                                case 3:  data.push(["Mar " + editedDataItem.year, editedDataItem.cost]); break;
                                case 4:  data.push(["Apr " + editedDataItem.year, editedDataItem.cost]); break;
                                case 5:  data.push(["May " + editedDataItem.year, editedDataItem.cost]); break;
                                case 6:  data.push(["Jun " + editedDataItem.year, editedDataItem.cost]); break;
                                case 7:  data.push(["Jul " + editedDataItem.year, editedDataItem.cost]); break;
                                case 8:  data.push(["Aug " + editedDataItem.year, editedDataItem.cost]); break;
                                case 9:  data.push(["Sep " + editedDataItem.year, editedDataItem.cost]); break;
                                case 10: data.push(["Oct " + editedDataItem.year, editedDataItem.cost]); break;
                                case 11: data.push(["Nov " + editedDataItem.year, editedDataItem.cost]); break;
                                case 12: data.push(["Dec " + editedDataItem.year, editedDataItem.cost]); break;
                            }
                        });


                        //draw the Line Chart
                        var linechart_data = google.visualization.arrayToDataTable(data);
                        var linechart_options = {
                            title: object.name,
                            //curveType: 'function',
                            legend: { position: 'bottom' }
                        };
                        var linechart = new google.visualization.LineChart(document.getElementById('linechart'));
                        linechart.draw(linechart_data, linechart_options);


                    });



            };

            $scope.drawLineChart = function(data){

            }

        }]);
