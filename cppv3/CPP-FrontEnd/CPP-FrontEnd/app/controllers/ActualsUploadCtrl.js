angular.module('cpp.controllers').
    controller('ActualsUploadCtrl', ['$http', 'Page', '$state', 'UpdateDocumentType', '$uibModal', 'DocumentType', '$scope', '$rootScope', 'ProjectTitle', 'TrendStatus', '$location',
    function ($http, Page, $state, UpdateDocumentType, $uibModal, DocumentType, $scope, $rootScope, ProjectTitle, TrendStatus, $location) {
        var okToExit = true;
        Page.setTitle('Upload Actuals');
        ProjectTitle.setTitle('');
        TrendStatus.setStatus('');
        $scope.checkedRow = [];

        $scope.gridOPtions = {};
        $scope.myExternalScope = $scope;

        var formdata = new FormData();

        // Upload files for actual costs.
        $scope.uploadFilesCosts = function () {
            document.getElementById("loading").style.display = "block"; //Manasi 01-04-2021
            var request = {
                method: 'POST',
                url: serviceBasePath + 'actualsUploadFiles/Post',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                },
                ignore: true
            };

            // Alert for missing files
            var input = document.forms["actualsForm"]["actualFileUpload"];
            if (input.value == null || input.value == "")
            {
                dhtmlx.alert("No file selected");
                return;
            }

            // SEND THE FILES.
            $http(request)
                .then(function success(response) {
                    document.getElementById("loading").style.display = "none";  //Manasi 01-04-2021
                    displayFilesCosts(response.data);
                }, function error(response) {
                    document.getElementById("loading").style.display = "none";  //Manasi 01-04-2021
                }).finally(function () {

                    // Clear selected files
                    formdata = new FormData();
                    $('#uploadBtnCosts').prop('disabled', false);
                    document.getElementById("loading").style.display = "none";  //Manasi 01-04-2021
                });
               
        };

        // Get files for timesheet selection.
        $scope.getTheFilesCosts = function ($files) {
            console.log('get files', $files);
            $scope.files = $files;
            $('#uploadBtnTimesheet').prop('disabled', true);
            angular.forEach($files, function (value, key) {
                formdata.append(key, value);
                $('#uploadBtnTimesheet').prop('disabled', false);
            });
        };

        // Go through each file and add the correct responses to the table.
        function displayFilesCosts(response)
        {
            var alertResponse = "Files uploaded";
            var toAdd = [];

            // Go through upload responses and add a line for in the format {text, class}
            for (resulti = 0; resulti < response.result.length; resulti++) {
                var currentResponse = response.result[resulti];

                var color = "greenFileName";
                if (currentResponse.response == currentResponse.duplicate) {
                    color = "blueFileName";
                    alertResponse = "Files uploaded (excluding duplicates)";
                }
                else if (currentResponse.response == currentResponse.error) {
                    color = "redFileName";
                    alertResponse = "Files uploaded (excluding errors and duplicates)";
                }

                // Add the file name
                toAdd.push({ "text": currentResponse.fileName, "class": color });
                
                // Add every actual line
                for (linei = 0; linei < currentResponse.actualLinesThatNeedResponse.length; linei++) {
                    var currentLine = currentResponse.actualLinesThatNeedResponse[linei];

                    var color = "green";
                    if (currentLine.Response == currentLine.duplicate) {
                        color = "blue";
                    }
                    else if (currentLine.Response == currentLine.formatError || currentLine.Response == currentLine.nonExistentCost) {
                        color = "red";
                    }
                    toAdd.push({ "text": currentLine["ResponseText"], "class": color });
                }
            }

            $scope.responsesCosts = toAdd;

            // Alert user
            dhtmlx.alert(alertResponse);

        }

        // Unpack the response and return a string to display
        function unpackUploadResponseCosts(responses)
        {
            var toReturn = "";
            // Find the space that delimits the successes and errors
            for (var responseI = 0; responseI < responses.length; responseI++) {
                var currentResponse = responses[responseI];
                var spaceIndex = 0;
                for (var charI = 0; charI < currentResponse.length; charI++) {
                    var currentChar = responses[responseI].charAt(charI);
                    if (currentChar == ' ') {
                        console.log(currentChar);
                        spaceIndex = charI;
                    }
                }

                var suc = currentResponse.slice(0, spaceIndex);
                var err = currentResponse.slice(spaceIndex + 1);

                toReturn += suc + " successful rows and " + err + " errors";
            }

            return toReturn;
        }




        // Upload files for timesheet hours.
        $scope.uploadFilesTimesheet = function () {
            $('#uploadBtnTimesheet').prop('disabled', true);
            var request = {
                method: 'POST',
                url: serviceBasePath + 'timesheetUploadFiles/Post/',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };

            // Alert for missing files
            var input = document.forms["timesheetForm"]["timesheetFileUpload"];
            if (input.value == null || input.value == "") {
                dhtmlx.alert("No file selected");
                return;
            }

            // SEND THE FILES.
            $http(request).then(function success(response) {
                dhtmlx.alert("Files uploaded (TODO: display errors)");

                displayFilesTimesheet(response.data);
            }, function error(response) {

            }).finally(function(){
                // Clear selected files
                fileUpload.value = "";
                formdata = new FormData();
                $('#uploadBtnTimesheet').prop('disabled', false);   
            });
               
        };

        // Get files for costs selection.
        $scope.getTheFilesTimesheet = function ($files) {
            console.log('get files', $files);
            $('#uploadBtnTimesheet').prop('disabled', true);
            $scope.files = $files;
            angular.forEach($files, function (value, key) {
                formdata.append(key, value);
                $('#uploadBtnTimesheet').prop('disabled', false);
            });
        };

        // Go through each file and append it to the div that has id=uploadResponse
        function displayFilesTimesheet(response) {

            var toAdd = [];

            // Go through upload responses and add a line for in the format {text, class}
            for (resulti = 0; resulti < response.result.length; resulti++) {
                var currentResponse = response.result[resulti];

                var color = "greenFileName";
                if (currentResponse.response == currentResponse.duplicate) {
                    color = "blueFileName";
                }
                else if (currentResponse.response == currentResponse.error) {
                    color = "redFileName";
                }

                // Add the file name
                toAdd.push({ "text": currentResponse.fileName, "class": color });

                // Add every actual line
                for (linei = 0; linei < currentResponse.actualLinesThatNeedResponse.length; linei++)
                {
                    var currentLine = currentResponse.actualLinesThatNeedResponse[linei];

                    var color = "green";
                    if (currentLine.Response == currentLine.duplicate) {
                        color = "blue";
                    }
                    else if (currentLine.Response == currentLine.formatError || currentLine.Response == currentLine.nonExistentCost) {
                        color = "red";
                    }
                    toAdd.push({ "text": currentLine["ResponseText"], "class": color });
                }
            }

            $scope.responsesTimesheet = toAdd;
        }
    }]);
