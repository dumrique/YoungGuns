'use strict';

angularApp.service('FormSubmitService', function FormSubmitService($http) {

    return {
        submit: function(form) {
            const baseurl = 'youngguns.southcentralus.cloudapp.azure.com:19000';
            
            for(var i=0; i < form.form_fields.length; i++) {
                var field = form.form_fields[i];       
                field.type = field.field_calculation ? 'calc' : 'info';
            }

            console.log(form);
        }
    };
});