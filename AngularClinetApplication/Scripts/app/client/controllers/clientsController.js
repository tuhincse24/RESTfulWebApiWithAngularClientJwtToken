'use strict';
app.controller('clientsController', ['$scope', 'clientsService', function ($scope, ordersService) {

    $scope.orders = [];

    ordersService.getOrders().then(function (results) {

        $scope.clients = results.data;

    }, function (error) {
    });

}]);