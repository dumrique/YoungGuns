'use strict';

angularApp.service('FormService', function FormService($http) {
    const baseurl = 'http://youngguns.southcentralus.cloudapp.azure.com:80/';
    const localurl = '	Http://localhost:80/';
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
            return $http.get(localurl + 'api/taxsystem/single?id=' + id);
        },
        forms: function() {
            return $http.get(localurl + 'api/taxsystem');
        },
        submit: function(form) {         
//            for(var i=0; i < form.taxsystem_fields.length; i++) {
//                var field = form.taxsystem_fields[i];       
//                field.type = field.field_calculation ? 'calc' : 'info';
//            }
            return $http.post(localurl + 'api/taxsystem', form);
        },
        getSession: function() {
            return $http.get(localurl + 'api/returnsession');
        },
        calculate: function(form, originalForm, sessionGuid, taxSystemId) {
            sessionGuid = sessionGuid.replace(/['"]+/g, '');
            //console.log('KWEJFOIQEWJFOPIJWEIOFJ', sessionGuid.replace(/['"]+/g, ''));
            var session = {};
            $http.post(localurl + 'api/returnsession', {sessionGuid: sessionGuid, taxSystemId: taxSystemId}).then(function(response) {
                session.sessionId = sessionGuid;
                session.returnId = response.data.returnId;
                session.baseVersion = response.data.baseVersion;

                var requestBody = { newValues: {}, sessionGuid: session.sessionId, returnId: session.returnId, baseVersion: session.baseVersion};
                for(var i = 0; i < form.fields.length; i++) {
                    var field = form.fields[i];
                    var originalField = originalForm.fields.filter((f) => f.field_id === field.field_id)[0];
    
                    if(field.field_value !== originalField.field_value) {             
                        var x = parseInt(field.field_value, 10);
                        requestBody.newValues[field.field_id] = x;
                       
                    }
                }
                console.log('NEW VALUES', requestBody.newValues)
                var rvalue = form.fields;

                $http.put(localurl + 'api/return', requestBody).then(function(response) {
                    //update the given taxsystem fields with the values returned here.

                    Object.keys(response.data.returnSnapshot.fieldValues).forEach(function(key, index) {
                        var field = rvalue.filter((f) => f.field_id == key)[0];
                        field.field_value = response.data.returnSnapshot.fieldValues[key];
                    })
                    console.log('PUT RVALUE IS', rvalue)
                    return rvalue;
                })
            })
        }
    };
});
