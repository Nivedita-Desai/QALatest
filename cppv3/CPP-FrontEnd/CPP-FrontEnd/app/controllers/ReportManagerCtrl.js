angular.module('cpp.controllers').
    controller('ReportManagerCtrl', ['$scope', '$timeout', '$uibModal', '$rootScope', '$http', 'ProgramElement',
        function ($scope, $timeout, $uibModal, $rootScope, $http, ProgramElement) {
            
            $('.modal-backdrop').hide();

            var sqlDateFormat = "YYYY-MM-DD";

            (function () {
                if (window.history && window.history.pushState) {
                    $(window).on('popstate', function () {
                        $scope.$close('close');
                    });
                }
            })();

            //List of the 3 granularities
            $scope.granularityList = [
                "week",
                "month",
                "year"
            ]

            //List of selectable report types
            $scope.reportTypeList = [
				"Budget Planning Report",	//SSRS
                "Budget Summary Report",
                "Labor Management Report", //Manasi - new name for FTE Report
                "Material Report", //SSRS
                "ODC Report", //SSRS
                "Subcontractor Report", //SSRS
                "Budget vs. Actual Summary Report"
            ];

            if ($scope.params.TrendNumber == 1000) {

                $scope.reportTypeList = [

                    "Budget Planning Report",	//SSRS
                    "Budget Summary Report",
                    "Labor Management Report", //Manasi - new name for FTE Report
                    "Material Report", //SSRS
                    "ODC Report", //SSRS
                    "Subcontractor Report", //SSRS
                    "Budget vs. Actual Summary Report"

                ];

            } else if ($scope.params.TrendNumber == 2000 || $scope.params.TrendNumber == 3000) {

                $scope.reportTypeList = [
                  
                ];

            }
            else {
                $scope.reportTypeList = [

                    "Budget Planning Report",	//SSRS
                    "Budget Summary Report", //SSRS
                    
                   
                ];
            }

            //Default select of report type
           $scope.reportTypeFilter = $scope.reportTypeList[0];

            //Default granularity pick
            $scope.granularityFilter = $scope.granularityList[0];

            //When a report type is selected
            $scope.selectReportType = function (reportType) {
                console.log(reportType);
                if ((reportType == 'Budget vs. Actual Summary Report' || reportType == 'Labor Management Report' || reportType == 'Material Report'
                    || reportType == 'ODC Report' || reportType == 'Subcontractor Report') && $scope.params.TrendNumber != 1000) {
                    //dhtmlx.alert(reportType + ' is only available in the current view. Please select another.');
                   // $scope.reportTypeFilter = reportType;
                    //return false;
                } else {
                    $scope.reportTypeFilter = reportType;
                    return true;
                }
                $scope.reportTypeFilter = reportType;
            }

            //Select granularity
            $scope.selectGranularity = function (granularity) {
                $scope.granularityFilter = granularity;
            }

            //When clicked on back button or X
            $scope.goBack = function (param) {
                $scope.$close(param);
            }

            //When clicked on run button
            $scope.generateReport = function () {
                //alert($scope.reportTypeFilter);
                //alert(!$scope.selectReportType($scope.reportTypeFilter));
                //if (!$scope.selectReportType($scope.reportTypeFilter)) {
                //    return;
                //}
                // Var TrendNumber = $scope.params.TrendNumber;
                $('#ganttReportRun').prop('disabled', true); //Aditya
                $('#ganttReportClose').prop('disabled', true); //Aditya
                var reportType = $scope.reportTypeFilter;
                if ((reportType == 'Budget vs. Actual Summary Report' || reportType == 'Labor Management Report' || reportType == 'Material Report'
                    || reportType == 'ODC Report' || reportType == 'Subcontractor Report') && $scope.params.TrendNumber != 1000) {
                    //------------------------------------- Swapnil 30/11/2020 --------------------------------------
                    //dhtmlx.alert(reportType + ' is only available in the current view. Please select another.');
                    dhtmlx.alert(reportType + ' is only available in the Budget vs Actual view.'); //Please select another report.');   //Manasi 03-03-2021
                    //--------------------------------------------------------------------------------------------------
                    return false;
                }
                //Manasi 26-07-2021
                else if ((reportType == 'Budget Planning Report' || reportType == 'Budget Summary Report')
                    && ($scope.params.TrendNumber == 2000 || $scope.params.TrendNumber == 3000)) {
                    dhtmlx.alert(reportType + ' is available in the Baseline and Budget vs Actual view.');
                    return false;
                }

                if ($scope.reportTypeFilter == 'Material Report') {            //Material Report - MySQL
                    var baseUrl = null;

                    var todayDate = moment().format(sqlDateFormat);


                    baseUrl = serviceBasePath + 'Request/MaterialReport';

                    var url = baseUrl
                      + '?projectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                      + '?projectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'excel';

                    var fileName = generateFileName('Material Report');

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        //Declare parameters for pdf viewer modal
                        scope.params = {
                            content: response.data,
                            excelUrl: excelUrl,
                            baseUrl: baseUrl,
                            fileName: fileName,
                            contentType: 'base64',
                            excelDownloadable: true,
                            pdfDownloadable: true
                        };

                        //Open pdf viewer modal
                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'Budget Summary Report') {            //Budget Summary Report - MySQL
                    var baseUrl = null;

                    var todayDate = moment().format(sqlDateFormat);


                    baseUrl = serviceBasePath + 'Request/BudgetSummaryReport';

                    var url = baseUrl
                        + '?ProjectID=' + $scope.params.ProjectID
                        + '&Granularity=' + $scope.granularityFilter
                        + '&FileType=' + 'PDF'
                        + '&TrendNumber=' + $scope.params.TrendNumber;    //Manasi 20-07-2020

                    console.log(url);

                    var excelUrl = baseUrl
                      + '?ProjectID=' + $scope.params.ProjectID
                      + '&Granularity=' + $scope.granularityFilter
                        + '&FileType=' + 'excel'
                        + '&TrendNumber=' + $scope.params.TrendNumber;    //Manasi 20-07-2020;

                    var fileName = generateFileName('Budget Summary Report');

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        //Declare parameters for pdf viewer modal
                        scope.params = {
                            content: response.data,
                            excelUrl: excelUrl,
                            baseUrl: baseUrl,
                            fileName: fileName,
                            contentType: 'base64',
                            excelDownloadable: true,
                            pdfDownloadable: true
                        };

                        //Open pdf viewer modal
                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'Labor Management Report') {            //FTE Report - MySQL
                    var baseUrl = null;

                    var todayDate = moment().format(sqlDateFormat);


                    baseUrl = serviceBasePath + 'Request/FTEReport';

                    var url = baseUrl
                      + '?projectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                      + '?projectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'excel';

                    var fileName = generateFileName('Labor Management Report');

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        //Declare parameters for pdf viewer modal
                        scope.params = {
                            content: response.data,
                            excelUrl: excelUrl,
                            baseUrl: baseUrl,
                            fileName: fileName,
                            contentType: 'base64',
                            excelDownloadable: true,
                            pdfDownloadable: true
                        };

                        //Open pdf viewer modal
                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'ODC Report') {            //ODC Report - MySQL
                    var baseUrl = null;

                    var todayDate = moment().format(sqlDateFormat);


                    baseUrl = serviceBasePath + 'Request/ODCReport';

                    var url = baseUrl
                      + '?projectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                      + '?projectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'excel';

                    var fileName = generateFileName('ODC Report');

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        //Declare parameters for pdf viewer modal
                        scope.params = {
                            content: response.data,
                            excelUrl: excelUrl,
                            baseUrl: baseUrl,
                            fileName: fileName,
                            contentType: 'base64',
                            excelDownloadable: true,
                            pdfDownloadable: true
                        };

                        //Open pdf viewer modal
                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'Subcontractor Report') {            //Subcontractor Report - MySQL
                  //  alert($scope.params.TrendNumber);
                    //if ($scope.params.IsTrendApproved != true) {
                    //    dhtmlx.alert('You can only view this report only when baseline is approved.');
                    //    return false;
                    //}
                    var baseUrl = null;

                    var todayDate = moment().format(sqlDateFormat);


                    baseUrl = serviceBasePath + 'Request/SubcontractorReport';

                    var url = baseUrl
                        + '?projectID=' + $scope.params.ProjectID
                        + '&TrendNumber=' + $scope.params.TrendNumber
                      + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                        + '?projectID=' + $scope.params.ProjectID
                        + '&TrendNumber=' + $scope.params.TrendNumber
                      + '&FileType=' + 'excel';

                    var fileName = generateFileName('Subcontractor Report');

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        //Declare parameters for pdf viewer modal
                        scope.params = {
                            content: response.data,
                            excelUrl: excelUrl,
                            baseUrl: baseUrl,
                            fileName: fileName,
                            contentType: 'base64',
                            excelDownloadable: true,
                            pdfDownloadable: true
                        };

                        //Open pdf viewer modal
                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'Budget vs. Actual Summary Report') {            //Summary Report - MySQL
                    var baseUrl = null;

                    var todayDate = moment().format(sqlDateFormat);


                    baseUrl = serviceBasePath + 'Request/SummaryReport';

                    var url = baseUrl
                      + '?ProjectID=' + $scope.params.ProjectID
                      + '&StartDate=' + todayDate    //hard code testing
                      + '&FileType=' + 'PDF';

                    var excelUrl = baseUrl
                      + '?ProjectID=' + $scope.params.ProjectID
                      + '&StartDate=' + todayDate    //hard code testing
                      + '&FileType=' + 'excel';

                    var fileName = generateFileName('Budget vs Actual Summary Report');  //Manasi 02-03-2021

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        //Declare parameters for pdf viewer modal
                        scope.params = {
                            content: response.data,
                            excelUrl: excelUrl,
                            baseUrl: baseUrl,
                            fileName: fileName,
                            contentType: 'base64',
                            excelDownloadable: true,
                            pdfDownloadable: true
                        };

                        //Open pdf viewer modal
                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'Budget Planning Report') {            //Budget Planning Report - MySQL
                	var baseUrl = null;

                	var todayDate = moment().format(sqlDateFormat);


                	baseUrl = serviceBasePath + 'Request/BudgetPlanningReport';

                	var url = baseUrl
                        + '?ProjectID=' + $scope.params.ProjectID
                        + '&TrendNumber=' + $scope.params.TrendNumber
                      + '&FileType=' + 'PDF';

                	var excelUrl = baseUrl
                        + '?ProjectID=' + $scope.params.ProjectID
                        + '&TrendNumber=' + $scope.params.TrendNumber
                      + '&FileType=' + 'excel';

                	var fileName = generateFileName('Budget Planning Report');

                	console.log({ url: url });

                	$http.get(url).then(function success(response) {
                		console.log(response);

                		var scope = $rootScope.$new();

                		//Declare parameters for pdf viewer modal
                		scope.params = {
                			content: response.data,
                			excelUrl: excelUrl,
                			baseUrl: baseUrl,
                			fileName: fileName,
                			contentType: 'base64',
                			excelDownloadable: true,
                			pdfDownloadable: true
                		};

                		//Open pdf viewer modal
                		openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                	}, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                	});
                }
                else if ($scope.reportTypeFilter == 'Budget Summary Report SDA') {            //Budget Summary Report SDA - MySQL
                	var baseUrl = null;

                	var todayDate = moment().format(sqlDateFormat);


                	baseUrl = serviceBasePath + 'Request/BudgetSummaryCPP1Report';

                	var url = baseUrl
                      + '?ProjectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'PDF';

                	var excelUrl = baseUrl
                      + '?ProjectID=' + $scope.params.ProjectID
                      + '&FileType=' + 'excel';

                	var fileName = generateFileName('Budget Summary Report SDA');

                	console.log({ url: url });

                	$http.get(url).then(function success(response) {
                		console.log(response);

                		var scope = $rootScope.$new();

                		//Declare parameters for pdf viewer modal
                		scope.params = {
                			content: response.data,
                			excelUrl: excelUrl,
                			baseUrl: baseUrl,
                			fileName: fileName,
                			contentType: 'base64',
                			excelDownloadable: true,
                			pdfDownloadable: true
                		};

                		//Open pdf viewer modal
                		openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                	}, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                	});
                }
                else if ($scope.reportTypeFilter == 'Budget' ||           //Budget - Tableau
                    $scope.reportTypeFilter == 'Project Forecast' ||        //Project Forecast - Tableau
                    $scope.reportTypeFilter == 'Project Trend Report') {    //Project Trend Report - Tableau
                    var url = null;
                    var reportType = '';

                    var ProjectId = $scope.params.ProjectID;
                    var TrendNumber = $scope.params.TrendNumber;

                    if ($scope.reportTypeFilter == 'Budget') {
                        reportType = 'FACSum';
                    }
                    if ($scope.reportTypeFilter == 'Project Forecast') {
                        reportType = 'ForecastTrend';
                    }
                    if ($scope.reportTypeFilter == 'Project Trend Report') {
                        reportType = 'AllTrend';
                    }

                    url = serviceBasePath + 'response/report/' + ProjectId + '/' + TrendNumber + '/' + reportType;

                    var fileName = generateFileName($scope.reportTypeFilter);

                    console.log({ url: url });

                    $http.get(url, { responseType: 'arraybuffer' }).then(function success(response) {
                        console.log(response);

                        var scope = $rootScope.$new();

                        var blobtest = new Blob([response.data], {
                            type: 'application/pdf'
                        });

                        console.log(blobtest);

                        scope.params = {
                            content: blobtest,
                            fileName: fileName,
                            contentType: 'blob',
                            excelDownloadable: false,
                            pdfDownloadable: true
                        };

                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
                else if ($scope.reportTypeFilter == 'Budgeted vs Actual vs ETC Chart') {     //Budgeted vs Actual vs ETC Chart - Tableau
                    var url = null;
                    var reportType = 'FTERT';

                    var ProjectId = $scope.params.ProjectID;

                    url = serviceBasePath + 'response/report/' + ProjectId + '/' + reportType;

                    var fileName = generateFileName($scope.reportTypeFilter);

                    console.log({ url: url });

                    $http.get(url).then(function success(response) {
                        console.log(response);
                        var scope = $rootScope.$new();

                        scope.params = {
                            content: response.data.result,
                            fileName: fileName,
                            contentType: 'url',
                            excelDownloadable: false,
                            pdfDownloadable: false
                        };

                        openPDFViewerModal(scope);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    }, function error(response) {
                        console.log(response);
                        $('#ganttReportRun').prop('disabled', false); //Aditya
                        $('#ganttReportClose').prop('disabled', false); //Aditya
                    });
                }
            }

            //Generate file name for download based on the report type
            function generateFileName(reportType) {
                var fileName = reportType;
                $('#ganttReportRun').prop('disabled', true); //Aditya
                $('#ganttReportClose').prop('disabled', true); //Aditya
                $scope.filedateformat = "DDMMYY_hhmm";  //Manasi 28-07-2020
                $scope.filedateformat = moment().format($scope.filedateformat);   //Manasi 28-07-2020
                fileName = fileName + '_' + $scope.filedateformat;  //Manasi 28-07-2020

                if (reportType == 'Testing SSRS') {
                    if ($scope.ReportParameter1) {
                        fileName += '_' + $scope.ReportParameter1;
                    }

                    if (!$scope.ReportParameter1) {
                        fileName += '_No Filter';
                    }
                }

                return fileName;
            }

            //Open the pdf viewer modal to view the content
            function openPDFViewerModal(scope) {
                $rootScope.modalInstance = $uibModal.open({
                    backdrop: 'static',
                    keyboard: false,
                    scope: scope,
                    templateUrl: "app/views/modal/PDFViewerModal.html",
                    size: "md",
                    controller: "PDFViewerCtrl"
                });
            }
        }
    ]);