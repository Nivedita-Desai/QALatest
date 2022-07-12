angular.module('cpp.controllers').
    controller('ChartCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', 'Program', 'ProgramElement', 'Project', 'Organization', 'Page',
        function ($scope, $timeout, $uibModal, $rootScope, $http, Program, ProgramElement, Project, Organization, Page) {

            Page.setTitle('Budget vs Actual vs Forecast Metrics Graph');
            
            $('.modal-backdrop').hide();

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            //initializations
            $scope.projectList = "";

            initializeFilterLists();
            //Select programelementid

            //window.onloadend = function () {
            //    var project = document.getElementById('selectProject');
            //    project.disabled = false;
            //};

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            //Call to initialize all lists for the filters, making backend calls
            function initializeFilterLists() {
                var pgmId = "";
                var pgmEltId = "";
                Project.lookup().get({ ProgramID: pgmId, ProgramElementID: pgmEltId }, function (projectData) {
                    $scope.projectList = projectData.result;
                });

            }

            $scope.showDetails = function (projectId) {
                var request = {
                    method: 'GET',
                    url: serviceBasePath + 'Request/GetGraph/' + projectId,
                    data: '', //fileUploadProject.files, //$scope.
                    ignore: true,
                    headers: {
                        'Content-Type': undefined
                    }
                };
                $http(request).then(function success(response) {
                    $scope.data = response.data;
                    //var innerArr = response.data.chartData;

                    //google.charts.load('current', { packages: ['corechart', 'line'] });
                    //google.charts.setOnLoadCallback(drawCurveTypes);

                    //function drawCurveTypes() {
                    //    var data = new google.visualization.DataTable();
                    //    data.addColumn('string', 'X');
                    //    data.addColumn('number', 'TotalBudget');
                    //    data.addColumn('number', 'ATD');
                    //    data.addColumn('number', 'BTD');
                    //    data.addColumn('number', 'BTC');
                    //    data.addColumn('number', 'EAC');
                    //    data.addColumn('number', 'FTC');

                    //    data.addRows(innerArr);

                    //    var options = {
                    //        hAxis: {
                    //            title: 'Date',
                    //            logScale: true
                    //        },
                    //        vAxis: {
                    //            title: '$ Amount',
                    //            logScale: false
                    //        },
                    //        colors: ['#a52714', '#097138']
                    //    };

                    //    var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
                    //    chart.draw(data, options);
                    //}
                    //for (var i = 0; i < innerArr.length; i++) {
                    //    OuterArr.push(innerArr[i].chartData);
                    //}
                    //for (var j = 0; j < OuterArr.length; j++) {
                    //    finalArr.push(OuterArr[j]);
                    //}


                    //console.log(innerArr);
                    //$scope.lineChartYData = data.yData;
                    //$scope.lineChartXData = data.xData;

                    //var canvas = document.getElementById("barChart");
                    //var ctx = canvas.getContext('2d');

                    var ctx = document.getElementById("barChart").getContext("2d");
                    
                    
                    // Global Options:
                    //Chart.defaults.global.defaultFontColor = 'black';
                    //Chart.defaults.global.defaultFontSize = 16;
                    var graphData = $scope.data;
                    var data = {
                        "labels": graphData.labels,
                        "datasets": graphData.datasets
                    };

                    //var customTooltips = function (tooltip) {
                    //    // Tooltip Element
                    //    var tooltipEl = document.getElementById('chartjs-tooltip');

                    //    //if (!tooltipEl) {
                    //        //tooltipEl = document.createElement('div');
                    //        //tooltipEl.id = 'chartjs-tooltip';
                    //        //tooltipEl.innerHTML = '<table></table>';
                    //        this._chart.canvas.parentNode.appendChild(tooltipEl);
                    //    //}

                    //    // Hide if no tooltip
                    //    if (tooltip.opacity === 0) {
                    //        tooltipEl.style.opacity = 0;
                    //        return;
                    //    }

                    //    // Set caret Position
                    //    tooltipEl.classList.remove('above', 'below', 'no-transform');
                    //    if (tooltip.yAlign) {
                    //        tooltipEl.classList.add(tooltip.yAlign);
                    //    } else {
                    //        tooltipEl.classList.add('no-transform');
                    //    }

                    //    function getBody(bodyItem) {
                    //        return bodyItem.lines;
                    //    }

                    //    // Set Text
                    //    if (tooltip.body) {
                    //        var titleLines = tooltip.title || [];
                    //        var bodyLines = tooltip.body.map(getBody);

                    //        var innerHtml = '<thead>';

                    //        titleLines.forEach(function (title) {
                    //            innerHtml += '<tr><th>' + title + '</th></tr>';
                    //        });
                    //        innerHtml += '</thead><tbody>';

                    //        bodyLines.forEach(function (body, i) {
                    //            var colors = tooltip.labelColors[i];
                    //            var style = 'background:' + colors.backgroundColor;
                    //            style += '; border-color:' + colors.borderColor;
                    //            style += '; border-width: 2px';
                    //            var span = '<span class="chartjs-tooltip-key" style="' + style + '"></span>';
                    //            innerHtml += '<tr><td>' + span + body + '</td></tr>';
                    //        });
                    //        innerHtml += '</tbody>';

                    //        var tableRoot = tooltipEl.querySelector('table');
                    //        tableRoot.innerHTML = innerHtml;
                    //    }

                    //    var positionY = this._chart.canvas.offsetTop;
                    //    var positionX = this._chart.canvas.offsetLeft;

                    //    // Display, position, and set styles for font
                    //    tooltipEl.style.opacity = 1;
                    //    tooltipEl.style.left = positionX + tooltip.caretX + 'px';
                    //    tooltipEl.style.top = positionY + tooltip.caretY + 'px';
                    //    tooltipEl.style.fontFamily = tooltip._bodyFontFamily;
                    //    tooltipEl.style.fontSize = tooltip.bodyFontSize + 'px';
                    //    tooltipEl.style.fontStyle = tooltip._bodyFontStyle;
                    //    tooltipEl.style.padding = tooltip.yPadding + 'px ' + tooltip.xPadding + 'px';
                    //};

                    // Notice the scaleLabel at the same level as Ticks
                    //var options = {
                    //    scales: {
                    //        yAxes: [{
                    //            ticks: {
                    //                beginAtZero: true
                    //            },
                    //            scaleLabel: {
                    //                display: true,
                    //                labelString: '$ Amount',
                    //                fontSize: 20
                    //            }
                    //        }]
                    //    }
                    //    //,
                    //    //tooltips: {
                    //    //    enabled: false,
                    //    //    mode: 'index',
                    //    //    position: 'nearest',
                    //    //    custom: customTooltips
                    //    //}
                    //};

                    // Chart declaration:
                    //var mybarChart = new Chart(ctx, {
                    //    type: 'line',
                    //    data: data,
                    //    options: {
                    //        scales: {
                    //            yAxes: [{
                    //                ticks: {
                    //                    beginAtZero: true
                    //                },
                    //                scaleLabel: {
                    //                    display: true,
                    //                    labelString: '$ Amount',
                    //                    fontSize: 20
                    //                }
                    //            }]
                    //        },
                    //        plugins: [{
                    //            afterDatasetsDraw: function (mybarChart) {
                    //                debugger
                    //                var ctx1 = mybarChart.ctx;
                    //                mybarChart.data.datasets.forEach(function (dataset, index) {
                    //                    var datasetMeta = mybarChart.getDatasetMeta(index);
                    //                    if (datasetMeta.hidden) return;
                    //                    datasetMeta.data.forEach(function (point, index) {
                    //                        var value = dataset.data[index],
                    //                            x = point.getCenterPoint().x,
                    //                            y = point.getCenterPoint().y,
                    //                            radius = point._model.radius,
                    //                            fontSize = 14,
                    //                            fontFamily = 'Verdana',
                    //                            fontColor = 'black',
                    //                            fontStyle = 'normal';
                    //                        ctx1.save();
                    //                        ctx1.textBaseline = 'middle';
                    //                        ctx1.textAlign = 'center';
                    //                        ctx1.font = fontStyle + ' ' + fontSize + 'px' + ' ' + fontFamily;
                    //                        ctx1.fillStyle = fontColor;
                    //                        ctx1.fillText(value, x, y - radius - fontSize);
                    //                        ctx1.restore();
                    //                    });
                    //                });
                    //            }
                    //        }]
                    //    }
                    //});

                    var chart = new Chart(ctx, {
                        type: 'line',
                        data: data,
                        options: {
                            scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true
                                        //max: 7
                                    }
                                }]
                            }
                        },
                        plugins: [{
                            afterDatasetsDraw: function (chart) {
                                var ctx = chart.ctx;
                                chart.data.datasets.forEach(function (dataset, index) {
                                    var datasetMeta = chart.getDatasetMeta(index);
                                    if (datasetMeta.hidden) return;
                                    datasetMeta.data.forEach(function (point, index) {
                                        var value = dataset.data[index],
                                            x = point.getCenterPoint().x,
                                            y = point.getCenterPoint().y,
                                            radius = point._model.radius,
                                            fontSize = 14,
                                            fontFamily = 'Verdana',
                                            fontColor = 'black',
                                            fontStyle = 'normal';
                                        ctx.save();
                                        ctx.textBaseline = 'middle';
                                        ctx.textAlign = 'center';
                                        ctx.font = fontStyle + ' ' + fontSize + 'px' + ' ' + fontFamily;
                                        ctx.fillStyle = fontColor;
                                        //ctx.fillText(value, x, y - radius - fontSize);   //Displays the amount on the graph
                                        ctx.restore();
                                    });
                                });
                            }
                        }]
                    });

                    //window.myLine = new Chart(ctx, {
                    //    type: 'line',
                    //    //responsive: true,
                    //    //showTooltips: true,
                    //    //multiTooltipTemplate: "<%= value %>",
                    //    data: data,
                    //    options: options
                    //});

                    //window.myLine = new Chart(ctx).Line(data, {
                    //    responsive: true,
                    //    showTooltips: true,
                    //    multiTooltipTemplate: "<%= value %>",
                    //    options: options,
                    //});

                });
            }
        }
    ]);