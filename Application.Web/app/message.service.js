(function() {
    'use strict';

    angular.module('dozapp')
        .factory('messageService', messageService);

    function messageService() {
        var messages = {
            list: [],
            add : _add
        };

        return messages;

        function _add(messageObj) {
            messages.list.push(messageObj);
        }
    }

})();