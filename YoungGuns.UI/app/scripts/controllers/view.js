'use strict';

var ViewCtrl = angularApp.controller('ViewCtrl', function ($scope, FormService) {
	$scope.forms = [
		{
			id: 1,
			name: 'form1'
		},
		{
			id: 2,
			name: 'form2'
		},
		{
			id: 3,
			name: 'form3'
		}
	];

	$scope.selectForm = function(form) {
		$scope.form = form;
	}
	
    $scope.form = {};

	$scope.update = function(id) {
		console.log('update', id)
	}
});
