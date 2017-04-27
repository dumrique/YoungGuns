'use strict';

var ViewCtrl = angularApp.controller('ViewCtrl', function ($scope, FormService) {
	$scope.previewMode = false;
	// FormService.getSession().then(function(response) {
	// 	// ??
	// })

	FormService.forms().then(function (response) {
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
		});
	}

	$scope.calculate = function() {

	}
});
