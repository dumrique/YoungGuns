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
            //return $http.post(baseurl + 'api/taxsystem', form);
        },
        getSession: function() {
            return $http.get(baseurl + 'api/returnsession');
           console.log('submitting form', form);
            return $http.post('http://localhost:8100/api/taxsystem', form).then(function(response) {
                return response.data;
            })
        },
        calculate: function(form, originalForm) {
            var requestBody = {};
            for(var i = 0; i < form.taxsystem_fields.length; i++) {
                var field = form.taxsystem_fields[i];
                var originalField = originalForm.taxsystem_fields.filter((f) => f.field_id === field.field_id)[0];
   
                if(field.field_value !== originalField.field_value) {             
                    requestBody[field.field_id] = field.field_value;
                }
            }
            console.log('requestr body is', requestBody);
            var rvalue = form.taxsystem_fields;

            return $http.get('./static-data/fakecalculateddata.json').then(function(response) {
                //update the given taxsystem fields with the values returned here.

                Object.keys(response.data.returnSnapshot.fieldValues).forEach(function(key, index) {
                    var field = rvalue.filter((f) => f.field_id == key)[0];
                    field.field_value = response.data.returnSnapshot.fieldValues[key];
                })
                return rvalue;
            })
        }
    };
});
