var app = angular.module('AppUserProduct', []);

// Khởi tạo controller loginCtrl
app.controller('userProductCtrl', function ($scope, $http) {
    $scope.listProduct = []; // Khởi tạo mảng sản phẩm
    $scope.productDetails = {}; // Lưu trữ thông tin chi tiết sản phẩm
    $scope.categories = []; // Danh sách danh mục
    $scope.colors = []; // Danh sách màu sắc
    $scope.sizes = []; // Danh sách size
    
    $scope.chunkedProducts = []; // Mảng lưu trữ các nhóm sản phẩm theo từng trang
    $scope.currentPage = 1; // Trang hiện tại
    $scope.itemsPerPage = 8; // Tổng số sản phẩm trên một trang
    $scope.productsPerRow = 4; // Số sản phẩm trên một dòngg
    $scope.latestProducts = []; //để chứa 4 sản phẩm mới nhất
    var current_url = "https://localhost:44366"; 


    // Hàm tạo URL ảnh
    $scope.getImageUrl = function (fileName) {
        return current_url + '/api/TuiXach/get-img/' + fileName;
    };
    $scope.viewProductDetails = function (product) {
        // Chuyển hướng đến trang chi tiết sản phẩm với mã sản phẩm trong URL
        window.location.href = '/user/details.html?maSp=' + product.maSp;
    };


    // Hàm thêm vào giỏ hàng
    $scope.addToCart = function () {
       
        // Lấy giỏ hàng hiện tại từ localStorage hoặc tạo mới nếu chưa có
        var cart = JSON.parse(localStorage.getItem(perID + '_cart')) || [];
    
        // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
        var existingProduct = cart.find(item => item.productId === $scope.productDetails.productId);
        if (existingProduct) {
            existingProduct.quantity += 1; // Nếu có thì tăng số lượng
        } else {
            // Thêm sản phẩm mới vào giỏ hàng
            cart.push({
                name: $scope.productDetails.tenSp,
                price: $scope.productDetails.giaBan,
                quantity: 1,
                image: $scope.productDetails.hinhAnh
            });
        }
    
        // Lưu giỏ hàng vào localStorage
        localStorage.setItem(perID + '_cart', JSON.stringify(cart));
        alert('Sản phẩm đã được thêm vào giỏ hàng!');
    };
    $scope.loadCart = function () {
        var perID = localStorage.getItem('perID'); // Lấy ID người dùng từ localStorage
        if (!perID) {
            alert('Bạn chưa đăng nhập');
            return;
        }
    
        // Lấy giỏ hàng từ localStorage
        var cart = JSON.parse(localStorage.getItem(perID + '_cart')) || [];
        $scope.cart = cart;
    
        // Tính tổng số lượng và tổng giá
        $scope.totalQuantity = cart.reduce((sum, item) => sum + item.quantity, 0);
        $scope.totalPrice = cart.reduce((sum, item) => sum + (item.quantity * item.price), 0);
    };
    
    
    $scope.LoadProduct = function() {
        $http({
            method: 'GET',
            url: current_url + '/api/TuiXach/get-all',
        }).then(function (response){
            $scope.listProduct = response.data;  
            console.log("Danh sách sản phẩm:", $scope.listProduct);
           
         // Chia nhỏ mảng sản phẩm theo hàng và trang
         $scope.preparePaginatedProducts();

        });
    };

    // Hàm lấy chi tiết sản phẩm theo mã sản phẩm
    $scope.ProductDetails = function () {
        // Lấy mã sản phẩm từ URL
        const urlParams = new URLSearchParams(window.location.search);
        const maSp = urlParams.get('maSp');
        console.log("maSp:", maSp);
    
        if (maSp) {
            // Gọi API lấy thông tin chi tiết sản phẩm
            $http({
                method: 'GET',
                url: current_url + '/api/TuiXach/get-by-id/' + maSp,
            }).then(function (response) {
                $scope.productDetails = response.data.tuiXach;
                console.log("Chi tiết sản phẩm:", $scope.productDetails);
           
            }).catch(function (error) {
                console.error("Lỗi khi gọi API:", error);
            });
        } else {
            console.error("Không tìm thấy maSp trong URL.");
        }
    };

    // Hàm lấy sản phẩm gần nhất (chỉ lấy 4 sản phẩm mới nhất)
    $scope.LoadLatestProducts = function () {
        $http({
            method: 'GET',
            url: current_url + '/api/TuiXach/get-all',
        }).then(function (response) {
            $scope.listProduct = response.data;
            // Trộn danh sách sản phẩm ngẫu nhiên
        let shuffledProducts = $scope.listProduct.sort(function() { return 0.5 - Math.random(); });

        // Lấy 4 sản phẩm ngẫu nhiên
        $scope.randomProducts = shuffledProducts.slice(0, 4);

        console.log("Danh sách 4 sản phẩm ngẫu nhiên:", $scope.randomProducts);
        });
    };

    // Chuẩn bị danh sách sản phẩm theo từng trang và từng hàng
    $scope.preparePaginatedProducts = function () {
        let paginated = [];
        let totalItems = $scope.listProduct.length;

        // Chia nhỏ sản phẩm theo trang
        for (let i = 0; i < totalItems; i += $scope.itemsPerPage) {
            let pageProducts = $scope.listProduct.slice(i, i + $scope.itemsPerPage);

            // Chia nhỏ mỗi trang thành các hàng (4 sản phẩm một hàng)
            let rows = [];
            for (let j = 0; j < pageProducts.length; j += $scope.productsPerRow) {
                rows.push(pageProducts.slice(j, j + $scope.productsPerRow));
            }
            paginated.push(rows);
        }
        $scope.paginatedProductsList = paginated;
    };

    // Hàm trả về danh sách sản phẩm của trang hiện tại
    $scope.paginatedProducts = function () {
        return $scope.paginatedProductsList[$scope.currentPage - 1] || [];
    };

    // Hàm tính tổng số trang
    $scope.totalPages = function () {
        return Math.ceil($scope.listProduct.length / $scope.itemsPerPage);
    };

    // Chuyển đến trang cụ thể
    $scope.setPage = function (page) {
        if (page >= 1 && page <= $scope.totalPages()) {
            $scope.currentPage = page;
        }
    };


     // Lấy danh sách danh mục từ API
    $scope.loadCategories = function () {
        $http({
            method: 'GET',
            url: current_url + '/api/DanhMucSanPham/all',
        }).then(function (response){
                $scope.categories = response.data; 
                console.log("Danh sách danh mục:", $scope.categories);
                $scope.categories = chunkArray(response.data, 3);
        });
    };
    // Hàm chia mảng thành các nhóm
    function chunkArray(array, chunkSize) {
        let results = [];
        for (let i = 0; i < array.length; i += chunkSize) {
            results.push(array.slice(i, i + chunkSize));
        }
        return results;
    }


     // Lấy bảng màu từ API
    $scope.loadColor = function () {
        $http({
            method: 'GET',
            url: current_url + '/api/MauSac/all',
        }).then(function (response){
                $scope.colors = response.data; 
                console.log("Mausac:", $scope.colors);
                $scope.colors = chunkArray(response.data, 3); // Chia danh sách màu thành nhóm 3
                console.log($scope.colors); // Dữ liệu màu từ API
                console.log($scope.colorMap); // Bảng ánh xạ tên màu và mã màu

            });
        };
    // Hàm chia mảng thành các nhóm
    function chunkArray(array, chunkSize) {
        let results = [];
        for (let i = 0; i < array.length; i += chunkSize) {
            results.push(array.slice(i, i + chunkSize));
        }
        return results;
    }
    $scope.colorMap = {
        Beige: "#f5f5dc",
        Black: "#000000",
        Brown: "#a52a2a",
        Bule: "#0000ff",
        Gold: "#ecdb7c",
        Green: "#008000",
        Pink: "#FFC0CB",
        Red: "#FF0000",
        White: "#FFFFFF"
    };
    

      // Lấy bảng size từ API
      $scope.loadSize = function () { 
        $http({
            method: 'GET',
            url: current_url + '/api/Size/get-all',
        }).then(function (response){
                $scope.sizes = response.data; 
                console.log("Bảng size:", $scope.sizes);
            });
    };



});
