'use strict';

angularApp.service('FormService', function FormService($http) {
    const baseurl = 'http://youngguns.azurewebsites.net/';
    const localurl = 'http://localhost:14522/';
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
                value : 'No Calculation'
            },
            {
                name : 'calcformula',
                value : 'Calculate Formula'
            }
        ],
        form:function (id) {
            return $http.get(baseurl + 'api/taxsystem/single?id=' + id);
        },
        forms: function() {
            return $http.get(baseurl + 'api/taxsystem');
        },
        submit: function(form) {         
            for(var i=0; i < form.taxsystem_fields.length; i++) {
                var field = form.taxsystem_fields[i];       
                field.type = field.field_calculation ? 'calc' : 'info';
            }
            console.log('POST', form);
            //return $http.post(baseurl + 'api/taxsystem', form);
        },
        getSession: function() {
            return $http.get(baseurl + 'api/returnsession');
           console.log('submitting form', form);
            return $http.post('http://localhost:8100/api/taxsystem', form).then(function(response) {
                console.log(response.data);
                return response.data;
            })
        }
    };
});
