
var onboardapp = angular.module('onboardapp', ['ui.bootstrap']);

onboardapp.controller('CustomerController', function ($scope, $uibModal, $http, httpService) {
    $scope.CustomerList = [];

    $scope.Id = -1;
    $scope.Name = "";
    $scope.Address1 = "";
    $scope.Address2 = "";
    $scope.City = "";

    //initialize view from database
    $http({
        method: 'GET',
        url: '/Customer/Get'
    }).then(function successCallback(response) {
        $scope.CustomerList = response.data;
    }, function errorCallback(response) {
    }
        );



    $scope.delete = function (customer) {
        httpService.delete($scope, customer);
    }

    $scope.submit = function () {
        if ($scope.Id === -1) {
            httpService.add($scope);
        }
        else {
            httpService.edit($scope);
            //$scope.reset();
        }

    }

    $scope.edit = function (customer) {
        $scope.Id = customer.Id;
        $scope.Name = customer.Name;
        $scope.Address1 = customer.Address1;
        $scope.Address2 = customer.Address2;
        $scope.City = customer.City;
    }

    $scope.reset = function () {
        $scope.Id = -1;
        $scope.Name = "";
        $scope.Address1 = "";
        $scope.Address2 = "";
        $scope.City = "";
    }


}); //end of controller

onboardapp.controller('OrdersController', function ($scope, $uibModal, $http, httpOrderService) {
    $scope.OrdersList = [];
    $scope.CustomerList = []; //list of customer objects [{Id: 3, CustomerName: "something"}]
    $scope.ProductList = []; //list of product objects [{Id: 4, Productname: "something", Price: "something"}]

    $scope.Id = -1;
    $scope.OrderId = -1;
    $scope.Date = new Date();
    $scope.Customer = {};
    $scope.Product = {};

    $http.get('/Customer/Get').then(function (res) {
        $scope.CustomerList = res.data;
    });

    $http.get('/Products/Get').then(function (res) {
        $scope.ProductList = res.data;
    });

    $http.get('/Orders/Get').then(function (res) {
        $scope.OrdersList = res.data;
        //$scope.convertDateTime($scope.OrdersList);
        for (var i = 0; i < $scope.OrdersList.length; i++) {
            $scope.convertDateTime($scope.OrdersList[i]);
        }
        var lastOrder = $scope.OrdersList[$scope.OrdersList.length - 1];
        $scope.OrderId = lastOrder.OrderId + 1;
    });

    $scope.convertDateTime = function(Order){
            Order.Date = new Date(parseInt(Order.Date.substr(6)));
    };

    $scope.submit = function () {
        if ($scope.Id === -1) {
            httpOrderService.add($scope);
        }
        else {
            httpOrderService.edit($scope);
        }
    };

    $scope.edit = function (order) {

        $scope.Id = order.Id;
        $scope.OrderId = order.OrderId;

   
        //$scope.Date = new Date(parseInt(order.Date.substr(6)));
        $scope.Date = order.Date;

        //function findCustomerId(element) {
        //    return element == order.CustomerId;
        //}
        //function findProductId(element) {
        //    return element == order.ProductId;
        //}
        $scope.Customer = $scope.CustomerList.find(x => x.Id == order.CustomerId);
        $scope.Product = $scope.ProductList.find(x => x.Id == order.ProductId);
        //var pIndex = $scope.ProductList[].findIndex(findProductId);
        //$scope.Product = $scope.ProductList[pIndex];
        //$scope.Customer =  {
        //    Id: order.CustomerId,
        //    Name: order.CustomerName,
        //}

        //$scope.Product = {
        //    Id: order.ProductId,
        //    ProductName: order.Product,
        //    Price: order.Price,
        //}

        //$scope.Customer.Id = order.CustomerId;
        //$scope.Customer.Name = order.CustomerName;
        //$scope.Product.Id = order.ProductId;
        //$scope.Product.ProductName = order.Product;
        //$scope.Product.Price = order.Price;
    }

    $scope.delete = function (order) {
        httpOrderService.delete($scope, order);
    };

    $scope.reset = function () {
        $scope.Id = -1;
        var lastOrder = $scope.OrdersList[$scope.OrdersList.length - 1];
        $scope.OrderId = lastOrder.OrderId + 1;
        $scope.Date = new Date();
        $scope.Customer = {};
        $scope.Product = {};

        console.log("RESSETTEDDD");
    }
});

onboardapp.service('httpService', function ($http) {

    this.delete = function ($scope, customer) {
        $http({
            method: 'POST',
            url: '/Customer/DeleteConfirmed?id=' + customer.Id,
            data: customer.Id
        }).then(function successCallback(response) {
            $scope.CustomerList = $scope.CustomerList.filter(function (customer) {
                return customer.Id !== response.data.Id;
            });
        }, function errorCallback(response) {
        })
    };

    this.add = function ($scope) {

        $http.post('/Customer/Create', { name: $scope.Name, add1: $scope.Address1, add2: $scope.Address2, city: $scope.City }).then(function (res) {

            $scope.CustomerList.push(res.data);
        })
        // end of http post
    }

    //this.add = function ($scope) {
    //    $http({
    //        method: 'POST',
    //        url: '/Customer/Create?name=' + $scope.Name +
    //        '&add1=' + $scope.Address1 +
    //        '&add2=' + $scope.Address2 +
    //        '&city=' + $scope.City,
    //    }).then(function successCallback(response) {
    //        //need Id
    //        $scope.CustomerList.push(response.data);
    //    });
    //    // end of http post
    //};
    this.edit = function ($scope) {
        $http.post('/Customer/Edit',
            { id: $scope.Id, name: $scope.Name, add1: $scope.Address1, add2: $scope.Address2, city: $scope.City }
        ).then(function (response) {
            var index = $scope.CustomerList.findIndex(o => o.Id === $scope.Id);
            $scope.CustomerList[index].Name = $scope.Name;
            $scope.CustomerList[index].Address1 = $scope.Address1;
            $scope.CustomerList[index].Address2 = $scope.Address2;
            $scope.CustomerList[index].City = $scope.City;
            $scope.reset();
        })

    //this.edit = function ($scope) {
    //    $http({
    //        method: 'POST',
    //        url: '/Customer/Edit?id=' + $scope.Id +
    //        '&name=' + $scope.Name +
    //        '&add1=' + $scope.Address1 +
    //        '&add2=' + $scope.Address2 +
    //        '&city=' + $scope.City
    //    }).then(function successCallback(response) {
    //        var index = $scope.CustomerList.findIndex(o => o.Id === $scope.Id);
    //        $scope.CustomerList[index].Name = $scope.Name;
    //        $scope.CustomerList[index].Address1 = $scope.Address1;
    //        $scope.CustomerList[index].Address2 = $scope.Address2;
    //        $scope.CustomerList[index].City = $scope.City;
    //        $scope.reset();
    //    });

        this.reset = function () {
            $scope.Id = -1;
            $scope.Name = "";
            $scope.Address1 = "";
            $scope.Address2 = "";
            $scope.City = "";
        }
        // end of http post
    };

});

onboardapp.service('httpOrderService', function ($http) {

    this.add = function ($scope) {
        
        $http.post('/Orders/Create', { date: $scope.Date, customerid: $scope.Customer.Id, productid: $scope.Product.Id }).then(function (res) {
       
            $scope.convertDateTime(res.data);
            $scope.OrdersList.push(res.data);
        })
        // end of http post
    }

    // Convert "somedayz" date to query string
    this.formatDate = function (date) {
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        var hour = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();

        return day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;

    }



    this.edit = function ($scope) {
        $http.post('/Orders/Edit',
            { id: $scope.Id, orderid: $scope.OrderId, date: $scope.Date, customerid: $scope.Customer.Id, productid: $scope.Product.Id }
        ).then(function (response) {
            var index = $scope.OrdersList.findIndex(o => o.Id === $scope.Id);
            $scope.OrdersList[index].Id = $scope.Id;
            $scope.OrdersList[index].OrderId = $scope.OrderId;
            //$scope.OrdersList[index].Date = "/Date(" + Date.parse($scope.Date) + ")/";
            $scope.OrdersList[index].Date = $scope.Date;
            $scope.OrdersList[index].CustomerId = $scope.Customer.Id;
            $scope.OrdersList[index].CustomerName = $scope.Customer.Name;
            $scope.OrdersList[index].Product = $scope.Product.ProductName;
            $scope.OrdersList[index].ProductId = $scope.Product.Id;
            $scope.OrdersList[index].Price = $scope.Product.Price;
            console.log("BEFORE RESET");
            $scope.reset();
        })


        //$http({
        //    method: 'POST',
        //    url: '/Orders/Edit?id=' + $scope.Id +
        //    '&orderid=' + $scope.OrderId +
        //    '&date=' + this.formatDate($scope.Date) +
        //    '&customerid=' + $scope.Customer.Id +
        //    '&productid=' + $scope.Product.Id
        //}).then(function successCallback(response) {
        //    var index = $scope.OrdersList.findIndex(o => o.Id === $scope.Id);
        //    $scope.OrdersList[index].Id = $scope.Id;
        //    $scope.OrdersList[index].OrderId = $scope.OrderId;
        //    //$scope.OrdersList[index].Date = "/Date(" + Date.parse($scope.Date) + ")/";
        //    $scope.OrdersList[index].Date = $scope.Date;
        //    $scope.OrdersList[index].CustomerId = $scope.Customer.Id;
        //    $scope.OrdersList[index].CustomerName = $scope.Customer.Name;
        //    $scope.OrdersList[index].Product = $scope.Product.ProductName;
        //    $scope.OrdersList[index].ProductId = $scope.Product.ProductId;
        //    $scope.OrdersList[index].Price = $scope.Product.Price;
        //    console.log("BEFORE RESET");
        //    $scope.reset();
        //});

    }

    this.delete = function ($scope, order) {
        $http({
            method: 'POST',
            url: '/Orders/Delete?id=' + order.Id + '&orderId=' + order.OrderId,
        }).then(function successCallback(res) {
            console.log("res.data: " + res.data);
            console.log("res.Id: " + res.data.Id);
            //$scope.OrdersList = $scope.OrdersList.filter(function (order) {
            //    return order.Id !== res.data.Id;
            //});
            var index = $scope.OrdersList.findIndex(o => o.Id === res.data.Id);
            $scope.OrdersList.splice(index, 1);
        });
    }


});
