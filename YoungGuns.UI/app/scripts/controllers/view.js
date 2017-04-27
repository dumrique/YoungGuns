'use strict';

var ViewCtrl = angularApp.controller('ViewCtrl', function ($scope, FormService) {
	$scope.forms = FormService.forms();

	$scope.selectForm = function(form) {
		$scope.form = form;
	}
	
    $scope.form = {};

	$scope.update = function(id) {
		console.log('update', id);
	}
});
