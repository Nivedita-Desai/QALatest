'use strict';

angular.module('cpp.services').
    factory('ProgramElement', function ($resource) {
        return {
            lookup : function() {
				return $resource(serviceBasePath+"Request/ProgramElement/:ProgramID/:ProgramElementID",
                    {ProgramID:'@ProgramID',ProgramElementID:"@ProgramElementID"});
            },
            persist : function() {
                return $resource(serviceBasePath+"Response/ProgramElement");
            }
        }    
    });
