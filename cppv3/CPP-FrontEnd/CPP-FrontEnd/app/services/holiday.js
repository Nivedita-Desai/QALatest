'use strict';

angular.module('cpp.services').
    factory('Holiday', function ($resource) {
        return $resource(serviceBasePath + "Request/Holiday");
    }).
    factory('UpdateHoliday', function ($resource) {
        return $resource(serviceBasePath + "Response/Holiday");
    }).
    factory("Holidaysbyyear", function ($resource) {
        return {
            lookup: function () {
                return $resource(serviceBasePath + "Request/Holiday/:year");
            }
        };
    });

