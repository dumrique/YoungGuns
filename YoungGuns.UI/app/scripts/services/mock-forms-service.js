'use strict';

angularApp.service('MockFormService', function FormService($http) {
    var formsJsonPath = './static-data/sample_forms.json';

    return {
        forms: function() {
            return $http.get(formsJsonPath);
        }
    };
});
