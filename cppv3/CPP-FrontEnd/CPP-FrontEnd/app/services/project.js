'use strict';

angular.module('cpp.services').
    factory('Project', function ($resource) {
        return {
            lookup : function() {
				return $resource(serviceBasePath+"Request/Project/:ProgramID/:ProgramElementID/:ProjectID",
                    {ProgramID:'@ProgramID',ProgramElementID:"@ProgramElementID",ProjectID:"@ProjectID"} );
            },
            persist : function() {
                return $resource(serviceBasePath+"Response/Project");
            }
        }    
    }).
    factory('test',function($resource){
        return $resource(serviceBasePath+"Request/Project/:ProgramID/:ProgramElementID/:ProjectID");

    }).
    factory('getMaximumFutureDate',function($resource){
       return $resource(serviceBasePath + "Request/MaxFutureDate/:ProjectID");
    }).
    factory('currentProject',function($resource){
        return $resource(serviceBasePath + "Request/CurrentProject/:ProjectID");
    }).
    factory('getProjectByOrg', function($resource){
      return $resource(serviceBasePath + "Request/ProjectByOid/:OrganizationID");
    });
