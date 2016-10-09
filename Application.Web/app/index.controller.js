(function() {
    'use strict';

    angular.module('dozapp')
           .controller('IndexController', IndexController);

    IndexController.$inject = ['$scope', 'indexService', 'messageService', 'ngProgressFactory'];

    function IndexController($scope, indexService, messageService, ngProgressFactory) {
        var vm = this;
        vm.exportData = [];
        vm.selectAll = false;
        vm.selectedStatus = '';
        vm.statuses = [];
        vm.statusALL = 'Всі';
        vm.companies = messageService.list;     
        vm.exportCompanies = exportCompanies;
        vm.filterCompanies = filterCompanies;
        vm.checkCompaniesForSelection = checkCompaniesForSelection;
        vm.selectAllCompanies = selectAllCompanies;
        vm.progressBar = ngProgressFactory.createInstance();
        vm.progressBar.setHeight('4px');

        activate();

        function activate() {

            vm.selectedStatus = vm.statusALL;
            vm.statuses = uniq(vm.companies);
            vm.statuses.push(vm.statusALL);

            function uniq(arr) {
                var a = [];
                for (var i = 0, j = arr.length;i < j; i ++)
                {
                    if (a.indexOf(arr[i].status) === -1 && arr[i] !== '') {
                        a.push(arr[i].status);
                    }
                }
                return a;
            }
        }

        function filterCompanies(value) {
            return value.status === vm.selectedStatus ||
                vm.selectedStatus === vm.statusALL;
        }

        function selectAllCompanies() {
            vm.companies.forEach((company) => {
                company.selected = vm.selectAll;
            });
        }

        function checkCompaniesForSelection() {
            var selected = true;
            vm.companies.forEach((company) => {
                if (company.selected) {
                    selected = false;                    
                }                
            });
            return selected;
        }

        function exportCompanies() {

            // copy selected companies links into array
            vm.exportData = [];
            vm.companies.forEach((company) => {
                if (company.selected) {
                    if (filterCompanies(company)) {
                      vm.exportData.push(company.link);   
                    }                    
                }                
            });
            // clear controls
            vm.selectAll = false;
            vm.companies.forEach((company) => {
                company.selected = false;
            });

            // start progress bar
            vm.progressBar.start();

            // send export request
            indexService.exportTenders(vm.exportData)
                .then(onSuccess)
                .catch(onFailure);

            function onSuccess(response) {
                vm.progressBar.complete();
                window.location = "/Home/RetrieveTenders?reportFileName=" + response.data;
            }
            function onFailure(reject) {
                vm.progressBar.complete();
                alert('Error occured during data export : ' + reject.data);
            }
        };
    };

})();