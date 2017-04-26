// 'use strict';

// angularApp.service('FormSubmitService', function FormSubmitService($http) {

//     return {
//         submit: function(form) {
//             const baseurl = 'http://youngguns.azurewebsites.net';
            
//             for(var i=0; i < form.form_fields.length; i++) {
//                 var field = form.form_fields[i];       
//                 field.field_type = field.field_calculation ? 'calc' : 'info';
//             }
//             console.log(form);
//             // $http.post(baseurl + '/api/field', form).then(function(response) {
//             //     console.log(response);
//             // })
//             // .catch(function(response) {
//             //     console.log('POST form request failed', response)
//             // })
//         }
//     };
// });