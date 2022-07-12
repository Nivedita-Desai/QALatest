'use strict';

angular.module('cpp.services').
    factory('Program', function (   $resource) {
        return {
            lookup: function () {
				return $resource(serviceBasePath+"Request/Program/:OrganizationID/:ProgramID",{OrganizationID:'@OrganizationID',ProgramID:"@ProgramID"});
            },
            persist : function() {
                return $resource(serviceBasePath+"Response/Program");
            }
        }
    }).
    factory("ProgramFund",function($resource)
    {
        return {

            lookup: function(){
                return $resource(serviceBasePath + "request/programFund");
            },
            persist:function(){
                return $resource(serviceBasePath + "Response/programFund");
            }
        }
    });