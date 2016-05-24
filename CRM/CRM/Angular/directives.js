app.directive('afterAddTicket', ['$timeout', function ($timeout) {
    return function (scope, elem, attr) {
        scope.afterAddTicket(elem);
    };
}]);