(function() {
    'use strict';

    angular.module('dozapp')
        .factory('indexService', indexService);

    indexService.$inject = ['$http'];

    function indexService($http) {
        var indexServ = {
            exportTenders : _exportTenders
        };

        return indexServ;

        function _exportTenders(links) {
            return $http({
                method: 'POST',
                url: '/Home/RetrieveTenders',
                data : links
            });
        }
    }

})();