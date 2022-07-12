/**
 * Created by ikhong on 1/21/2016.
 */
'use strict';

angular.module('cpp.services').
    factory('ProjectScope', function ($resource) {
        return {
            lookup : function() {
                return $resource(serviceBasePath+"Request/ProjectScope/:ProjectID",
                    {ProjectID:'@ProjectID'} );
            },
            persist : function() {
                return $resource(serviceBasePath+"Response/ProjectScope");
            }
        }
    })
   ;
