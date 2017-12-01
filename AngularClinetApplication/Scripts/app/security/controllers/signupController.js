'use strict';
app.controller('signupController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        name: "",
        password: "",
        confirmPassword: ""
    };

    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "User has been registered successfully";
            $location.path('/login');

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Failed to register user due to:" + errors.join(' ');
         });
    };

}]);