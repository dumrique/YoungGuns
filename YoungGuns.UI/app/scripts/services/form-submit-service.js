'use strict';

angularApp.service('FormSubmitService', function FormSubmitService($http) {

    return {
        submit: function(form) {
            console.log(form);
        }
    };
});
