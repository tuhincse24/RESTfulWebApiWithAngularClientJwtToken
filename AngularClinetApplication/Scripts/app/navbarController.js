(function () {

    var injectParams = ['$scope', '$location', 'authService'];

    var NavbarController = function ($scope, $location, authService) {
            $scope.appTitle = 'Client Management';

        $scope.isCollapsed = false;

        $scope.highlight = function (path) {
            return $location.path().substr(0, path.length) === path;
        };

        $scope.loginOrOut = function () {
            setLoginLogoutText();
            var isAuthenticated = authService.user.isAuthenticated;
            if (isAuthenticated) { //logout 
                authService.logout().then(function () {
                    $location.path('/');
                    return;
                });                
            }
            redirectToLogin();
        };

        function redirectToLogin() {
            var path = '/login' + $location.$$path;
            $location.replace();
            $location.path(path);
        }

        $scope.$on('loginStatusChanged', function (loggedIn) {
            setLoginLogoutText(loggedIn);
        });

        $scope.$on('redirectToLogin', function () {
            redirectToLogin();
        });

        function setLoginLogoutText() {
            $scope.loginLogoutText = (authService.authentication.isAuthenticated) ? 'Logout' : 'Login';
        }

        setLoginLogoutText();

    };

    NavbarController.$inject = injectParams;

    angular.module('angularApp').controller('navbarController', NavbarController);

}());
