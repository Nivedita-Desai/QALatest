'use strict';

angular.module('cpp.services')
	// super simple service
	// each function returns a promise object 
	.factory('myLocalStorage', function ($rootScope) {
        return {

            get: function (key) {
               return localStorage.getItem(key);
            },

            set: function (key, data) {
               localStorage.setItem(key, data);
            },

            setJSON: function (key, data) {
               localStorage.setItem(key, JSON.stringify(data));
            },
            
            remove: function (key) {
                localStorage.removeItem(key);
            },

            clearAll : function () {
                localStorage.clear();
            }
        };
    });