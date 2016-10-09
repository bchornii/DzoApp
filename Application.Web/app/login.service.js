(function () {
    'use strict';

    angular.module('dozapp')
        .factory('loginService', loginService);

    loginService.$inject = ['$http'];

    function loginService($http) {
        var loginServ = {
            login : _login
        };

        return loginServ;

        function _login(loginData) {
            return $http({
                method: 'POST',
                url: '/Home/Login',
                data: loginData
            });
        }
    }

})();