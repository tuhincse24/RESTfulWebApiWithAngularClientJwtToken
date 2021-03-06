﻿
var app = angular.module('angularApp', ['ngRoute', 'ui.bootstrap', 'LocalStorageModule']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        templateUrl: "/Scripts/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/Scripts/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/Scripts/app/views/signup.html"
    });

    $routeProvider.when("/clients", {
        controller: "clientsController",
        templateUrl: "/Scripts/app/views/clients.html"
    });

    $routeProvider.when("/about", {
        controller: "aboutController",
        templateUrl: "/Scripts/app/views/about.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var serviceBase = 'http://localhost:59598/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'IAmTheFirstClient'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


