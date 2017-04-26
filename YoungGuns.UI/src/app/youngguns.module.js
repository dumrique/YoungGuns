var angular = require('angular');

var dependencies = [
];

angular.module('youngguns', dependencies)
    .controller('youngguns.controller', ['$scope', require('./youngguns.controller')])