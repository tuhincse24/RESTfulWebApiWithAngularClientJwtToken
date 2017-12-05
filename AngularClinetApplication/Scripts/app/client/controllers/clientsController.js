'use strict';
app.controller('clientsController', ['$scope', 'clientsService', function ($scope, clientsService) {

    $scope.clients = [];

    clientsService.getClients().then(function (results) {

        $scope.clients = results.data;

    }, function (error) {
    });

}]);