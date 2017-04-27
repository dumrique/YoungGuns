'use strict';

var ViewCtrl = angularApp.controller('ViewCtrl', function ($scope, FormService, MockFormService) {
	$scope.previewMode = false;
	 FormService.getSession().then(function(response) {
		$scope.session = response.data;
	 })

	FormService.forms().then(function (response) {
		console.log('FORMS', response.data);
		$scope.forms = response.data;
	});

	$scope.selectForm = function(form) {
		$scope.form = form;
	}
	
    $scope.form = {};

	$scope.get = function() {
		 var formData = FormService.form($scope.form.id).then(function (response) {
		 	$scope.previewMode = !$scope.previewMode;
		 	$scope.previewFormx = angular.copy(response.data);
			 console.log('COPIED DATA', response.data);
		 });
	}

	$scope.calculate = function() {
		var response = FormService.calculate($scope.previewFormx, $scope.originalData);
		$scope.previewFormx.taxsystem_fields = angular.copy(response);
	}
});
