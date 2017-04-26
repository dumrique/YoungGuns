'use strict';

angularApp.service('FormService', function FormService($http) {
    const baseurl = 'http://youngguns.azurewebsites.net';
    var formsJsonPath = './static-data/sample_forms.json';

    return {
        fields:[
            {
                name : 'textfield',
                value : 'Textfield'
            },
            {
                name : 'email',
                value : 'E-mail'
            },
            {
                name : 'password',
                value : 'Password'
            },
            {
                name : 'radio',
                value : 'Radio Buttons'
            },
            {
                name : 'dropdown',
                value : 'Dropdown List'
            },
            {
                name : 'date',
                value : 'Date'
            },
            {
                name : 'textarea',
                value : 'Text Area'
            },
            {
                name : 'checkbox',
                value : 'Checkbox'
            },
            {
                name : 'hidden',
                value : 'Hidden'
            },
            {
                name : 'calcfield',
                value : 'Calculation'
            }
        ],
        form:function (id) {
            return $http.get(baseurl + '/api/field').then(function (response) {
                return response.data;
            });
        },
        forms: function() {
            return $http.get(baseurl + '/api/allforms');
        },
        submit: function(form) {         
            for(var i=0; i < form.form_fields.length; i++) {
                var field = form.form_fields[i];       
                field.type = field.field_calculation ? 'calc' : 'info';
            }

            return $http.post(baseurl + '/api/taxsystem', form);
        }
    };
});
