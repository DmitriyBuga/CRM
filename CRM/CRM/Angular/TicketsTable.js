function HightlightMenu(path, cur_url) {
    //навешиваем события по наведению мыши  
    jQuery(path).each(function () {
        jQuery(this).mouseover(function () { jQuery(this).parent('li').addClass('active'); });
        jQuery(this).mouseout(function () { jQuery(this).parent('li').removeClass('active'); })
    });
    //Ищем подходящий пункт меню для выделения  
    var url = "";
    if (cur_url == "")
        url = window.location.toString();
    else
        url = cur_url;

    var max = 0;
    var link = null;

    jQuery(path).each(function () {
        //finding the longest href  
        if (url.indexOf(this.href) >= 0 && this.href.length > max) {
            link = this;
            max = this.href.length;
        }
    });

    if (link)
        jQuery(link).parent('li').addClass('current');
}

function initMenu() {
    //left navigation current item highlight  
    HightlightMenu("#top_menu > li > a", "");
};

jQuery(document).ready(function () {
    initMenu();
});
app.controller("crmTableController", function ($scope, $uibModal, $filter, $timeout, angularService, viewModel) {
    
    $scope.lNewTicket = false;
    $scope.tasks = viewModel.tasks;
    $scope.tickets = viewModel.tickets;
    $scope.selectedTicket = {};
    $scope.searchTicket = function (data, ticketId) {
        var returnData;
        for (var i = 0; i < data.length; i++) {
            if (data[i].id == ticketId) {
                return data[i]
            }
            if (data[i].currentTicketsChain.length > 0)
                returnData = $scope. searchTicket(data[i].currentTicketsChain, ticketId);
        }
        return returnData;
    }
    $scope.$watch('lNewTicket', function () {
        //don't append image in new ticket before saving
        var loadImageBtn = angular.element(document.querySelector('#authUser_loadFile'));
        //alert(loadImageBtn.disabled);
        if (loadImageBtn !=  undefined) {
            loadImageBtn[0].disabled = !loadImageBtn[0].disabled
        }
        
    });
    $scope.getTicket = function (ticketId, ticketsChain) {
        function searchTicket(data, ticketId) {
            var returnData;
            for (var i = 0; i < data.length; i++) {
                if (data[i].id == ticketId) {
                    return data[i]
                }
                if (data[i].currentTicketsChain.length > 0)
                    returnData = searchTicket(data[i].currentTicketsChain, ticketId);
            }
            return returnData;
        }
        return searchTicket(ticketsChain, ticketId);
    }
    $scope.saveTicket = function (ticketId) {
        var getData = angularService.saveTicket($scope.selectedTicket);
        if (getData.then(function (imageData) {
        {
            $scope.selectedTicket.id = parseInt(imageData.data);
            $scope.lNewTicket = false;
        }
        }, function (err) { alert(err); }));
    }
    setSelectedTicketImages = function(ticketId){
        var getData = angularService.loadTicketImages(ticketId);
        if (getData.then(function (imageData) {
        {
            $scope.selectedTicket.images = new Array();
            for (var i = 0; i < imageData.data.length; i = i + 2) {
                $scope.selectedTicket.images[i/2] = new Array(imageData.data[i], imageData.data[i+1]);
                //$scope.selectedTicket.images[i/2][1] = imageData.data[i+1];
            }
            
        }
        }, function (err) { alert(err); }));
    }
    $scope.afterAddTicket = function (elem) {
        if ($scope.lNewTicket) {
            var active = angular.element(document.querySelector('#ticket_in_tree.active'));
            if (active.length > 0) {
                active.removeClass('active');
                elem.addClass('active');
            }
        }
        
        //$timeout($scope.setFirstActiveTicketButton(0), 100000)
    }
    $scope.setFirstActiveTicketButton = function (ticketId) {
        var active = angular.element(document.querySelector('#ticket_in_tree.active'));
        if (active.length == 0) {
            active = angular.element(document.querySelector('#ticket_in_tree'));
            active.addClass('active');
        }
    }
    $scope.setButtonSelected = function(title){
        var active = angular.element(document.querySelector('#ticket_in_tree.active'));
        if (active.length > 0)
        {
            active.removeClass('active');
            //
            active = angular.element(document.querySelectorAll('#ticket_in_tree'));
            for (var i = 0; i < active.length; i++)
            {
                if(active[i].title == title)
                    active.eq(i).addClass('active');
            }
        }
        else
        {
            active = angular.element(document.querySelector('#ticket_in_tree'));
            active.addClass('active');
        }
        
    }
    $scope.setSelectedTicket = function (ticketId) {
        $scope.selectedTicket = {};
        $scope.selectedTicket = $scope.getTicket(ticketId, $scope.currentTicketsChain);
        $scope.selectedTicket.images = {};
        setSelectedTicketImages(ticketId);
    }
    $scope.selectTicketInTree = function (ticketId) {
        var active = angular.element(event.target);
        $scope.setButtonSelected(active[0].title)
        $scope.setSelectedTicket(ticketId);
    }
    $scope.getCurrentImage = function () {
        return $scope.currentImage;
    }
    $scope.openPhoto = function (slide) {
        $scope.currentImage = slide;
    }
    $scope.selectTask = function (nTask) {
        angular.element(document.querySelector('.task.active')).removeClass('task active');
        angular.element(event.target.parentElement).addClass('task active');
    }
    $scope.deleteTicket = function () {
        if (confirm("Вы действительно хотите удалить текущую заявку?"))
        {
            if (!$scope.lNewTicket)
                angularService.deleteTicket($scope.selectedTicket.id);
            function arrayCloneWODeleted(arr) {
                var i, copy;
                if (typeof arr === 'object' && arr != null && arr.length > 0)
                {
                    if (arr[0].hasOwnProperty('id') && arr[0].id == $scope.selectedTicket.id) {
                        if (arr[0].hasOwnProperty('parent_id') && arr[0].parent_id == null)  //Надо удалить из основной таблицы
                        {
                            $scope.tickets = _.reject($scope.tickets, function (num) { return num.id == $scope.selectedTicket.id; });   
                        }
                        arr = {};
                        return arr;
                    }
                    if (arr[0].hasOwnProperty('currentTicketsChain') && arr[0].currentTicketsChain.length > 0)
                    {
                        arr[0].currentTicketsChain = _.reject(arr[0].currentTicketsChain, function(num) {return num.id == $scope.selectedTicket.id;});
                    }
                }
                if (Array.isArray(arr)) {
                    copy = arr.slice(0);
                    for (i = 0; i < copy.length; i++) {
                        copy[i] = arrayCloneWODeleted(copy[i]);
                    }
                    return copy;
                } else if (typeof arr === 'object') {
                    copy = {};
                    for (var key in arr) {
                        copy[key] = arrayCloneWODeleted(arr[key]);
                    }
                    return copy;
                } else {
                    return arr;
                }
            }

            $scope.currentTicketsChain = arrayCloneWODeleted($scope.currentTicketsChain);
            if ($scope.selectedTicket.parent_id >= 0)
            {
                $scope.setSelectedTicket($scope.selectedTicket.parent_id)
                $scope.setButtonSelected($scope.selectedTicket.id)
                $scope.lNewTicket = false;
                //$scope.selectTicketInTree();
                    
            }
        }
    }
    $scope.createNewTicketObject = function () {
        var ticket = {
            parent_id: $scope.selectedTicket.id,
            currentTicketsChain: {},
            subject: $scope.selectedTicket.subject,
            children: $scope.selectedTicket.children,
            cust_id: $scope.selectedTicket.cust_id,
            department_id: $scope.selectedTicket.department_id,
            email: $scope.selectedTicket.email,
            manager_id: $scope.selectedTicket.manager_id,
            task_id: $scope.selectedTicket.task_id,
            user_id: $scope.selectedTicket.user_id,
            status_id: $scope.selectedTicket.status_id,
            body: $scope.searchTicket.body,
            reference: "",
            responce: "",
            mail_data:"",
            id: -1
        }
        return ticket;
    }
    $scope.answerTicket = function () {
        /*
        var temp = {
            parent_id: $scope.selectedTicket.id,
            currentTicketsChain: {},

            subject: $scope.selectedTicket.subject,
            id: -1
        }
        */
        $scope.selectedTicket.currentTicketsChain.push($scope.createNewTicketObject());
        $scope.selectedTicket = $scope.getTicket(-1, $scope.currentTicketsChain);
        $scope.selectedTicket.images = {};
        $scope.lNewTicket = true;
    }
    $scope.deleteImage = function (imageId)
    {
        angularService.deleteImage(imageId);
        $scope.selectedTicket.images = _.reject($scope.selectedTicket.images, function (num) { return num[1] == imageId;})
        //$scope.selectedTicket.images
    }
    $scope.editTicket = function ($event, id) {
        $scope.lExpandTicket = true;
        $scope.currentTicketsChain = [];
        if(id >= 0){
            var getData = angularService.getTicketsChainById(id);
            if (getData.then(function (ticketsChain) {
                var nodes = ticketsChain.data;
                var map = {}, node, roots = [];
                for (var i = 0; i < nodes.length; i += 1) {
                    node = nodes[i];
                    node.currentTicketsChain = [];
                    map[node.id] = i; // use map to look-up the parents
                    if (node.parent_id != null) {
                        nodes[map[node.parent_id]].currentTicketsChain.push(node);
                    }
                    else {
                        $scope.currentTicketsChain.push(node);
                    }
                }
                $scope.selectTicketInTree(id);
            }, function (err) { alert("Не удалось получить данные");}));
        }
    };
    $scope.file_load = function (element) {
        
        $scope.$apply(function (scope) {
            var data = new FormData();

            for (var i in element.files) {
                data.append("uploadedFile", element.files[i]);
            }
            data.append("ticketId", JSON.stringify($scope.selectedTicket.id))

            // ADD LISTENERS.
            var objXhr = new XMLHttpRequest();
            //objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);

            // SEND FILE DETAILS TO THE API.

            objXhr.open("POST", "/Tickets/FileUpload/");
            //objXhr.setRequestHeader("id", estateId);
            //objXhr.setRequestHeader("Content-Type", element.type);
            objXhr.send(data);
            /*
                        var photofile = element.files[0];
                        
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            
                            // handle onload
                        };
                        reader.readAsDataURL(photofile);
                        alert(reader)
                        */

        });
        //$scope.slides.pop(element.files[0])

    };
    function transferComplete(e) {
        debugger
        setSelectedTicketImages($scope.selectedTicket.id);
        //alert("Files uploaded successfully.");
        //alert($scope.estateId);
     //   $scope.getImageList($scope.estateId);
    }
});
