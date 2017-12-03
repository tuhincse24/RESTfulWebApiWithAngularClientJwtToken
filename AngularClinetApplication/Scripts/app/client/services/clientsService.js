'use strict';
app.factory('clientsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var clientsServiceFactory = {};

    var _getClients = function () {

        return $http.get(serviceBase + 'api/clients').then(function (results) {
            return results;
        });
    };

    clientsServiceFactory.getClients = _getClients;

    return clientsServiceFactory;

}]);