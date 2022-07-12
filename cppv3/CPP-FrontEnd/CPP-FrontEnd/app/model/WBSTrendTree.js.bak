WBSTrendTree = (function ($) {
    var obj = function WBSTrendTree() {
        if (!this instanceof WBSTrendTree) {
            return new WBSTrendTree();
        }

        var TRENDSTATUSCODELIST = [];
        var isFieldValueChanged = false; // Jignesh-31-03-2021
        _angularscope = null;
        _baseline = null;
        _wbsTree = null;
        _trendNumber = "";
        _httpProvider = null;
        _selectedTreeNode = null;
        _Trend = null;
        _ApprovalThresholdInfo = null;
        _RequestApproval = null;
        _projectName = null;
        _trendStatusCode = null;
        _ProjectWhiteListService = null;
        var modal, modal_mode = "Update", metadata;
        //Cost overhead type arrays
        var costOverheadTypes = [{
            ID: 1,
            CostOverHeadType: "Billable Rate"
        }, {
            ID: 2,
            CostOverHeadType: "Raw Rate with Multiplier"
        }];
        obj.prototype.setScope = function (scope) {
            if (scope) {
                _angularscope = scope;
            }
        }
        obj.prototype.getScope = function () {
            return _angularscope;
        }
        obj.prototype.setWBSTree = function (wbsTree) {
            if (wbsTree) {
                _wbsTree = wbsTree;
            }
        }
        obj.prototype.getWBSTree = function () {
            return _wbsTree;
        }
        obj.prototype.setHttpProvider = function ($http) {
            if ($http) {
                _httpProvider = $http;
            }
        }
        obj.prototype.setApprovalThresholdInfo = function (threshold) {
            if (threshold) {
                _ApprovalThresholdInfo = threshold;
            }
        }
        obj.prototype.setRequestApproval = function (request) {
            if (request) {
                _RequestApproval = request;
            }
        }
        obj.prototype.getApprovalThresholdInfo = function () {
            return _ApprovalThresholdInfo;
        }
        obj.prototype.setSelectedTreeNode = function (selectedTreeNode) {
            if (selectedTreeNode) {
                _selectedTreeNode = selectedTreeNode;
            }
        }
        obj.prototype.getSelectedTreeNode = function () {
            return _selectedTreeNode;
        }
        obj.prototype.setTrend = function (Trend) {
            if (Trend) {
                _Trend = Trend;
            }
        }
        obj.prototype.setTrendNumber = function (TrendNumber) {
            if (TrendNumber) {
                _trendNumber = TrendNumber;
            }
        }
        obj.prototype.getTrendNumber = function () {
            return _trendNumber;
        }
        obj.prototype.setTrendStatusCode = function (trendStatusCode) {
            if (trendStatusCode) {
                _trendStatusCode = trendStatusCode;
            }
        }
        obj.prototype.getTrendStatusCode = function () {
            return _trendStatusCode;
        }
        obj.prototype.trendGraph = function (allowedVisibility) {
            var selectedProjectID = _wbsTree.getSelectedProjectID();
            this.renderTrendGraph(selectedProjectID, allowedVisibility);
            this.bindEventsForTrendNodes();
        };

        obj.prototype.trendGraphNoProject = function () {
            this.renderTrendGraph(null);
        };
        obj.prototype.getProjectName = function () {
            return _projectName;
        }
        obj.prototype.setProjectName = function (newProjectName) {
            _projectName = newProjectName;
        }
        obj.prototype.getProjectWhiteListService = function () {
            return _ProjectWhiteListService;
        }
        obj.prototype.setProjectWhiteListService = function (ProjectWhiteListService) {
            _ProjectWhiteListService = ProjectWhiteListService;
        }
        obj.prototype.renderTrendGraph = function (selProjId, allowedVisibility) {
            var wbsTrenTree = this;

            _httpProvider.get(serviceBasePath + "Request/TrendGraph/" + selProjId)
                .then(function (response) {
                    console.log(response);
                    // $http.get("http://localhost:29986/api/Request/TrendGraph/" + selProjId).then(function(response){
                    var trendgraphData = response.data;
                    if (response.data.result.PastTrendList.length != 0) {
                        _baseline = response.data.result.PastTrendList[0];
                    } else {
                        _baseline = response.data.result.FutureTrendList[0];
                    }
                    if (response.data.result.FutureTrendList.length != 0) {
                        var futureTrendList = response.data.result.FutureTrendList;
                        var pastTrendList = response.data.result.PastTrendList;
                        var futureTrendNumberArray = [];
                        var pastTrendNumberArray = [];
                        var maxFutureTrendNumber = 0;
                        var maxPastTrendNumber = 0;
                        angular.forEach(futureTrendList, function (trend, index) {
                            if (trend.TrendNumber) {
                                futureTrendNumberArray.push(trend.TrendNumber);
                            }
                        });
                        angular.forEach(pastTrendList, function (trend, index) {
                            if (trend.TrendNumber) {
                                pastTrendNumberArray.push(trend.TrendNumber);
                            }
                        });
                        if (futureTrendNumberArray.length !== 0) {
                            maxFutureTrendNumber = Math.max.apply(Math, futureTrendNumberArray);
                        }
                        if (pastTrendNumberArray.length !== 0) {
                            maxPastTrendNumber = Math.max.apply(Math, pastTrendNumberArray);
                        }
                        _trendNumber = Math.max.apply(Math, [maxFutureTrendNumber, maxPastTrendNumber]);
                        _trendNumber = (parseInt(_trendNumber) + 1).toString();
                    } else {
                        var pastTrendListObject = response.data.result.PastTrendList;
                        var pastTrendArray = [];
                        angular.forEach(pastTrendListObject, function (item) {
                            if (item.TrendNumber) {
                                pastTrendArray.push(item.TrendNumber);
                            }
                        });
                        var max = Math.max.apply(Math, pastTrendArray);
                        _trendNumber = max + 1;
                        wbsTrenTree.setTrendNumber(max + 1);
                    }
                    wbsTrenTree.renderTrendGraphView(trendgraphData, selProjId, allowedVisibility);
                });
        }
        obj.prototype.renderTrendGraphView = function (trendgraphData, selProjId, allowedVisibility) {
            var projProjectManagerID = localStorage.getItem('selectProjectProjectManagerIDDash');
            var projDirectorID = localStorage.getItem('selectProjectDirectorIDDash');
            var projSchedulerID = localStorage.getItem('selectProjectSchedulerIDDash');
            var projVicePresidentID = localStorage.getItem('selectProjectVicePresidentIDDash');
            var projFinancialAnalystID = localStorage.getItem('selectProjectFinancialAnalystIDDash');
            var projtCapitalProjectAssistantID = localStorage.getItem('selectProjectCapitalProjectAssistantIDDash');

            if (!allowedVisibility) {
                return;
            }

            //STEPHEN - FORGOTTEN CODE SECURITY
            //var noAccessMsg = '';
            //switch (wbsTree.getLocalStorage().role) {
            //	case 'Admin':
            //	case 'Accounting':
            //		break;
            //	case 'Director':
            //		if (wbsTree.getLocalStorage().employeeID != projDirectorID) {
            //			noAccessMsg = 'You are not assigned ' + wbsTree.getLocalStorage().role + ' to this Project. (gv)';
            //		}
            //		break;
            //	case 'Project Manager':
            //		if (wbsTree.getLocalStorage().employeeID != projProjectManagerID) {
            //			noAccessMsg = 'You are not assigned ' + wbsTree.getLocalStorage().role + ' to this Project. (gv)';
            //		}
            //		break;
            //	case 'Financial Analyst':
            //		if (wbsTree.getLocalStorage().employeeID != projFinancialAnalystID) {
            //			noAccessMsg = 'You are not assigned ' + wbsTree.getLocalStorage().role + ' to this Project. (gv)';
            //		}
            //		break;
            //	case 'Vice President':
            //		if (wbsTree.getLocalStorage().employeeID != projVicePresidentID) {
            //			noAccessMsg = 'You are not assigned ' + wbsTree.getLocalStorage().role + ' to this Project. (gv)';
            //		}
            //		break;
            //	case 'Scheduler':
            //		if (wbsTree.getLocalStorage().employeeID != projSchedulerID) {
            //			noAccessMsg = 'You are not assigned ' + wbsTree.getLocalStorage().role + ' to this Project. (gv)';
            //		}
            //		break;
            //	case 'Capital Project Assistant':
            //		if (wbsTree.getLocalStorage().employeeID != projtCapitalProjectAssistantID) {
            //			noAccessMsg = 'You are not assigned ' + wbsTree.getLocalStorage().role + ' to this Project. (gv)';
            //		}
            //		break;
            //	default:
            //		noAccessMsg = 'You are not assign to this Project. (gv)';
            //}
            //if (noAccessMsg != '') {
            //	//dhtmlx.alert('Role:' + wbsTree.getLocalStorage().role + ' EmployeeID:' + wbsTree.getLocalStorage().EmployeeID);
            //	//dhtmlx.alert(noAccessMsg);
            //	//var svg2 = d3.select("#trend-tree-container > svg > g");
            //	//svg2.selectAll("*").remove();
            //	//wbsTreeNode.trendGraphNoProject();
            //	//return;
            //}
            ////dhtmlx.alert('Role:' + wbsTree.getLocalStorage().role + ' EmployeeID:' + wbsTree.getLocalStorage().employeeID);
            ////App Security ViewProject : 4
            //if (wbsTree.getLocalStorage().acl[4] == 0) {
            //	//Skip generate trend
            //	//dhtmlx.alert("Sorry, you cannot access this.");
            //	//var svg2 = d3.select("#trend-tree-container > svg > g");
            //	//svg2.selectAll("*").remove();
            //	//return;
            //}

            var g = new dagreD3.graphlib.Graph().setGraph({
                nodesep: 30,
                ranksep: 70,
                edgesep: 30,
                rankdir: 'LR'
            });
            var isProject = false;
            console.log(selProjId);
            if (selProjId === 'null') {
                //alert("FAFAFAFAFAFAF");
                console.log(selProjId);
                var ww = g.setNode("", {
                    metadata: null,
                    label: "No Project Selected",
                    class: "CurrentProject01",
                    shape: "iconCurrentText",
                    labelStyle: "font-size:24px;font-weight:normal;"
                });

            } else {
                var states = trendgraphData;
                wbsTrendTree.setProjectName(states.result.CurrentProjectList.ProjectName);    //Swapnil
                console.log(states);
                states.result.BaselineProjectList = JSON.parse(JSON.stringify(states.result.CurrentProjectList));
                states.result.BaselineProjectList.luan = 'luankhong';
                states.result.BaselineProjectList.level = "BaselineProject";
                states.result.BaselineProjectList.name = "BaselineProject";


                var ww = g.setNode(states.result.BaselineProjectList.level, {
                    metadata: states.result.BaselineProjectList,
                    label: "Baseline",
                    class: "CurrentProject",
                    shape: "iconCurrent",
                    labelStyle: "font-size:14px;font-weight:bold;"
                });

                //---------------------------------------- Swapnil 30/11/2020 ---------------------------------------------
                //Nivedita22-12-2021
                var xx = g.setNode(states.result.CurrentProjectList.level, {
                    metadata: states.result.CurrentProjectList,
                    label: "Current",
                    class: "CurrentProject",
                    shape: "iconCurrent",
                    labelStyle: "font-size:14px;font-weight:bold;"
                });

                //var xx = g.setNode(states.result.CurrentProjectList.level, {
                //    metadata: states.result.CurrentProjectList,
                //    label: "Budget vs Actual",
                //    class: "CurrentProject",
                //    shape: "iconCurrent",
                //    labelStyle: "font-size:14px;font-weight:bold;"
                //});
                //----------------------------------------------------------------------------------------------------

                var yy = g.setNode(states.result.ForecastProjectList.level, {
                    metadata: states.result.ForecastProjectList,
                    label: "Forecast",
                    class: "CurrentProject",
                    shape: "iconCurrent",
                    labelStyle: "font-size:14px;font-weight:bold;"
                });



                var pastTrendCount = 0;
                states.result.PastTrendList.forEach(function (state) {
                    if (state.TrendNumber == "0") {
                        return;
                    }
                    pastTrendCount++;
                    g.setEdge(states.result.BaselineProjectList.level, state.name, {
                        labelStyle: "font-size:13px;font-weight:normal;",
                        lineInterpolate: 'basis',
                        arrrowhead: 'vee'
                    });
                    if (state.TrendStatus == "Approved") {
                        g.setNode(state.name, {
                            metadata: state,
                            shape: "iconA",
                            label: state.name,
                            labelStyle: "font-size:14px;font-weight:normal;"
                        });
                    } else if (state.TrendStatus == "Rejected") {
                        g.setNode(state.name, {
                            metadata: state,
                            shape: "iconR",
                            label: state.name,
                            labelStyle: "font-size:14px;font-weight:normal;"
                        });
                    }
                    g.setEdge(state.name, states.result.CurrentProjectList.level, {
                        labelStyle: "font-size:13px;font-weight:normal;",
                        lineInterpolate: 'basis',
                        arrrowhead: 'vee'
                    });
                });
                if (pastTrendCount == 0) {
                    g.setEdge(states.result.BaselineProjectList.level, states.result.CurrentProjectList.level, {
                        labelStyle: "font-size:13px;font-weight:normal;",
                        lineInterpolate: 'basis',
                        class: "dimEdgePath",
                        arrrowhead: 'vee'
                    });
                }
                var futureTrendCount = 0;
                states.result.FutureTrendList.forEach(function (state) {
                    if (state.TrendNumber == "0") {
                        return;
                    }
                    futureTrendCount++;
                    g.setEdge(states.result.CurrentProjectList.level, state.name, {
                        labelStyle: "font-size:13px;font-weight:normal;",
                        lineInterpolate: 'basis',
                        arrrowhead: 'vee'
                    });
                    g.setNode(state.name, {
                        metadata: state,
                        shape: "iconP",
                        label: state.name,
                        labelStyle: "font-size:14px;font-weight:normal;"
                    });
                    g.setEdge(state.name, states.result.ForecastProjectList.level, {
                        labelStyle: "font-size:13px;font-weight:normal;",
                        lineInterpolate: 'basis',
                        arrrowhead: 'vee'
                    });
                });
                if (futureTrendCount == 0) {
                    g.setEdge(states.result.CurrentProjectList.level, states.result.ForecastProjectList.level, {
                        labelStyle: "font-size:13px;font-weight:normal;",
                        lineInterpolate: 'basis',
                        class: "dimEdgePath",
                        arrrowhead: 'vee'
                    });
                }

            }

            // Set some general styles
            g.nodes().forEach(function (v) {
                var node = g.node(v);
            });
            //
            //// Add some custom colors based on state

            if (g.node('BaselineProject')) {
                g.node('BaselineProject').style = "fill: #7AE3EA";
                g.node('BaselineProject').y = 50;
            }
            if (g.node('CurrentProject')) {
                g.node('CurrentProject').style = "fill: #7AE3EA";
                g.node('CurrentProject').y = 50;
            }
            if (g.node('ForecastProject')) {
                g.node('ForecastProject').style = "fill: #7AE3EA";
                g.node('ForecastProject').y = 50;
            }
            if (g.node('ProjectTitle')) {
                g.node('ProjectTitle').style = "fill: #7AE3EA";
                g.node('ProjectTitle').x = 500;
            }

            var oldsvg = d3.select("#trend-tree-container > svg > g");
            oldsvg.selectAll("*").remove();
            var svg = d3.select("#trend-tree-container").select("svg");

            var inner = svg.selectAll("g");
            console.debug("INNER", inner);
            // Set up zoom support
            var zoom = d3.behavior.zoom().on("zoom", function () {

                //  inner.attr("transform", "translate(" + d3.event.translate + ")" + "scale(" + d3.event.scale + ")");
                inner.attr("transform", "translate(" + d3.event.translate + ")" + "scale(" + d3.event.scale + ")");

            });
            svg.call(zoom).on("dblclick.zoom", null);
            // Create the renderer
            var render = new dagreD3.render();
            // Add our custom shape IconA
            render.shapes().iconA = function (parent, bbox, node) {
                var w = bbox.width,
                    h = bbox.height,
                    points = [
                        { x: 0, y: 0 },
                        { x: 30, y: 0 },
                        { x: 30, y: 30 },
                        //{ x: w/2, y: -h * 3/2 },
                        { x: 0, y: 30 }
                    ];
                var shapeSvg = parent.append("image")
                    .attr("xlink:href", function (d) {
                        return "assets/js/wbs-tree/images/nodeB.png"
                    })
                    .attr("x", "-7px")
                    .attr("y", "7px")
                    .attr("width", "15px")
                    .attr("height", "15px");
                node.intersect = function (point) {
                    return dagreD3.intersect.polygon(node, points, point);
                };
                return shapeSvg;
            };
            // Add our custom shape IconP
            render.shapes().iconP = function (parent, bbox, node) {
                var w = bbox.width,
                    h = bbox.height,
                    points = [
                        { x: 0, y: 0 },
                        { x: 30, y: 0 },
                        { x: 30, y: 30 },
                        //{ x: w/2, y: -h * 3/2 },
                        { x: 0, y: 30 }
                    ];
                var shapeSvg = parent.append("image")
                    .attr("xlink:href", function (d) {
                        return "assets/js/wbs-tree/images/nodeC.png"
                    })
                    .attr("x", "-7px")
                    .attr("y", "7px")
                    .attr("width", "15px")
                    .attr("height", "15px");
                node.intersect = function (point) {
                    return dagreD3.intersect.polygon(node, points, point);
                };
                return shapeSvg;
            };
            // Add our custom shape IconR
            render.shapes().iconR = function (parent, bbox, node) {
                var w = bbox.width,
                    h = bbox.height,
                    points = [
                        { x: 15, y: 0 },
                        { x: 30, y: 0 },
                        { x: 30, y: 30 },
                        //{ x: w/2, y: -h * 3/2 },
                        { x: 0, y: 30 }
                    ];
                var shapeSvg = parent.append("image")
                    .attr("xlink:href", function (d) {
                        return "assets/js/wbs-tree/images/nodeD.png"
                    })
                    .attr("x", "-7px")
                    .attr("y", "7px")
                    .attr("width", "15px")
                    .attr("height", "15px");
                node.intersect = function (point) {
                    return dagreD3.intersect.polygon(node, points, point);
                };
                return shapeSvg;
            };
            // Add our custom shape IconCurrent
            render.shapes().iconCurrent = function (parent, bbox, node) {
                console.debug("BBOX", bbox);
                var w = bbox.width + 20,
                    h = bbox.height + 40,
                    points = [
                        { x: 0, y: 0 },
                        { x: w, y: 0 },
                        { x: w, y: h },
                        //{ x: w/2, y: -h * 3/2 },
                        { x: 0, y: h }
                    ];
                var shapeSvg = parent.append("image")
                    .attr("xlink:href", function (d) {
                        return "assets/js/wbs-tree/images/bluebutton.png"
                    })
                    .attr("x", -(w) / 2)
                    .attr("y", -(h) / 2)
                    .attr("width", w)
                    .attr("height", h)
                    .attr("opacity", 0.4) // change this to zero to hide the target area

                node.intersect = function (point) {
                    return dagreD3.intersect.polygon(node, points, point);
                };
                return shapeSvg;
            };
            // Add our custom shape IconCurrent
            render.shapes().iconCurrentText = function (parent, bbox, node) {
                node.x = 500;
                var shapeSvg = parent
                    .attr("x", 0)
                    .attr("y", 0)
                    .attr("style", "margin-left:'100px';")
                    .attr('class', 'nodeText');
                return shapeSvg;
            };

            var svg2 = d3.select("#trend-tree-container > svg > g");
            svg2.selectAll("*").remove();

            render(inner, g);
            if (selProjId != null) {
                var projectName = wbsTrendTree.getProjectName();
                //This is to calculate the lenght of text in pixel
                var canvas = document.createElement('canvas');
                var offset = 20;
                var ctx = canvas.getContext("2d");
                ctx.font = "18px Verdana";
                ctx.fontWeight = "bold";
                //==================================================
                var width = ctx.measureText(projectName).width;
                var tWidth = ((svg.attr('width') - width) / 2) - offset;
                var dy = 0;
                if (g.graph().height > svg.attr('height')) {//svg.attr('height') = 350
                    dy = (svg.attr('height') - (g.graph().height)) / 2;
                } else {
                    dy = ((g.graph().height - svg.attr('height')) / 2) + offset;
                }
                var width = svg2.attr('width');
                console.debug("GRAPH_Width", svg.attr('height'));
                svg2.append('text')
                    .attr("x", tWidth)
                    .attr("y", dy)
                    .attr("style", "font-size:16px;font-weight:bold")
                    .text(function () {
                        return projectName;
                    })
            }
            // Center the graph
            var initialScale = 1;

            var width = 600,
                height = 350,
                center = [width / 2, height / 2];

            //   var initialScale = 0.75;
            var _height = svg.attr('height') - g.graph().height;
            var _width = svg.attr('width') - g.graph().width;
            console.log(height / _height);
            var padding = 50,
                bBox = inner.node().getBBox(),
                hRatio = height / (bBox.height + padding),
                wRatio = width / (bBox.width + padding);
            console.log(bBox);
            console.log(d3.event);
            //  inner.attr("transform", "translate(" + d3.event.translate + ")" + "scale(" + d3.event.scale + ")");
            // inner.attr("transform", "translate(" + d3.event.translate + ")" + "scale(" + d3.event.scale + ")");


            zoom
                .translate([20, (svg.attr("height") - g.graph().height * initialScale) / 2])
                .scale(initialScale)
                .event(svg);

            if (selProjId == 'null') {
                zoom
                    .translate([(svg.attr("width") - g.graph().width * initialScale) / 2, (svg.attr("height") - g.graph().height * initialScale) / 2])
                    .scale(initialScale)
                    .event(svg);
            }

            var wbsTreeNode = this;
            var isDragging = false;
            var temp = d3.selectAll('svg.trendTree g.node')
                .on("click", function (d) {
                    if (isDragging == true) return (g.node(d).metadata);
                    var node = g.node(d);
                    console.log(node);
                    if (wbsTreeNode.getWBSTree().getSelectedOrganizationID() == null) {
                        wbsTreeNode.getWBSTree().setSelectedOrganizationID($("#selectOrg").val());
                    }
                    node.metadata.OrganizationID = wbsTreeNode.getWBSTree().getSelectedOrganizationID();

                    if (node.metadata.level == "PastTrend" || node.metadata.level == "FutureTrend") {
                        window.location.href = "#/app/cost-gantt/" + node.metadata.ProjectID + "/" + node.metadata.TrendNumber + "/" + node.metadata.OrganizationID;
                    } else if (node.metadata.level == "BaselineProject") {
                        window.location.href = "#/app/cost-gantt/" + node.metadata.ProjectID + "/0/" + node.metadata.OrganizationID;
                    }
                    else if (node.metadata.level == "CurrentProject") {
                        // window.location.href = "#/app/current-project/" + node.metadata.ProjectID + "/" + node.metadata.OrganizationID + "/" + "week";
                        //window.location.href = "#/app/cost-gantt/" + node.metadata.ProjectID + "/1000/" + node.metadata.OrganizationID;
                        //Nivedita22-12-2021
                        window.location.href = "#/app/cost-gantt/" + node.metadata.ProjectID + "/3000/" + node.metadata.OrganizationID;
                    }  else if (node.metadata.level == "ForecastProject") {
                        // window.location.href = "#/app/future-project/" + node.metadata.ProjectID + "/" + node.metadata.OrganizationID + "/" + "week";
                        window.location.href = "#/app/cost-gantt/" + node.metadata.ProjectID + "/2000/" + node.metadata.OrganizationID;
                    }
                });

            d3.selectAll('svg.trendTree g.node')
                .on("contextmenu", function (d) {
                    d3.event.preventDefault();
                    console.log("context menu for: " + d); //console.log
                    var node = g.node(d);
                    //node.metadata.level = node.metadata.level == "PastTrend" ? "FutureTrend" : node.metadata.level;
                    metadata = node.metadata;
                    wbsTreeNode.setSelectedTreeNode(node);
                    wbsTreeNode.getScope().trend = node;
                    if (node.metadata.level == "BaselineProject"
                        || node.metadata.level == "ForecastProject"
                        //|| node.metadata.level == "PastTrend" // Jignesh-01-04-2021
                    ) {
                        return;
                    }
                    wbsTreeNode.showContextTrendMenu(node, node.metadata.level);
                    $("#contextMenuAdd").html("Add New");
                    $("#contextMenuScope").parent().hide();
                    if (node.metadata.level == "CurrentProject") {
                        // alert("1");
                        $("#contextMenuAdd").html("Add Trend");
                        $("#contextMenuEdit").parent().hide();
                        $("#contextMenuDelete").parent().hide();
                        $("#contextMenuMap").parent().hide();
                        $("#contextMenuViewGantt").parent().hide();

                    } else if (node.metadata.level == "FutureTrend") { 
                        $("#contextMenuAdd").parent().hide();
                        modal_mode = "Update";
                    }
                    //========================= Jignesh-01-04-2021 =====================
                    else if (node.metadata.level == "PastTrend") {
                        $("#contextMenuDelete").parent().hide();
                        $("#contextMenuAdd").parent().hide();
                        modal_mode = "Update";
                    }
                    //==================================================================
                });
        }

        var pageXx = 0;
        var pageYy = 0;
        //$(document).mousemove(function (event) {
        $('body').mousemove(function (event) {
            pageXx = event.pageX;
            pageYy = event.pageY;

        });
        obj.prototype.showContextTrendMenu = function (node, type) {

            if (wbsTree.getLocalStorage().acl[6] == 0 && wbsTree.getLocalStorage().acl[7] == 0) {
                dhtmlx.alert("Sorry, you cannot access this.");
                return;
            }
            //var svg_graph_wbs = d3.select("#tree-container").select("svg > g");
            //var svg_rect_wbs = svg_graph_wbs[0][0].getBBox();
            //var g_width = svg_rect_wbs.width;
            //var g_height = svg_rect_wbs.height;
            //var svg_graph = d3.select("#trend-tree-container").select("svg > g");
            //var svg_rect = svg_graph[0][0].getBBox();
            //var zoom = d3.behavior.zoom();
            //var g_scale = zoom.scale();
            //var xOffset = svg_rect.x;
            //var yOffset = svg_rect.y;
            //var pageY = g_height;
            //var pageX = node.x + 130 + g_width - (xOffset * g_scale * 2);
            $("#contextMenu").attr('contextType', type);
            //====================== Jignesh-01-04-2021 ====================================
            if (type == "PastTrend") {
                //$("#contextMenuEdit").html("View Trend");
                if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                    $("#contextMenuEdit").html("Open Trend");
                } else {
                    $("#contextMenuEdit").html("Edit/Open Trend");
                }
            }
            //==============================================================================
            else {
                if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                    $("#contextMenuEdit").html("Open Trend");
                } else {
                    $("#contextMenuEdit").html("Edit/Open Trend");
                }

            }
            // alert("2");
            $("#contextMenuEdit").parent().show();
            if (wbsTree.getLocalStorage().acl[7] == 1) {
                // alert("2");
                $("#contextMenuAdd").parent().show();
                $("#contextMenuDelete").parent().show();
            } else {
                $("#contextMenuDelete").parent().hide();
            }
            $("#contextMenuMap").parent().hide();
            //var currentZoom = ((window.outerWidth - 10) / window.innerWidth);
            //currentZoom = (currentZoom > 1) ? currentZoom : 1;

            showContextMenu(node);
        }

        //    $("#contextMenu").css({
        //        display: "block",
        //        left: (pageXx * currentZoom), //the position of the mouse is different relatvie to the screen based on the brwoser zoom
        //        top: (pageYy * currentZoom)
        //        // left: pageXx + 725, // Jignesh-TDM-06-01-2020
        //        // top: pageYy + 250 // Jignesh-TDM-06-01-2020
        //    });
        //    $('#contextMenu').css({
        //        'margin-top': '',
        //        'margin-left': '-4em'
        //    });
        //}

        function showContextMenu(node) {
            var svg_graph_wbs = d3.select("#tree-container").select("svg > g");
            var svg_rect_wbs = svg_graph_wbs[0][0].getBBox();
            console.log(svg_rect_wbs);
            var g_width = svg_rect_wbs.width;
            var g_height = svg_rect_wbs.height;
            var svg_graph = d3.select("#trend-tree-container").select("svg > g");
            var svg_rect = svg_graph[0][0].getBBox();
            var zoom = d3.behavior.zoom();
            var g_scale = zoom.scale();
            var xOffset = svg_rect.x;
            var yOffset = svg_rect.y;
            var pageY = node.x + 200 - (xOffset * g_scale * 2);
            var pageX = node.y - 50 - (yOffset * g_scale * 2);

            //Get current Browser zoom level
            var browserZoom = ((window.outerWidth - 10) / window.innerWidth);
            browserZoom = (browserZoom < 1) ? browserZoom : 1;
            //0.7 and 0.9 zoom level on a 1366 not working properly
            if (window.outerWidth <= 1366 && (browserZoom >= 0.7 && browserZoom <= 1))
                browserZoom = 1;
            var mediaZoom = $('html').css('zoom');
            var scaleX = $('html').css('transform').split(',')[0];
            var scaleY = $('html').css('transform').split(',')[3];
            scaleY = (scaleY) ? scaleY.trim() : 1;
            scaleX = (scaleX) ? scaleX.split('(')[1] : scaleX;
            scaleX = (scaleX) ? scaleX : 1;
            $("#contextMenu").css({
                display: "block",
                left: ((pageXx / mediaZoom) / scaleX) * browserZoom,
                top: (((pageYy / mediaZoom) / scaleY) * browserZoom) + $('body').scrollTop()
            });
            $('#contextMenu').css({
                'margin-top': '',
                'margin-left': '-4%'
            });
        }

        obj.prototype.bindEventsForTrendNodes = function () {
            wbsTrendTree = this;
            //$('#PastTrendModal').on('show.bs.modal', function (event) {
            //    var selectedNode = wbsTrendTree.getWBSTree().getSelectedNode();
            //    var selectedNodeTrend = wbsTrendTree.getSelectedTreeNode();
            //    modal = $(this);
            //    if (selectedNodeTrend) {
            //        metadata = selectedNodeTrend.metadata;
            //    }
            //    metadata.name = "Trend";
            //    modal.find('.modal-title').text("Trend");
            //    modal.find('.modal-body #trend_number').html(metadata.TrendNumber);
            //    modal.find('.modal-body #trend_description').val(metadata.TrendDescription);
            //    modal.find('.modal-body #trend_justification').val(metadata.TrendJustification);
            //    //modal.find('.modal-body #trend_impact').val(metadata.TrendImpact);
            //    $("input[name=trend_impact][value='" + metadata.TrendImpact + "']").prop("checked", true);
            //    $("input[name=trend_impact_schedule][value='" + metadata.TrendImpactSchedule + "']").prop("checked", true);  //Manasi 13-07-2020
            //    $("input[name=trend_impact_cost_schedule][value='" + metadata.TrendImpactCostSchedule + "']").prop("checked", true); //Manasi 13-07-2020
            //    modal.find('.modal-body #approval_from').val(metadata.ApprovalFrom);
            //    modal.find('.modal-body #approval_date').val(metadata.ApprovalDate ? metadata.ApprovalDate : ""); // Jignesh-01-03-2021
            //});
            function defaultModalPosition() {
                $('.modal-dialog').css('top', '');
                $('.modal-dialog').css('left', '');
            }
            $('#PastTrendModal').on('show.bs.modal', function (event) {

                var zIndex = 1041;
                $('#PastTrendModal').css('z-index', zIndex);

                $('#divPastTrendChangeOrderDD').hide(); // Jignesh 30-10-2020
                $("#pastnochngreq").prop("checked", true); // Jignesh 30-10-2020
                $('#pasttrend_client_approved_date').datepicker(); // Jignesh-26-02-2021
                $('#secPastTrendApprovedDate').hide(); // Jignesh-02-03-2021
                defaultModalPosition();
                var selectedNode = wbsTrendTree.getWBSTree().getSelectedNode();
                var s = wbsTrendTree.getWBSTree().getNewTrend();
                var selectedNodeTrend = wbsTrendTree.getSelectedTreeNode();
                console.log(selectedNodeTrend);
                modal = $(this);

                modal.find('.modal-body #pasttrend_approval_date').datepicker(); // Jignesh-26-02-2021

                //luan here
                //Populate trend status code for dropdown
                wbsTrendTree.getWBSTree().getTrendStatusCode().get({}, function (response) {
                    var trendStatusCodeDropDown = modal.find('.modal-body #pasttrend_status_code');
                    var laborRateDropDown = modal.find('.modal-body #labor_rate_code');
                    laborRateDropDown.empty();
                    console.log(costOverheadTypes);
                    for (var x = 0; x < costOverheadTypes.length; x++) {
                        laborRateDropDown.append('<option selected="false">' + costOverheadTypes[x].CostOverHeadType + '</option>')
                    }
                    laborRateDropDown.attr('disabled', false);
                    laborRateDropDown.val("");
                    trendStatusCodeList = response.result;

                    TRENDSTATUSCODELIST = response.result;

                    trendStatusCodeDropDown.empty();
                    console.log(trendStatusCodeList);
                    for (var x = 0; x < trendStatusCodeList.length; x++) {
                        if (trendStatusCodeList[x].TrendStatusCodeName == null) {
                            continue;
                        }
                        trendStatusCodeDropDown.append('<option selected="false">' + trendStatusCodeList[x].TrendStatusCodeName + '</option>');
                        trendStatusCodeDropDown.val('');
                    }

                    if (metadata.TrendNumber) {//Update trend
                        //luan here - Find the trend status code name given id
                        var trendStatusCodeList = TRENDSTATUSCODELIST;
                        var trendStatusCodeName = null;
                        var selectedTrendStatusCode = modal.find('.modal-body #pasttrend_status_code');
                        for (var x = 0; x < trendStatusCodeList.length; x++) {
                            if (trendStatusCodeList[x].TrendStatusCodeID == metadata.TrendStatusCodeID) {
                                trendStatusCodeName = trendStatusCodeList[x].TrendStatusCodeName;
                            }
                        }
                        selectedTrendStatusCode.val(trendStatusCodeName);
                    } else {
                        //Inherit the labor rate from baseline when creating new trend
                        wbsTree.getTrendId().get({ trendId: 0, projectId: metadata.ProjectID }, function (response) {
                            console.log(response);
                            var costOverheadID = response.result.CostOverheadTypeID;
                            for (var x = 0; x < costOverheadTypes.length; x++) {
                                if (costOverheadTypes[x].ID == costOverheadID) {
                                    modal.find('.modal-body #labor_rate_code').val(costOverheadTypes[x].CostOverHeadType);
                                    modal.find('.modal-body #labor_rate_code').attr('disabled', false);
                                }

                            }
                        });
                    }
                });

                if (selectedNodeTrend) {
                    metadata = selectedNodeTrend.metadata;
                    if (s == true)
                        metadata.TrendNumber = "";
                    wbsTrendTree.setSelectedTreeNode(null);
                } else {
                    metadata = {};
                    metadata.ProjectID = selectedNode.ProjectID;
                }
                //modal_mode = 'Update';
                modal.find('.modal-title').text("Trend");
                var trendNumber = wbsTrendTree.getTrendNumber();
                

                modal_mode = 'Update';
                modal.find('.c-modal-footer #approve_trend').show();
                modal.find('.modal-body #pasttrend_number').html(metadata.TrendNumber);
                modal.find('.modal-body #pasttrend_description').val(metadata.TrendDescription);
                modal.find('.modal-body #pasttrend_justification').val(metadata.TrendJustification);
                //modal.find('.modal-body #trend_impact').val(metadata.TrendImpact);
                $("input[name=pasttrend_impact][value='" + metadata.TrendImpact + "']").prop("checked", true);
                $("input[name=pasttrend_impact_schedule][value='" + metadata.TrendImpactSchedule + "']").prop("checked", true); //Manasi 13-07-2020
                $("input[name=pasttrend_impact_cost_schedule][value='" + metadata.TrendImpactCostSchedule + "']").prop("checked", true); //Manasi 13-07-2020

                // Jignesh 31-12-2020
                if (metadata.IsApprovedByClient == 1) {
                    modal.find('.modal-body #chkPastTrendApprovalByClient').prop("checked", true);
                    modal.find('.modal-body #secPastTrendApprovedDate').show();
                    //$("#chkPastTrendApprovalByClient").prop("checked", true);
                    //$('#secPastTrendApprovedDate').show(); // Jignesh-23-02-2021
                }
                else {
                    modal.find('.modal-body #chkPastTrendApprovalByClient').prop("checked", false);
                    modal.find('.modal-body #secPastTrendApprovedDate').hide();
                    //$("#chkPastTrendApprovalByClient").prop("checked", false);
                    //$('#secPastTrendApprovedDate').hide(); // Jignesh-23-02-2021
                }

                modal.find('.modal-body #approval_from').val(metadata.ApprovalFrom);
                modal.find('.modal-body #pasttrend_approval_date').val(metadata.ApprovalDate ? moment(metadata.ApprovalDate).format('MM/DD/YYYY') : ""); // Jignesh-01-03-2021
                modal.find('.modal-body #pasttrend_client_approved_date').val(metadata.ClientApprovedDate ? moment(metadata.ClientApprovedDate).format('MM/DD/YYYY') : ""); // Jignesh-26-02-2021
                //====================================== Jignesh-TDM-06-01-2020 =======================================
                //$('#DeleteUploadTrend').attr('disabled', 'disabled');
                //$('#updateDMBtnTrend').removeAttr('disabled');
                //$('#ViewUploadFileTrend').attr('disabled', 'disabled');
                //$('#downloadBtnTrend').attr('disabled', 'disabled');
                //$('#documentUploadTrend').removeAttr('title');   //Manasi 23-02-2021
                //$('#delete_future_trend').removeAttr('disabled');   //Manasi 24-02-2021
                //$('#spnBtndelete_future_trend').removeAttr('title');   //Manasi 24-02-2021
                //============================================================================================================

                //====================================== Created By Jignesh 28-10-2020 =======================================



                wbsTrendTree.getWBSTree().getChangeOrder().get({}, function (changeOrderData) {
                    var changeOrderList = changeOrderData.result;
                    var changeOrderCodeDropDown = modal.find('.modal-body #pasttrend_change_order');
                    changeOrderCodeDropDown.empty();



                    for (var x = 0; x < changeOrderList.length; x++) {
                        if (changeOrderList[x].ProgramElementID == metadata.ProgramElementId) {
                            if (changeOrderList[x].ChangeOrderID == metadata.ChangeOrderID) {
                                changeOrderCodeDropDown.append('<option value="' + changeOrderList[x].ChangeOrderID + '" selected> ' + changeOrderList[x].ChangeOrderName + '</option>');
                                changeOrderCodeDropDown.val(changeOrderList[x].ChangeOrderID);
                            } else {
                                changeOrderCodeDropDown.append('<option value="' + changeOrderList[x].ChangeOrderID + '"> ' + changeOrderList[x].ChangeOrderName + '</option>');
                            }
                        }
                    }



                    if (metadata.IsChangeRequest == 1) {
                        modal.find('.modal-body #pastchngreq').prop("checked", true);
                        modal.find('.modal-body #divPastTrendChangeOrderDD').show();
                        //$("#pastchngreq").prop("checked", true);
                        //$('#divPastTrendChangeOrderDD').show();
                    } else {
                        modal.find('.modal-body #pastnochngreq').prop("checked", true);
                        modal.find('.modal-body #divPastTrendChangeOrderDD').hide();
                        //$("#pastnochngreq").prop("checked", true);
                        //$('#divPastTrendChangeOrderDD').hide();
                        changeOrderCodeDropDown.val('');
                    }


                    // Jignesh 31-12-2020
                    if (metadata.IsApprovedByClient == 1) {
                        $("#chkPastTrendApprovalByClient").prop("checked", true);
                    }
                    else {
                        $("#chkPastTrendApprovalByClient").prop("checked", false);
                    }
                });



                //============================================================================================================
                //====================================== Jignesh-TDM-06-01-2020 =======================================
                _Document.getDocumentByProjID().get({ DocumentSet: 'Project', ProjectID: _selectedProjectID }, function (response) {
                    wbsTree.setDocumentList(response.result);
                    var trendNumber = metadata.TrendNumber; // 15-01-2021
                    var gridUploadedDocument = $('#gridUploadedDocumentTrend tbody');
                    gridUploadedDocument.empty();
                    for (var x = 0; x < _documentList.length; x++) {
                        if (_documentList[x].TrendNumber == trendNumber) {
                            //Edited by Jignesh (29-10-2020)
                            gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesTrend" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                '</td > <td ' +
                                'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '>' + _documentList[x].DocumentTypeName + '</td>' +
                                '<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                '<tr > ');
                        }
                    }

                    var deleteDocBtn = modal.find('.modal-body #delete-doc');
                    deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                    //======================== Jignesh-05-03-2021 ========================================
                    $('input[name=rbCategoriesTrend]').on('click', function (event) {
                        if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                            $('#DeleteUploadTrend').attr('disabled', 'disabled');
                            $('#updateDMBtnTrend').attr('disabled', 'disabled');
                            $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                            $('#downloadBtnTrend').attr('disabled', 'disabled');
                        } else {
                            $('#DeleteUploadTrend').removeAttr('disabled');
                            $('#updateDMBtnTrend').removeAttr('disabled');
                            $('#ViewUploadFileTrend').removeAttr('disabled');
                            $('#downloadBtnTrend').removeAttr('disabled');
                        }
                    });
                    //=====================================================================================
                });
                //============================================================================================================
                

            });

            //============== Jignesh-31-03-2021 =================================================================

            $('#cancel_PastTrend,#btnCancelPastTrend_X').unbind('click').on('click', function () {
                $('#PastTrendModal').modal('hide');
            });
            //====================================================================================================
            function defaultModalPosition() {
                $('.modal-dialog').css('top', '');
                $('.modal-dialog').css('left', '');
            }


            $('#FutureTrendModal').on('show.bs.modal', function (event) {
                modal = $(this);
                $('#divChangeOrderDD').hide(); // Jignesh 30-10-2020
                $("#nochngreq").prop("checked", true); // Jignesh 30-10-2020
                $('#client_approved_date').datepicker("destroy"); // Jignesh-15-04-2021
                $('#client_approved_date').datepicker(); // Jignesh-26-02-2021
                $('#secApprovedDate').hide(); // Jignesh-02-03-2021
                defaultModalPosition();
                var selectedNode = wbsTrendTree.getWBSTree().getSelectedNode();
                var s = wbsTrendTree.getWBSTree().getNewTrend();
                var selectedNodeTrend = wbsTrendTree.getSelectedTreeNode();
                console.log(selectedNodeTrend);
                
                modal.find('.modal-body #approval_date').datepicker("destroy"); // Jignesh-15-04-2021
                modal.find('.modal-body #approval_date').datepicker(); // Jignesh-26-02-2021

                //luan here
                //Populate trend status code for dropdown
                wbsTrendTree.getWBSTree().getTrendStatusCode().get({}, function (response) {
                    var trendStatusCodeDropDown = modal.find('.modal-body #trend_status_code');
                    var laborRateDropDown = modal.find('.modal-body #labor_rate_code');
                    laborRateDropDown.empty();
                    console.log(costOverheadTypes);
                    for (var x = 0; x < costOverheadTypes.length; x++) {
                        laborRateDropDown.append('<option selected="false">' + costOverheadTypes[x].CostOverHeadType + '</option>')
                    }
                    laborRateDropDown.attr('disabled', false);
                    laborRateDropDown.val("");
                    trendStatusCodeList = response.result;

                    TRENDSTATUSCODELIST = response.result;

                    trendStatusCodeDropDown.empty();
                    console.log(trendStatusCodeList);
                    for (var x = 0; x < trendStatusCodeList.length; x++) {
                        if (trendStatusCodeList[x].TrendStatusCodeName == null) {
                            continue;
                        }
                        trendStatusCodeDropDown.append('<option selected="false">' + trendStatusCodeList[x].TrendStatusCodeName + '</option>');
                        trendStatusCodeDropDown.val('');
                    }

                    if (metadata.TrendNumber) {//Update trend
                        //luan here - Find the trend status code name given id
                        var trendStatusCodeList = TRENDSTATUSCODELIST;
                        var trendStatusCodeName = null;
                        var selectedTrendStatusCode = modal.find('.modal-body #trend_status_code');
                        for (var x = 0; x < trendStatusCodeList.length; x++) {
                            if (trendStatusCodeList[x].TrendStatusCodeID == metadata.TrendStatusCodeID) {
                                trendStatusCodeName = trendStatusCodeList[x].TrendStatusCodeName;
                            }
                        }
                        selectedTrendStatusCode.val(trendStatusCodeName);

                        //luan here - Find boolean value for internal (commented by Jignesh 30-10-2020)
                        //var isInternal = null;
                        //var selectedIsInternal = modal.find('.modal-body #trend_is_internal');
                        //if (metadata.IsInternal) {
                        //	isInternal = true;
                        //} else {
                        //	isInternal = false;
                        //}
                        //selectedIsInternal[0].checked = isInternal;;

                        //--------------Manasi--------------------
                        //var selectedLaborRateCode = modal.find('.modal-body #labor_rate_code');
                        //console.log(metadata);
                        //for (var x = 0; x < costOverheadTypes.length; x++) {
                        //    if (costOverheadTypes[x].ID == metadata.CostOverheadTypeID) {
                        //        selectedLaborRateCode.val(costOverheadTypes[x].CostOverHeadType);
                        //        selectedLaborRateCode.attr('disabled', true); //Don't allow the user to update this now
                        //    }
                        //}
                        //----------------------------------
                    } else {
                        //Inherit the labor rate from baseline when creating new trend
                        wbsTree.getTrendId().get({ trendId: 0, projectId: metadata.ProjectID }, function (response) {
                            console.log(response);
                            var costOverheadID = response.result.CostOverheadTypeID;
                            for (var x = 0; x < costOverheadTypes.length; x++) {
                                if (costOverheadTypes[x].ID == costOverheadID) {
                                    modal.find('.modal-body #labor_rate_code').val(costOverheadTypes[x].CostOverHeadType);
                                    modal.find('.modal-body #labor_rate_code').attr('disabled', false);
                                }

                            }
                        });
                    }
                });

                if (selectedNodeTrend) {
                    metadata = selectedNodeTrend.metadata;
                    if (s == true)
                        metadata.TrendNumber = "";
                    wbsTrendTree.setSelectedTreeNode(null);
                } else {
                    metadata = {};
                    metadata.ProjectID = selectedNode.ProjectID;
                }
                //modal_mode = 'Update';
                modal.find('.modal-title').text("Trend");
                var trendNumber = wbsTrendTree.getTrendNumber();
                if (!metadata.TrendNumber || metadata.TrendNumber == "") {
                    $('#cancel_futuretrend').show();
                    modal_mode = 'Create';
                    modal.find('.c-modal-footer #approve_trend').hide();
                    //modal.find('.modal-body #trend_number').val(_trendNumber);
                    modal.find('.modal-body #trend_number').html(trendNumber);
                    modal.find('.modal-body #trend_description').val("");
                    modal.find('.modal-body #trend_justification').val("");
                    //modal.find('.modal-body #trend_impact').val("");
                    $("input[name=trend_impact][value='Cost']").prop("checked", true);
                    $("input[name=trend_impact_schedule][value='Schedule']").prop("checked", true);  //Manasi 13-07-2020
                    $("input[name=trend_impact_cost_schedule][value='Cost & Schedule']").prop("checked", true);  //Manasi 13-07-2020
                    $("#chkTrendApprovalByClient").prop("checked", false); // Jignesh 31-12-2020
                    modal.find('.modal-body #approval_from').val("");
                    modal.find('.modal-body #approval_date').val("");
                    modal.find('.modal-body #client_approved_date').val(""); // Jignesh-26-02-2021
                    //====================================== Jignesh-TDM-06-01-2020 =======================================
					var gridUploadedDocument = $('#gridUploadedDocumentTrend tbody'); // 15-01-2021
                    gridUploadedDocument.empty(); // 15-01-2021
                    $('#DeleteUploadTrend').attr('disabled', 'disabled');
                    $('#updateDMBtnTrend').attr('disabled', 'disabled');
                    $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                    $('#EditBtnTrend').attr('disabled', 'disabled');
                    $('#downloadBtnTrend').attr('disabled', 'disabled');
                    $('#documentUploadTrend').attr('title', "A trend needs to be saved before the documents can be uploaded"); //Manasi 23-02-2021
                    $('#delete_future_trend').attr('disabled', 'disabled');   //Manasi 24-02-2021
                    $('#spnBtndelete_future_trend').attr('title', "A trend needs to be saved before it can be deleted"); //Manasi 24-02-2021
                    //============================================================================================================
                    //====================================== Created By Jignesh 28-10-2020 =======================================
                    wbsTrendTree.getWBSTree().getChangeOrder().get({}, function (changeOrderData) {
                        var changeOrderList = changeOrderData.result;
                        var programElementID = wbsTree.getSelectedProgramElementID();

                        var changeOrderCodeDropDown = modal.find('.modal-body #trend_change_order');
                        changeOrderCodeDropDown.empty();

                        for (var x = 0; x < changeOrderList.length; x++) {
                            if (changeOrderList[x].ProgramElementID == programElementID) {
                                changeOrderCodeDropDown.append('<option value="' + changeOrderList[x].ChangeOrderID + '"> ' + changeOrderList[x].ChangeOrderName + '</option>');
                                changeOrderCodeDropDown.val('');
                            }
                        }

                    });
                    //============================================================================================================

                    return;
                }

                modal_mode = 'Update';
                modal.find('.c-modal-footer #approve_trend').show();
                modal.find('.modal-body #trend_number').html(metadata.TrendNumber);
                modal.find('.modal-body #trend_description').val(metadata.TrendDescription);
                modal.find('.modal-body #trend_justification').val(metadata.TrendJustification);
                //modal.find('.modal-body #trend_impact').val(metadata.TrendImpact);
                $("input[name=trend_impact][value='" + metadata.TrendImpact + "']").prop("checked", true);
                $("input[name=trend_impact_schedule][value='" + metadata.TrendImpactSchedule + "']").prop("checked", true); //Manasi 13-07-2020
                $("input[name=trend_impact_cost_schedule][value='" + metadata.TrendImpactCostSchedule + "']").prop("checked", true); //Manasi 13-07-2020

                // Jignesh 31-12-2020
                if (metadata.IsApprovedByClient == 1) {
                    $("#chkTrendApprovalByClient").prop("checked", true);
                    $('#secApprovedDate').show(); // Jignesh-23-02-2021
                }
                else {
                    $("#chkTrendApprovalByClient").prop("checked", false);
                    $('#secApprovedDate').hide(); // Jignesh-23-02-2021
                }

                modal.find('.modal-body #approval_from').val(metadata.ApprovalFrom);
                modal.find('.modal-body #approval_date').val(metadata.ApprovalDate ? moment(metadata.ApprovalDate).format('MM/DD/YYYY') : ""); // Jignesh-01-03-2021
                modal.find('.modal-body #client_approved_date').val(metadata.ClientApprovedDate ? moment(metadata.ClientApprovedDate).format('MM/DD/YYYY') : ""); // Jignesh-26-02-2021
                //====================================== Jignesh-TDM-06-01-2020 =======================================
                $('#DeleteUploadTrend').attr('disabled', 'disabled');
                $('#updateDMBtnTrend').removeAttr('disabled');
                $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                $('#EditBtnTrend').attr('disabled', 'disabled');
                $('#downloadBtnTrend').attr('disabled', 'disabled');
                $('#documentUploadTrend').removeAttr('title');   //Manasi 23-02-2021
                $('#delete_future_trend').removeAttr('disabled');   //Manasi 24-02-2021
                $('#spnBtndelete_future_trend').removeAttr('title');   //Manasi 24-02-2021
                //============================================================================================================

                //====================================== Created By Jignesh 28-10-2020 =======================================



                wbsTrendTree.getWBSTree().getChangeOrder().get({}, function (changeOrderData) {
                    var changeOrderList = changeOrderData.result;
                    var changeOrderCodeDropDown = modal.find('.modal-body #trend_change_order');
                    changeOrderCodeDropDown.empty();



                    for (var x = 0; x < changeOrderList.length; x++) {
                        if (changeOrderList[x].ProgramElementID == metadata.ProgramElementId) {
                            if (changeOrderList[x].ChangeOrderID == metadata.ChangeOrderID) {
                                changeOrderCodeDropDown.append('<option value="' + changeOrderList[x].ChangeOrderID + '" selected> ' + changeOrderList[x].ChangeOrderName + '</option>');
                                changeOrderCodeDropDown.val(changeOrderList[x].ChangeOrderID);
                            } else {
                                changeOrderCodeDropDown.append('<option value="' + changeOrderList[x].ChangeOrderID + '"> ' + changeOrderList[x].ChangeOrderName + '</option>');
                            }
                        }
                    }



                    if (metadata.IsChangeRequest == 1) {
                        $("#chngreq").prop("checked", true);
                        $('#divChangeOrderDD').show();
                    } else {
                        $("#nochngreq").prop("checked", true);
                        $('#divChangeOrderDD').hide();
                        changeOrderCodeDropDown.val('');
                    }


                    // Jignesh 31-12-2020
                    if (metadata.IsApprovedByClient == 1) {
                        $("#chkTrendApprovalByClient").prop("checked", true);
                    }
                    else {
                        $("#chkTrendApprovalByClient").prop("checked", false);
                    }
                });



                //============================================================================================================
                //====================================== Jignesh-TDM-06-01-2020 =======================================
                _Document.getDocumentByProjID().get({ DocumentSet: 'Project', ProjectID: _selectedProjectID }, function (response) {
                    wbsTree.setDocumentList(response.result);
                    var trendNumber = metadata.TrendNumber; // 15-01-2021
                    var gridUploadedDocument = $('#gridUploadedDocumentTrend tbody');
                    gridUploadedDocument.empty();
                    for (var x = 0; x < _documentList.length; x++) {
                        if (_documentList[x].TrendNumber == trendNumber) {
                            //Edited by Jignesh (29-10-2020)
                            gridUploadedDocument.append('<tr id="' + _documentList[x].DocumentID + '"><td style="width: 20px">' +
                                '<input id=rb' + _documentList[x].DocumentID + ' type="radio"  name="rbCategoriesTrend" value="' + serviceBasePath + 'Request/DocumentByDocID/' + _documentList[x].DocumentID + '" />' + //jignesh2111
                                '</td > <td ' +
                                'style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '><a>' + _documentList[x].DocumentName + '</a></td> ' +
                                '<td style=" overflow: hidden; text-overflow: ellipsis; white-space: nowrap; "' +
                                '>' + _documentList[x].DocumentTypeName + '</td>' +
                                '<td><input type="button" name="btnViewDetail"  id="viewDocumentDetail" style="color:white;background-color: #0c50e8;" value="View"/></td>' +
                                '<td class="docId" style="display:none;"><span>' + _documentList[x].DocumentID + '</span></td>' +
                                '<tr > ');
                        }
                    }

                    var deleteDocBtn = modal.find('.modal-body #delete-doc');
                    deleteDocBtn.attr('disabled', _documentList.length > 0 ? false : true);
                    //======================== Jignesh-05-03-2021 ========================================
                    $('input[name=rbCategoriesTrend]').on('click', function (event) {
                        if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                            $('#DeleteUploadTrend').attr('disabled', 'disabled');
                            $('#updateDMBtnTrend').attr('disabled', 'disabled');
                            $('#ViewUploadFileTrend').attr('disabled', 'disabled');
                            $('#EditBtnTrend').attr('disabled', 'disabled');
                            $('#downloadBtnTrend').attr('disabled', 'disabled');
                        } else {
                            $('#DeleteUploadTrend').removeAttr('disabled');
                            $('#updateDMBtnTrend').removeAttr('disabled');
                            $('#ViewUploadFileTrend').removeAttr('disabled');
                            $('#EditBtnTrend').removeAttr('disabled');
                            $('#downloadBtnTrend').removeAttr('disabled');
                        }
                        localStorage.selectedTrendDocument = $(this).closest("tr").find(".docId").text();
                    });
                    //=====================================================================================
                });
                //============================================================================================================
                if (wbsTree.getLocalStorage().acl[6] == 1 && wbsTree.getLocalStorage().acl[7] == 0) {
                    $('#delete_future_trend').attr('disabled', 'disabled');
                    $('#update_trend').attr('disabled', 'disabled');
                    $('#updateDMBtnTrend').attr('disabled', 'disabled'); // Jignesh-05-03-2021
                } else {
                    $('#delete_future_trend').removeAttr('disabled');
                    $('#update_trend').removeAttr('disabled');
                    $('#updateDMBtnTrend').removeAttr('disabled'); // Jignesh-05-03-2021
                }

            });

            $('#CurrentProjectModal').on('show.bs.modal', function (event) {
                modal = $(this);
                modal.find('.modal-title').text('Project: ' + metadata.ProjectName);
                modal.find('.modal-body #project_name').val(metadata.ProjectName);
                modal.find('.modal-body #project_manager').val(metadata.ProjectManager);
                modal.find('.modal-body #project_sponsor').val(metadata.ProjectSponsor);
            });
            $('#new_trend').unbind('click').on('click', function () {
                modal_mode = "Create";
            });
            $("#PastTrendModal").on('hidden.bs.modal', function (event) {
                var s = wbsTrendTree.getSelectedTreeNode();
                // s.metadata.TrendNumber = ""; Jignesh-08-04-2021
                wbsTrendTree.setSelectedTreeNode(s);
            });
            $('#FutureTrendModal').on('hidden.bs.modal', function (event) {
                var s = wbsTrendTree.getSelectedTreeNode();
                var wbstree = wbsTrendTree.getWBSTree();
                wbstree.setNewTrend(false);
                wbsTrendTree.setSelectedTreeNode(s);
            });

            $('#update_trend').unbind('click').on('click', function () {
                var TrendTileCheck = modal.find('.modal-body #trend_description').val();
                if (TrendTileCheck == "" || TrendTileCheck.length == 0) {
                    dhtmlx.alert('Enter Trend Title.');
                    return false;
                }
                if (modal_mode == 'Update') {
                    //luan here - Find the trend status code id
                    var trendStatusCodeList = TRENDSTATUSCODELIST;
                    var trendStatusCodeID = null;
                    var selectedTrendStatusCode = modal.find('.modal-body #trend_status_code');
                    console.log(selectedTrendStatusCode, selectedTrendStatusCode.val());
                    for (var x = 0; x < trendStatusCodeList.length; x++) {
                        console.log(trendStatusCodeList[x].TrendStatusCodeName, selectedTrendStatusCode.val());
                        if (trendStatusCodeList[x].TrendStatusCodeName == selectedTrendStatusCode.val()) {
                            trendStatusCodeID = trendStatusCodeList[x].TrendStatusCodeID;
                        }
                    }
                    //====================================== Created By Jignesh 30-10-2020 =======================================

                    var selectedIsChangeRequest = $('input[type=radio][name=chngreq]:checked').val();
                    var isChangeRequest = null;
                    var changeOrderId = null;
                    var isApprovedByClient = null; // Jignesh 31-12-2020

                    //Jignesh 31-12-2020
                    if (selectedIsChangeRequest == 'Change_Request') {
                        isChangeRequest = 1;
                        changeOrderId = $('#trend_change_order').val();
                        isApprovedByClient = $('#chkTrendApprovalByClient:checked').val();
                        if (isApprovedByClient) {
                            isApprovedByClient = 1;
                        }
                        else {
                            isApprovedByClient = 0;
                        }
                    }
                    else if (selectedIsChangeRequest == 'No_Change_Request') {
                        isChangeRequest = 0;
                        isApprovedByClient = 0;
                    }

                    //============================================================================================================
                    //luan here - Find the boolean value of internal Commented by Jignesh 30-10-2020
                    //var selectedIsInternal = modal.find('.modal-body #trend_is_internal');
                    //var isInternal = null;
                    //if (selectedIsInternal[0].checked) {
                    //	isInternal = 1;
                    //} else {
                    //	isInternal = 0;
                    //}

                    // Manasi
                    //var costOverheadTypeID = null;
                    //var selectedCostOverheadType = modal.find('.modal-body #labor_rate_code');
                    //for (var x = 0; x < costOverheadTypes.length; x++) {
                    //    if (costOverheadTypes[x].CostOverHeadType == selectedCostOverheadType.val())
                    //        costOverheadTypeID = costOverheadTypes[x].ID;
                    //}

                    //API to Insert/Update
                    //Parameterize JSON
                    console.log(metadata);
                    var toUpdateTrend = {
                        "Operation": "5",   //update trend setup
                        "ProjectID": metadata.ProjectID,
                        "TrendNumber": metadata.TrendNumber,
                        "TrendDescription": modal.find('.modal-body #trend_description').val(),
                        "TrendStatusID": 3,
                        "TrendStatusCodeID": trendStatusCodeID,
                        "IsChangeRequest": isChangeRequest,
                        "ChangeOrderID": changeOrderId,
                        //"IsInternal": isInternal,
                        "IsApprovedByClient": isApprovedByClient,     //Jignesh 05-01-2021
                        "ClientApprovedDate": modal.find('.modal-body #client_approved_date').val(), // Jignesh-09-02-2021
                        "TrendJustification": modal.find('.modal-body #trend_justification').val(),
                        "TrendImpact": modal.find('.modal-body input[name="trend_impact"]:checked').val(),
                        "TrendImpactSchedule": modal.find('.modal-body input[name="trend_impact_schedule"]:checked').val(),  //Manasi 13-07-2020
                        "TrendImpactCostSchedule": modal.find('.modal-body input[name="trend_impact_cost_schedule"]:checked').val(),  //Manasi 13-07-2020
                        "ApprovalFrom": modal.find('.modal-body #approval_from').val(),
                        "ApprovalDate": modal.find('.modal-body #approval_date').val(),
                        "CreatedOn": metadata.CreatedOn,
                        //"costOverheadTypeID" : costOverheadTypeID

                        "costOverheadTypeID": 1   //Manasi
                    };

                    console.log(toUpdateTrend);
                    _Trend.persist().save(toUpdateTrend, function (response) {
                        isFieldValueChanged = false; // Jignesh-31-03-2021
                        metadata.TrendJustification = modal.find('.modal-body #trend_justification').val();
                        metadata.TrendImpact = modal.find('.modal-body input[name="trend_impact"]:checked').val();
                        metadata.TrendImpactSchedule = modal.find('.modal-body input[name="trend_impact_schedule"]:checked').val();   //Manasi 13-07-2020
                        metadata.TrendImpactCostSchedule = modal.find('.modal-body input[name="trend_impact_cost_schedule"]:checked').val();    //Manasi 13-07-2020
                        metadata.ApprovalFrom = modal.find('.modal-body #approval_from').val();
                        metadata.ApprovalDate = modal.find('.modal-body #approval_date').val();
                        metadata.IsChangeRequest = $('input[type=radio][name=chngreq]:checked').val() == 'Change_Request' ? 1 : 0;
                        metadata.ChangeOrderID = changeOrderId = $('#trend_change_order').val();
                        metadata.IsApprovedByClient = $('#chkTrendApprovalByClient:checked').val() == 'approval by client' ? 1 : 0; //Jignesh 31-12-2020
                        metadata.ClientApprovedDate = modal.find('.modal-body #client_approved_date').val(); // Jignesh-09-02-2021
                        $('#FutureTrendModal').modal('hide');
                        wbsTrendTree.trendGraph();
                    });
                }
                if (modal_mode == "Create") {
                    //API to get Maximum trend number - increment to get new trend number
                    //Parameterize JSON

                    //luan here - Find the trend status code id
                    var trendStatusCodeList = TRENDSTATUSCODELIST;
                    var trendStatusCodeID = null;
                    var selectedTrendStatusCode = modal.find('.modal-body #trend_status_code');
                    console.log(selectedTrendStatusCode, selectedTrendStatusCode.val());
                    for (var x = 0; x < trendStatusCodeList.length; x++) {
                        console.log(trendStatusCodeList[x].TrendStatusCodeName, selectedTrendStatusCode.val());
                        if (trendStatusCodeList[x].TrendStatusCodeName == selectedTrendStatusCode.val()) {
                            trendStatusCodeID = trendStatusCodeList[x].TrendStatusCodeID;
                        }
                    }

                    //luan here - Find the boolean value of internal commented by Jignesh 30-10-2020
                    //var selectedIsInternal = modal.find('.modal-body #trend_is_internal');
                    //var isInternal = null;
                    //if (selectedIsInternal[0].checked) {
                    //	isInternal = 1;
                    //} else {
                    //	isInternal = 0;
                    //}

                    //====================================== Created By Jignesh 30-10-2020 =======================================

                    var selectedIsChangeRequest = $('input[type=radio][name=chngreq]:checked').val();
                    var isChangeRequest = null;
                    var changeOrderId = null;
                    var isApprovedByClient = null; //Jignesh 31-12-2020

                    //Jignesh 31-12-2020
                    if (selectedIsChangeRequest == 'Change_Request') {
                        isChangeRequest = 1;
                        changeOrderId = $('#trend_change_order').val();
                        isApprovedByClient = $('#chkTrendApprovalByClient:checked').val();
                        if (isApprovedByClient) {
                            isApprovedByClient = 1;
                        }
                        else {
                            isApprovedByClient = 0;
                        }
                    }
                    else if (selectedIsChangeRequest == 'No_Change_Request') {
                        isChangeRequest = 0;
                        isApprovedByClient = 0;
                    }

                    //============================================================================================================

                    // Manasi
                    //var CostOverheadTypeID = null;
                    //var selectedCostOverheadType = modal.find('.modal-body #labor_rate_code');
                    //for (var x = 0; x < costOverheadTypes.length; x++) {
                    //    if (costOverheadTypes[x].CostOverHeadType == selectedCostOverheadType.val()) {
                    //        CostOverheadTypeID = costOverheadTypes[x].ID;
                    //    }
                    //}

                    var toUpdateTrend = {
                        "Operation": "1",
                        "ProjectID": _selectedProjectID,
                        "TrendNumber": modal.find('.modal-body #trend_number').html(),
                        "TrendDescription": modal.find('.modal-body #trend_description').val(),
                        "TrendStatusID": 3,
                        "TrendStatusCodeID": trendStatusCodeID,
                        "IsChangeRequest": isChangeRequest,
                        "ChangeOrderID": changeOrderId,
                        //"IsInternal": isInternal,
                        "IsApprovedByClient": isApprovedByClient,   //Jignesh 05-01-2021
                        "ClientApprovedDate": modal.find('.modal-body #client_approved_date').val(), // Jignesh-09-02-2021
                        "TrendJustification": modal.find('.modal-body #trend_justification').val(),
                        "TrendImpact": modal.find('.modal-body input[name="trend_impact"]:checked').val(),
                        "TrendImpactSchedule": modal.find('.modal-body input[name="trend_impact_schedule"]:checked').val(),   //Manasi 13-07-2020
                        "TrendImpactCostSchedule": modal.find('.modal-body input[name="trend_impact_cost_schedule"]:checked').val(),  //Manasi 13-07-2020
                        "ApprovalFrom": modal.find('.modal-body #approval_from').val(),
                        "ApprovalDate": modal.find('.modal-body #approval_date').val(),
                        "CreatedOn": getTodayAsString(),
                        "PreTrendStartDate": metadata.CurrentStartDate,
                        "PreTrendEndDate": metadata.CurrentEndDate,
                        "PostTrendStartDate": metadata.CurrentStartDate,
                        "PostTrendEndDate": metadata.CurrentEndDate,
                        "PreTrendCost": 0,
                        "PostTrendCost": 0,
                        //"CostOverheadTypeID" : CostOverheadTypeID  Manasi
                        "CostOverheadTypeID": 1
                        //"CurrentThreshold": "0",  //initial threshold - luan here approval process
                        //"CurrentApprover_EmployeeID": 10000 //tbd - luan here approval process
                    };
                    console.log(toUpdateTrend);
                    _Trend.persist().save(toUpdateTrend, function (response) {
                        isFieldValueChanged = false; // Jignesh-31-03-2021
                        $('#FutureTrendModal').modal('hide');
                        //wbsTrendTree.trendGraph();
                        wbsTrendTree.trendGraph(true);   //Manasi
                    });
                }
            });
            //================================= Jignesh-02-04-2021 ==============================================
            $('#update_pasttrend').unbind('click').on('click', function () {
                var TrendTileCheck = modal.find('.modal-body #pasttrend_description').val();
                if (TrendTileCheck == "" || TrendTileCheck.length == 0) {
                    dhtmlx.alert('Enter Trend Title.');
                    return false;
                }
                var pastTrendNumber = metadata.TrendNumber;
                if (modal_mode == 'Update') {
                    var trendStatusCodeList = TRENDSTATUSCODELIST;
                    var trendStatusCodeID = null;
                    var selectedTrendStatusCode = modal.find('.modal-body #pasttrend_status_code');
                    console.log(selectedTrendStatusCode, selectedTrendStatusCode.val());
                    for (var x = 0; x < trendStatusCodeList.length; x++) {
                        console.log(trendStatusCodeList[x].TrendStatusCodeName, selectedTrendStatusCode.val());
                        if (trendStatusCodeList[x].TrendStatusCodeName == selectedTrendStatusCode.val()) {
                            trendStatusCodeID = trendStatusCodeList[x].TrendStatusCodeID;
                        }
                    }

                    var selectedIsChangeRequest = $('input[type=radio][name=pastchngreq]:checked').val();
                    var isChangeRequest = null;
                    var changeOrderId = null;
                    var isApprovedByClient = null;

                    if (selectedIsChangeRequest == 'Change_Request') {
                        isChangeRequest = 1;
                        changeOrderId = $('#pasttrend_change_order').val();
                        isApprovedByClient = $('#chkPastTrendApprovalByClient:checked').val();
                        if (isApprovedByClient) {
                            isApprovedByClient = 1;
                        }
                        else {
                            isApprovedByClient = 0;
                        }
                    }
                    else if (selectedIsChangeRequest == 'No_Change_Request') {
                        isChangeRequest = 0;
                        isApprovedByClient = 0;
                    }

                    console.log(metadata);
                    var toUpdateTrend = {
                        "Operation": "5",   //update trend setup
                        "ProjectID": metadata.ProjectID,
                        "TrendNumber": metadata.TrendNumber,
                        "TrendDescription": modal.find('.modal-body #pasttrend_description').val(),
                        "TrendStatusID": 1,
                        "TrendStatusCodeID": trendStatusCodeID,
                        "IsChangeRequest": isChangeRequest,
                        "ChangeOrderID": changeOrderId,
                        //"IsInternal": isInternal,
                        "IsApprovedByClient": isApprovedByClient,  
                        "ClientApprovedDate": modal.find('.modal-body #pasttrend_client_approved_date').val(),
                        "TrendJustification": modal.find('.modal-body #pasttrend_justification').val(),
                        "TrendImpact": modal.find('.modal-body input[name="pasttrend_impact"]:checked').val(),
                        "TrendImpactSchedule": modal.find('.modal-body input[name="pasttrend_impact_schedule"]:checked').val(),  
                        "TrendImpactCostSchedule": modal.find('.modal-body input[name="pasttrend_impact_cost_schedule"]:checked').val(), 
                        "ApprovalFrom": modal.find('.modal-body #approval_from').val(),
                        "ApprovalDate": modal.find('.modal-body #pasttrend_approval_date').val(),
                        "CreatedOn": metadata.CreatedOn,
                        //"costOverheadTypeID" : costOverheadTypeID

                        "costOverheadTypeID": 1 
                    };

                    console.log(toUpdateTrend);
                    _Trend.persist().save(toUpdateTrend, function (response) {
                        isFieldValueChanged = false; 
                        metadata.TrendNumber = pastTrendNumber;
                        metadata.TrendJustification = modal.find('.modal-body #pasttrend_justification').val();
                        metadata.TrendImpact = modal.find('.modal-body input[name="pasttrend_impact"]:checked').val();
                        metadata.TrendImpactSchedule = modal.find('.modal-body input[name="pasttrend_impact_schedule"]:checked').val();   //Manasi 13-07-2020
                        metadata.TrendImpactCostSchedule = modal.find('.modal-body input[name="pasttrend_impact_cost_schedule"]:checked').val();    //Manasi 13-07-2020
                        metadata.ApprovalFrom = modal.find('.modal-body #approval_from').val();
                        metadata.ApprovalDate = modal.find('.modal-body #pasttrend_approval_date').val();
                        metadata.IsChangeRequest = $('input[type=radio][name=pastchngreq]:checked').val() == 'Change_Request' ? 1 : 0;
                        metadata.ChangeOrderID = changeOrderId = $('#pasttrend_change_order').val();
                        metadata.IsApprovedByClient = $('#chkPastTrendApprovalByClient:checked').val() == 'approval by client' ? 1 : 0; //Jignesh 31-12-2020
                        metadata.ClientApprovedDate = modal.find('.modal-body #pasttrend_client_approved_date').val(); // Jignesh-09-02-2021
                        $('#PastTrendModal').modal('hide');
                        wbsTrendTree.trendGraph();
                    });
                }
                
            });
            //===================================================================================================
            //============== Jignesh-31-03-2021 =================================================================
            var trendPageFieldIDs = '#trend_description,#trend_justification,input[type=radio][name=chngreq],input[type=checkbox][name=trend_impact],' +
                'input[type=checkbox][name=trend_impact_schedule],input[type=checkbox][name=trend_impact_cost_schedule],#chkTrendApprovalByClient,' +
                '#client_approved_date,#approval_date,#trend_status_code';

            $(trendPageFieldIDs).unbind().on('input change paste', function (e) {
                isFieldValueChanged = true;
            });
            $('#cancel_futuretrend,#btnCancelTrend_X').unbind('click').on('click', function () {
                if (isFieldValueChanged) {
                    dhtmlx.confirm("Unsaved data will be lost. Want to Continue?", function (result) {
                        if (result) {
                            $('#FutureTrendModal').modal('hide');
                            isFieldValueChanged = false;
                        }
                    });
                }
                else {
                    $('#FutureTrendModal').modal('hide');
                }
            });
            //====================================================================================================


            $('#approve_trend').unbind('click').on('click', function () {
                if ($('#approve_trend').html().trim() === 'Approve Trend') {
                    var today = new Date();
                    var dd = today.getDate();
                    var mm = today.getMonth() + 1; //January is 0!
                    var yyyy = today.getFullYear();
                    $('#approval_date').val(mm + "/" + dd + "/" + yyyy);
                    var toUpdateTrend = {
                        "Operation": "2",
                        "ProjectID": metadata.ProjectID,
                        "TrendNumber": metadata.TrendNumber,
                        "TrendDescription": metadata.TrendDescription,
                        "TrendStatusID": 1,
                        "ApprovalDate": modal.find('.modal-body #approval_date').val(),
                        "CreatedOn": metadata.CreatedOn
                    };
                    _Trend.persist().save(toUpdateTrend, function (response) {
                        $('#FutureTrendModal').modal('hide');
                        wbsTrendTree.trendGraph();
                    });
                }
                else {
                    $('#approve_trend').html("Requesting  <img src='/CPPv3/assets/js/select2/select2-spinner.gif'  />");
                    $('#approve_trend').prop('disabled', true);
                    _RequestApproval.get({ "UserID": _ApprovalThresholdInfo.userName, "Role": _ApprovalThresholdInfo.role, "TrendID": metadata.TrendNumber, "ProjectID": metadata.ProjectID }, function (response) {
                        $('#approve_trend').html('Requested');
                    });
                }
            });

            //====================================== Created By Jignesh 30-10-2020 =======================================

            $('input[type=radio][name=chngreq]').on('change', function () {
                if (this.value == 'Change_Request') {
                    $('#divChangeOrderDD').show();
                }
                else if (this.value == 'No_Change_Request') {
                    $('#divChangeOrderDD').hide();
                }
            });

            //============================================================================================================

            //============== Jignesh-02-04-2021 ======================================
            $('input[type=radio][name=pastchngreq]').on('change', function () {
                if (this.value == 'Change_Request') {
                    $('#divPastTrendChangeOrderDD').show();
                }
                else if (this.value == 'No_Change_Request') {
                    $('#divPastTrendChangeOrderDD').hide();
                }
            });
            $('#chkPastTrendApprovalByClient').click(function () {
                if ($(this).is(':checked'))
                    $('#secPastTrendApprovedDate').show();
                else
                    $('#secPastTrendApprovedDate').hide();
            });
            //========================================================================

            //================================== Jignesh-19-02-2021 ===============================
            $('#chkTrendApprovalByClient').click(function () {
                if ($(this).is(':checked'))
                    $('#secApprovedDate').show();
                else
                    $('#secApprovedDate').hide();
            });
            //=====================================================================================

            //====== Jignesh-22-02-2021 Please comment entire below section ===================
            //======================================= Jignesh-AddNewDocModal-18-02-2021 ==========================================
            //$('#document_type_trend').unbind().on('change', function (event) {
            //    if ($(this).val() == "Add New") {
            //        $('#addNewDocumentTypeModal').modal({ show: true, backdrop: 'static' });
            //        $('#txtDocType').val('');
            //        $('#txtDocDescription').val('');
            //    }
            //});
            //$('#btnSaveDocType').unbind('click').on('click', function ($files) {
            //    var docType = $('#txtDocType').val();
            //    var description = $('#txtDocDescription').val();
            //    if (docType == "" || docType.length == 0) {
            //        dhtmlx.alert('Enter Document Type.');
            //        return;
            //    }
            //    if (description == "" || description.length == 0) {
            //        dhtmlx.alert('Enter Description.');
            //        return;
            //    }
            //    var listToSave = [];
            //    var dataObj = {
            //        Operation: '1',
            //        DocumentTypeName: docType,
            //        DocumentTypeDescription: description
            //    }
            //    listToSave.push(dataObj);
            //    var url = serviceBasePath + 'response/DocumentType/';
            //    _httpProvider({
            //        url: url,
            //        method: "POST",
            //        data: JSON.stringify(listToSave),
            //        headers: { 'Content-Type': 'application/json' }
            //    }).then(function success(response) {
            //        response.data.result.replace(/[\r]/g, '\n');

            //        if (response.data.result) {
            //            _httpProvider({
            //                url: serviceBasePath + "Request/DocumentType",
            //                method: "GET"
            //            }).then(function success(response) {
            //                wbsTree.setDocTypeList(response.data.result);

            //                modal = $('#DocUpdateModalTrend');
            //                var docTypeDropDownProgram = modal.find('.modal-body #document_type_trend');

            //                var docTypeList = wbsTree.getDocTypeList();
            //                docTypeDropDownProgram.empty();
            //                docTypeDropDownProgram.append('<option value="Add New"> ----------Add New---------- </option>'); // Jignesh-AddNewDocModal-18-02-2021
            //                for (var x = 0; x < docTypeList.length; x++) {
            //                    if (docTypeList[x].DocumentTypeName == docType) {
            //                        docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '" selected> ' + docTypeList[x].DocumentTypeName + '</option>');
            //                    }
            //                    else {
            //                        docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '"> ' + docTypeList[x].DocumentTypeName + '</option>');
            //                    }
            //                }
            //            });
            //            $('#cancel_addNewDocumentTypeModal_x').trigger('click');
            //            dhtmlx.alert(response.data.result);
            //        } else {
            //            dhtmlx.alert('No changes to be saved.');
            //        }
            //        //$state.reload();
            //    }, function error(response) {
            //        dhtmlx.alert("Failed to save. Please contact your Administrator.");
            //    });
            //});
            //=======================================================================================================
            //====================================== Jignesh-TDM-06-01-2020 =======================================

            $('#DocUpdateModalTrend').unbind().on('show.bs.modal', function (event) {
                //$("#FutureTrendModal").css({ "opacity": "0.4" });
                $("#ExecutionDateTrend").datepicker();
                modal = $(this);
                //load docTypeList
                var docTypeDropDownProgram = modal.find('.modal-body #document_type_trend');
                var docTypeList = wbsTree.getDocTypeList();
                docTypeDropDownProgram.empty();
                // Jignesh-25-03-2021
                if (wbsTree.getLocalStorage().role === "Admin") {
                    docTypeDropDownProgram.append('<option value="Add New"> ----------Add New---------- </option>');
                }
                if (g_edittrenddocument == false) {
                    for (var x = 0; x < docTypeList.length; x++) {
                        docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '"> ' + docTypeList[x].DocumentTypeName + '</option>');
                    }
                    docTypeDropDownProgram.val('');

                }
                else {
                    for (var x = 0; x < docTypeList.length; x++) {
                        if (docTypeList[x].DocumentTypeID == g_document_type_trend) {
                            docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '" selected> ' + docTypeList[x].DocumentTypeName + '</option>');
                            docTypeDropDownProgram.val(docTypeList[x].DocumentTypeID);
                        } else {
                            docTypeDropDownProgram.append('<option value="' + docTypeList[x].DocumentTypeID + '"> ' + docTypeList[x].DocumentTypeName + '</option>');
                        }
                    }
                }
                //docTypeDropDownProgram.val('');
                //$('#DocSpecialNoteTrend').val('');  //Manasi
                //$('#ExecutionDateTrend').val('');  //Manasi
                //$('#fileUploadTrend').val('');  //Manasi

            });
            $('#updateDMBtnTrend').unbind().on('click', function (event) {
                g_edittrenddocument = false;
                if (wbsTree.getIsProgramNew()) {
                    dhtmlx.alert('Uploading files only work in edit mode.');
                    return;
                }
                else {

                    $('#fileUploadTrend').val('');
                    $('#DocSpecialNoteTrend').val(''); 
                    $('#document_name_trend').val('');
                    $('#document_type_trend').val('');
                    $('#DocUpdateModalTrend').modal({ show: true, backdrop: 'static' });
                    
                }
            });
            $('#fileUploadTrend').change(function (ev) {
                console.log(fileUploadProject.files);
                $("#document_name_trend").val(fileUploadTrend.files[0].name);
            });

            //============================================================================================================
        }

        var getTodayAsString = function () {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var todayAsStr = yyyy + '-' + mm + '-' + dd;
            return todayAsStr;
        }
    };
    return obj;
}(jQuery));

