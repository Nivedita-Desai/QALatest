'use strict';

angular.module('cpp.services').

    //factory('Category',function($resource){
    //    return $resource(serviceBasePath + "Request/activitycategory");
    //   //return $resource("http://localhost:29986/api/Request/activitycategory");
    //}).
    factory('Category', function ($resource) {
        return $resource(serviceBasePath + "Request/ActivityCategoryByOrgID/:OrganizationID/:VersionID");
        //return $resource("http://localhost:29986/api/Request/activitycategory");
    }).
    factory('UpdateCategory',function($resource){
        return $resource(serviceBasePath+"Response/activitycategory/:Operation/:CategoryID/" +
        ":CategoryDescription/:SubCategoryID/:SubCategoryDescription");
    }).
    factory('MainActivityCategory',function($resource){
        return $resource(serviceBasePath + "request/MainActivityCategory/:Phase")
    }).
    factory('SubActivityCategory',function($resource){
        return $resource(serviceBasePath + "request/SubActivityCategory/:OrganizationID/:CategoryID/:Phase")
    }).
    factory('MainActivityCategoryProgram',function($resource){
        return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
    }).
    factory('SubActivityCategoryProgram',function($resource){
        return $resource(serviceBasePath + "request/SubActivityCategoryProgram/:ProgramID/:Phase/:CategoryID")
    }).
    factory('GanttCategory',function($resource){
        //Used this if you want to fetch category on the program level
        return {
            getMainCategory: function () {
                return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")

            },
            getSubCategory: function () {
                return $resource(serviceBasePath + "request/SubActivityCategoryProgram/:ProgramID/:Phase/:CategoryID")
            }
        }
    }).
    factory('ProgramCategory',function($resource){
      //Use this if you want to fetch category on the organization level
        return {

              isExisted : function() {
                return $resource(serviceBasePath + "request/checkExistingActivity/:ProgramID/:ActivityCategoryID",
                    {ProgramID : '@ProgramID', ActivityCategoryID: '@ActivityCategoryID'});
              },
              getMainActivityCategoryProgram: function() {
                  //return $resource(serviceBasePath + "request/MainActivityCategoryProgram/:ProgramID/:Phase")
             //    http://localhost:29986/api/Request/MainActivityCategory/1000
                  return $resource(serviceBasePath + "Request/MainActivityCategory/:Phase/:OrganizationID/:ProjectId")
              },
              getSubActivityCategoryProgram : function(){
                  return $resource(serviceBasePath + "request/SubActivityCategory/:OrganizationID/:CategoryID/:Phase")
              }
      }
    })
;