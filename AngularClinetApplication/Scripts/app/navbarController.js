(function () {

    var injectParams = ['$scope', '$location', 'authService'];

    var NavbarController = function ($scope, $location, authService) {
            $scope.appTitle = 'Client Management';
            $scope.authentication = authService.authentication;

        $scope.logOut = function () {
                var isAuthenticated = authService.authentication.isAuthenticated;
                if (isAuthenticated) { //logout 
                    authService.logOut();
                    $location.path('/');
                    return;
                }
            };

        $scope.login = function () {
            var path = '/login';
            $location.replace();
            $location.path(path);
        };

    };

    NavbarController.$inject = injectParams;

    angular.module('angularApp').controller('navbarController', NavbarController);

}());
