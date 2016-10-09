(function() {
    'use strict';

    angular.module('dozapp')
        .config(configurationProvider);

    configurationProvider.$inject = ['$routeProvider', '$locationProvider'];

    function configurationProvider($routeProvider, $locationProvider) {
        $routeProvider
            .when('/',
            {
                templateUrl: '/app/login.html',
                controller: 'LoginController as login'
            })
            .when('/login',
            {
                templateUrl: '/app/login.html',
                controller: 'LoginController as login'
            })
            .when('/index',
            {
                templateUrl: '/app/index.html',
                controller: 'IndexController as index'
            })
            .otherwise({ redirectTo: '/' });
        // use the HTML5 History API
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    };

})();