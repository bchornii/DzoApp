(function () {
    'use strict';

    angular.module('dozapp')
           .controller('LoginController', LoginController);

    LoginController.$inject = ['loginService', '$location', 'messageService', 'ngProgressFactory'];

    function LoginController(loginService, $location, messageService, ngProgressFactory) {
        var vm = this;

        vm.loginData = {
            userName: '',
            password: ''            
        };        
        vm.login = login;
        vm.errorOccured = false;
        vm.errMessage = '';
        vm.progressBar = ngProgressFactory.createInstance();
        vm.progressBar.setHeight('4px');

        function login() {
            vm.progressBar.start();
            loginService.login(vm.loginData)
                .then(onSuccess)
                .catch(onFailed);

            function onSuccess(response) {
                if (response.status === 200) {
                    messageService.list = [];
                    response.data.forEach(function(val) {
                        messageService.add(val);
                    });
                    vm.progressBar.complete();
                    $location.path('/index');
                }
            }
            function onFailed(reject) {
                vm.progressBar.complete();
                vm.errorOccured = true;                
                if (reject.status === 403) {
                    vm.errMessage = 'Please check login and password.';
                } else {
                    vm.errMessage = 'Unknown server error.';
                }
                clearInputs();
            }
        }

        function clearInputs() {
            vm.loginData.userName = '';
            vm.loginData.password = '';            
        }
    };

})();