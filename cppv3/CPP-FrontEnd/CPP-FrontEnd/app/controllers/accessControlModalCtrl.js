angular.module('cpp.controllers').    
    controller('AccessControlModalCtrl',['$scope','$rootScope','$uibModal','$http',
    function($scope,$rootScope,$uibModal,$http) {
        console.log($scope.params);

        var url = serviceBasePath+'response/role/';
         //   var url = 'http://localhost:29986/api/response/role/';
       // alert();
        $scope.ACItem= angular.copy($scope.params.ACItem);
            console.log($scope.params);

        if($scope.params.newOrEdit === 'new'){
            $scope.ACItem = null;
        }else{
            $scope.ACItem = angular.copy($scope.params.ACItem);
        }
        var dummyData ="";
        function copyData(){
                ($scope.ACItem.AccessControlList[0]==='1') ? $scope.viewProgram = true : $scope.viewProgram = false;
                ($scope.ACItem.AccessControlList[1]==='1') ? $scope.modifyProgram = true: $scope.modifyProgram = false ;
                ($scope.ACItem.AccessControlList[2]==='1') ? $scope.viewProgramElement = true : $scope.viewProgramElement = false ;
                ($scope.ACItem.AccessControlList[3]==='1') ? $scope.modifyProgramElement = true : $scope.modifyProgramElement = false;
                ($scope.ACItem.AccessControlList[4]==='1') ?  $scope.viewProject = true :  $scope.viewProject = false;
                ($scope.ACItem.AccessControlList[5]==='1') ? $scope.modifyProject = true : $scope.modifyProject = false;
                ($scope.ACItem.AccessControlList[6]==='1') ? $scope.viewTrend = true : $scope.viewTrend = false;
                ($scope.ACItem.AccessControlList[7]==='1') ? $scope.modifyTrend = true : $scope.modifyTrend = false;
                ($scope.ACItem.AccessControlList[8]==='1') ? $scope.viewActivity = true : $scope.viewActivity = false;
                ($scope.ACItem.AccessControlList[9]==='1') ? $scope.modifyActivity = true : $scope.modifyActivity = false;
                ($scope.ACItem.AccessControlList[10]==='1') ? $scope.memberManagement = true : $scope.memberManagement = false;
                ($scope.ACItem.AccessControlList[11]==='1') ? $scope.roleManagement = true : $scope.roleManagement = false;
                ($scope.ACItem.AccessControlList[12]==='1') ? $scope.lookupMangement = true : $scope.lookupMangement = false;
                ($scope.ACItem.AccessControlList[13]==='1') ? $scope.viewReport = true : $scope.viewReport = false;
                ($scope.ACItem.AccessControlList[14] === '1') ? $scope.viewChart = true : $scope.viewChart = false;
                ($scope.ACItem.AccessControlList[15] === '1') ? $scope.viewLabor = true : $scope.viewLabor = false;
            }

      /*  $scope.setModifyProgramElement = function(data){
            ($scope.modifyProgramElement=='0')? $scope.modifyProgramElement = '1' : $scope.modifyProgramElement ='0';
            console.log($scope.modifyProgramElement);
        }
        $scope.setViewProject = function(data){
            ($scope.viewProject=='0')? $scope.viewProject = '1' : $scope.viewProject ='0';
            console.log($scope.viewProject);
        }
        $scope.setModifyProject = function(data){
            ($scope.modifyProject=='0')? $scope.modifyProject = '1' : $scope.modifyProject ='0';
            console.log($scope.modifyProject);
        }

        $scope.setViewTrend = function(data){
            ($scope.viewTrend=='0')? $scope.viewTrend = '1' : $scope.viewTrend ='0';
            console.log($scope.viewTrend);
        }
        $scope.setModifyTrend = function(data){
            ($scope.modifyTrend=='0')? $scope.modifyTrend = '1' : $scope.modifyTrend ='0';
            console.log($scope.modifyTrend);
        }
        $scope.setViewActivity = function(data){
            ($scope.viewActivity=='0')? $scope.viewActivity = '1' : $scope.viewActivity ='0';
            console.log($scope.viewActivity);
        }
        $scope.setModifyActivity = function(data){
            ($scope.modifyActivity=='0')? $scope.modifyActivity = '1' : $scope.modifyActivity ='0';
            console.log($scope.modifyActivity);
        }
        $scope.setMemberManagement = function(data){
            ($scope.memberManagement=='0')? $scope.memberManagement = '1' : $scope.memberManagement ='0';
            console.log($scope.memberManagement);
        }
        $scope.setRoleManagement = function(data){
            ($scope.roleManagement=='0')? $scope.roleManagement = '1' : $scope.roleManagement ='0';
            console.log($scope.roleManagement);
        }

        $scope.setLookupManagement = function(data){
            ($scope.lookupMangement=='0')? $scope.lookupMangement = '1' : $scope.lookupMangement ='0';
            console.log($scope.lookupMangement);
        }
        $scope.setViewReport = function(data){
            ($scope.viewReport=='0')? $scope.viewReport = '1' : $scope.viewReport ='0';
            console.log($scope.viewReport);
        }
        $scope.setViewChart = function(data){
            ($scope.viewChart=='0')? $scope.viewChart = '1' : $scope.viewChart ='0';
            console.log($scope.viewChart);
        }*/
        function ConcatenateACL(){
            var temp = "";
            dummyData  =  ($scope.viewProgram)? temp = '1' : temp = '0';
            console.log(dummyData);
            dummyData +=  ($scope.modifyProgram) ? temp = '1' : temp = '0';
            console.log(dummyData);
            dummyData +=  ($scope.viewProgramElement) ? temp = '1':temp ='0';
            dummyData +=  ($scope.modifyProgramElement) ? temp = '1': temp = '0';
            dummyData +=  ($scope.viewProject) ? temp = '1': temp = '0';
            dummyData +=  ($scope.modifyProject) ? temp = '1': temp = '0';

            dummyData +=  ($scope.viewTrend) ? temp = '1' : temp ='0';
            dummyData +=  ($scope.modifyTrend) ? temp = '1':temp ='0';
            dummyData +=  ($scope.viewActivity) ? temp = '1':temp ='0';
            dummyData +=  ($scope.modifyActivity) ? temp = '1' : temp = '0';
            dummyData +=  ($scope.memberManagement) ? temp = '1':temp ='0';
            dummyData +=  ($scope.roleManagement) ?temp = '1': temp='0';

            dummyData +=  ($scope.lookupMangement) ? temp = '1':temp='0';
            dummyData +=  ($scope.viewReport) ? temp = '1': temp ='0';
            dummyData += ($scope.viewChart) ? temp = '1' : temp = '0';
            dummyData += ($scope.viewLabor) ? temp = '1' : temp = '0';
            console.log(dummyData);
        }
        //if($scope.params.newOrEdit === 'Edit' && $scope.params){
        //
        //    copyData();
        //    $scope.saveChanges = function() {
        //        console.log($scope.viewProgram);
        //
        //        ConcatenateACL();
        //        var dataObj = {
        //            "Operation": '2',
        //            "Role": $scope.params.ACItem.Role,
        //            "AccessControlList": dummyData
        //        }
        //       console.log(dataObj);
        //        $http.post(url,dataObj).then(function(response){
        //            if(response.data.result ==='Success'){
        //
        //                $scope.$close();
        //            }else{
        //                dhtmlx.alert('ACL failed to update');
        //            }
        //        })
        //    }
        //}
        //    else if($scope.params.newOrEdit ==='New' && $scope.params ){
        //
        //    $scope.ACItem = null;
        //    $scope.saveChanges = function(){
        //        ConcatenateACL();
        //        var arrayToSave = [];
        //        var dataObj = {
        //            "Operation":"1",
        //            "Role":$scope.ACItem.Role,
        //            "AccessControlList" : dummyData
        //        }
        //        console.log(dataObj);
        //        arrayToSave.push(dataObj);
        //        $http.post(url,arrayToSave).then(function(response){
        //            if(response.data.result === 'Success'){
        //                $scope.$close();
        //            }else{
        //                dhtmlx.alert('Failed to add new ACL');
        //            }
        //        })
        //    }
        //}
        $scope.goBack = function(){
            $scope.$close();
         }

        $scope.saveChanges = function(){
        //    alert();
            ConcatenateACL();
            var arrayToSave = [];
            var dataObj = {
                "Operation":"1",
                "Role":$scope.ACItem.Role,
                "AccessControlList" : dummyData
            }
            console.log(dataObj);
            arrayToSave.push(dataObj);
            $http.post(url, arrayToSave).then(function (response) {
                response.data.result.replace(/[\r]/g, '\n');

                if (response.data.result) {
                    dhtmlx.alert(response.data.result);
                } else {
                    dhtmlx.alert('No changes to be saved.');
                }

                $scope.$close();
            })
        }

    }]);