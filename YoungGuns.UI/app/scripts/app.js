'use strict';

var angularApp = angular.module('angularjsFormBuilderApp', ['ui.bootstrap', '$strap.directives']);

angularApp.config(function ($routeProvider) {

    $routeProvider
        .when('/', {
            templateUrl: 'views/create.html',
            controller: 'CreateCtrl'
        })
        .when('/forms/create', {
            templateUrl: 'views/create.html',
            controller: 'CreateCtrl'
        })
        .when('/forms/:id/view', {
            templateUrl: 'views/view.html',
            controller: 'ViewCtrl'
        })
        .otherwise({
            redirectTo: '/forms/create'
        });

}).run(['$rootScope',  function() {}]);


