/**
 * Created by ikhong on 3/15/2016.
 */
'use strict';

angular.module('cpp.services').
    factory('Session',function($resource){
        return {
            extend :function(){
                return $resource(serviceBasePath + "Request/ExtendSession")
            }
        }

    });