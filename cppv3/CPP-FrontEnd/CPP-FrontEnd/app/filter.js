angular.module('xenon.filters', []).
    filter('mapStatus',function(){
        return function(input) {
            if(!input.Role ){
                return input;
            }else if(input.Role){
                return input.Role;
            }

            else{
                return 'unknown';
            }
        };
    }).
    filter('mapRole', function(){
        return function (input) {
            console.log(input);
            if (input) {
                if (!input.Role) {

                    return input;
                } else if (input.Role) {
                    return input.Role;
                } else {
                    return 'unknown';
                }

            }
        };
    }).
    filter('mapFTEPosition', function () {
        return function (input) {
            console.log(input);
            if (input) {
                if (!input.PositionDescription) {

                    return input;
                } else if (input.PositionDescription) {
                    return input.PositionDescription;
                } else {
                    return 'unknown';
                }

            }
        };
    }).
    filter('mapPhase',function(){
        return function (input) {
            console.log(input);
            if (input) {
                if (!input.Code) {

                    return input;
                } else if (input.Code) {
                    return input.Code;
                } else {
                    return 'unknown';
                }

            }
        };
    })

;