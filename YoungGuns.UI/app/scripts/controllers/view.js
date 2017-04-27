'use strict';

var ViewCtrl = angularApp.controller('ViewCtrl', function ($scope, FormService, MockFormService) {
	$scope.previewMode = false;
	 FormService.getSession().then(function(response) {
		$scope.session = response.data;
	 })

	MockFormService.forms().then(function (response) {
		$scope.forms = response.data;
	});

	$scope.selectForm = function(form) {
		$scope.form = form;
	}
	
    $scope.form = {};

	$scope.get = function() {
//		MockFormService.forms().then(function(response) {
//			$scope.originalData = response.data[0];
//			$scope.previewMode = !$scope.previewMode;
//			$scope.previewFormx = angular.copy(response.data[0]);
//		})
		 var formData = FormService.form($scope.form.id).then(function (response) {
		 	$scope.previewMode = !$scope.previewMode;
		 	$scope.previewFormx = angular.copy(response.data);
		 });
	}

	$scope.calculate = function() {
		var response = FormService.calculate($scope.previewFormx, $scope.originalData);
		$scope.previewFormx.taxsystem_fields = angular.copy(response);
	}
});
