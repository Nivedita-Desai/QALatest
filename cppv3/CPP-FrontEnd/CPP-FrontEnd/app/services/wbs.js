'use strict';

angular.module('cpp.services').
    factory('WbsService', function ($resource, $cacheFactory) {
        return {
            getWBS: function (uID, orgId, pgmId, pgmEltId, projId, searchText, allData, deptID) {
                //var cache;
                //if ($cacheFactory.get('wbs')) {
                //   cache = $cacheFactory.get('wbs');
                //} else {
                //    cache = $cacheFactory('wbs');
                //}
                var url = serviceBasePath + "Request/WBS/" + uID + "/" + orgId + "/" + pgmId + "/" + pgmEltId + "/" + projId + "/null/null/null/null/null/" + searchText + "/" + allData + "/" + deptID;
                return $resource(serviceBasePath + "Request/WBS/" + uID + "/" + orgId + "/" + pgmId + "/" + pgmEltId + "/" + projId + "/null/null/null/null/null/" + searchText + "/" + allData + "/" + deptID, {}, {
                    'get': { method: 'GET', cache: false },
                    'query': { method: 'GET', cache: false, isArray: true }
                });
            }
        };
    });
