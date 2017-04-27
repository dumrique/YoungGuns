'use strict';

var ViewCtrl = angularApp.controller('ViewCtrl', function ($scope, FormService, MockFormService) {
	$scope.previewMode = false;
	// FormService.getSession().then(function(response) {
	// 	// ??
	// })

	MockFormService.forms().then(function (response) {
                $scope.forms = response.data;
            });

	$scope.selectForm = function(form) {
		$scope.form = form;
	}
	
    $scope.form = {};

	$scope.get = function() {
		MockFormService.forms().then(function(response) {
			$scope.previewMode = !$scope.previewMode;
			$scope.previewFormx = angular.copy(response.data[0]);
			
			console.log($scope.previewFormx)
		})
		// var formData = FormService.form($scope.form.id).then(function (response) {
		// 	$scope.previewMode = !$scope.previewMode;

		// 	$scope.previewFormx = angular.copy(response.data);
		// });
	}

	$scope.calculate = function() {
		FormService.calculate($scope.previewFormx).then(function(response) {
			$scope.previewFormx.taxsystem_fields = response.data;
		})
	}
});
