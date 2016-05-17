app.service("angularService", function ($http) {
    this.getTicketsChainById = function (ticketId) {
        var responce = $http({
            method: "POST",
            url: "GetTicketsChain",
            params: { ticketId: JSON.stringify(ticketId) },
            datatype: "JSON"
        });
        return responce;
    }
    this.deleteImage = function (imageId) {
        var responce = $http({
            method: "GET",
            url: "DeleteImage",
            params: {
                imageId: imageId
            }
        })
    }
    this.deleteTicket = function (ticketId) {
        var responce = $http({
            method: "GET",
            url: "DeleteTicket",
            params: {
                ticketId: ticketId
            }
        })
    }
    this.loadTicketImages = function (ticketId) {
        var responce = $http({
            method: "POST",
            url: "LoadImage",
            params: { ticketId: JSON.stringify(ticketId) },
            datatype: "JSON"
        });
        return responce;
    }
});