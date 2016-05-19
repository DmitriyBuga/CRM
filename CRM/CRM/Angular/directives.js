app.directive('afterAddTicket', ['$timeout', function ($timeout) {
    return function (scope, elem, attr) {
        //$timeout(function () { scope.afterAddTicket(elem); }, 1000);
        //scope.afterAddTicket(elem);
        scope.afterAddTicket(elem);
       
    };
}]);