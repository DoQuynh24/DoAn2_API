
var app = angular.module('AppLogin', []);

// Khởi tạo controller loginCtrl
app.controller('loginCtrl', function ($scope, $http) {
    var current_url = "https://localhost:44367"; 


    // Hàm tải thông tin người dùng
     $scope.loadUserInfo = function () {
        const perId = localStorage.getItem("PerID"); 
        if (!perId) {
            alert("Vui lòng đăng nhập lại!");
            return;
        }

        $http({
            method: 'GET',
            url: `${current_url}/api/User/get-user/${perId}`,
            headers: {
                'Content-Type': 'application/json',
                
            }
        }).then(function (response) {
            console.log(response);
            const user = response.data.user;
            // Gán thông tin người dùng vào các biến trong scope
            $scope.taikhoan = user.taiKhoan;
            $scope.matkhau = user.matKhau;
            $scope.hoten = user.hoTen;
            $scope.gioitinh = user.gioiTinh;
            $scope.diachi = user.diaChi;
            $scope.ngaysinh = user.ngaySinh ? new Date(user.ngaySinh) : null; 
            $scope.role = user.role;
        }).catch(function (error) {
            console.error("Lỗi khi tải thông tin người dùng:", error.data);
            alert("Không thể tải thông tin người dùng. Vui lòng đăng nhập lại!");
        });
    };

    // Hàm lưu thông tin người dùng sau khi thay đổi
    $scope.saveUser = function () {
        // Kiểm tra thông tin nhập vào
    if (!$scope.hoten || !$scope.diachi || !$scope.gioitinh || !$scope.ngaysinh) {
        alert("Vui lòng điền đầy đủ thông tin!");
        return;
    }

    const perId = localStorage.getItem("PerID"); // Lấy PerID từ localStorage
    if (!perId) {
        alert("Vui lòng đăng nhập lại!");
        console.log("PerID:", perId);
        return;
    }

    // Chuẩn bị dữ liệu để gửi lên API
    var formData = new FormData();
    
    formData.append("PerID", perId); // PerID lấy từ localStorage
    formData.append("TaiKhoan", $scope.taikhoan); // Tài khoản readonly
    formData.append("MatKhau", $scope.matkhau); // Mật khẩu có thể để trống nếu không thay đổi
    formData.append("HoTen", $scope.hoten);
    const localDate = new Date($scope.ngaysinh);
    localDate.setMinutes(localDate.getMinutes() - localDate.getTimezoneOffset());
    formData.append("NgaySinh", localDate.toISOString().split('T')[0]);
    formData.append("GioiTinh", $scope.gioitinh);
    formData.append("DiaChi", $scope.diachi);

    $http({
        method: 'PUT',
        url: `${current_url}/api/User/update-user`, // URL API cập nhật
        headers: {
            'Content-Type': undefined // Gửi dữ liệu dưới dạng FormData
        },
        data: formData
    }).then(function (response) {
        if (response.data && response.data.success) {
            alert("Cập nhật thông tin thành công!");
            // Có thể reload hoặc cập nhật giao diện tùy ý
            $scope.loadUserInfo(); // Gọi lại hàm tải thông tin
        } else {
            alert("Không thể cập nhật thông tin. Vui lòng thử lại sau!");
        }
    }).catch(function (error) {
        console.error("Lỗi khi cập nhật thông tin:", error);
        alert("Đã xảy ra lỗi khi cập nhật thông tin. Vui lòng thử lại sau!");
    });
    };
    
    // Gọi hàm loadUserInfo để tải thông tin khi trang được tải
    $scope.loadUserInfo();
    

 // Lấy HoTen từ localStorage khi tải trang
    $scope.HoTen = localStorage.getItem("HoTen");
    $scope.TaiKhoan = localStorage.getItem("TaiKhoan");
    $scope.login = function () {
        // Lấy dữ liệu từ input
        var current_url = "https://localhost:44366"; 
        var username = document.getElementById('username').value;
        var password = document.getElementById('password').value;
      

        // Kiểm tra dữ liệu nhập vào
        if (!username || !password) {
            $scope.errorMessage = "Vui lòng nhập đầy đủ thông tin!";
            return;
        }

        $http({
            method: 'POST',
            url: current_url + '/api/User/login',
            data: { 
                TaiKhoan: username,  
                MatKhau: password    
            },
            headers: {
                'Content-Type': 'application/json'
                
            }
        }).then(function (response) {
            console.log(response.data);
            // Kiểm tra token và role trong response
            if (response.data && response.data.token && response.data.user_id && response.data.hoten) {
                alert("Đăng nhập thành công!");
                // Lưu token vào localStorage
                localStorage.setItem("token", response.data.token);
                localStorage.setItem("PerID", response.data.user_id);
                localStorage.setItem("HoTen", response.data.hoten);
                localStorage.setItem("TaiKhoan", response.data.taikhoan);

                $scope.HoTen = response.data.hoten;
                $scope.TaiKhoan = response.data.taikhoan;

                //Kiểm tra Role
                if (response.data.role === "Admin") {
                    // Chuyển hướng đến trang admin
                    window.location.href = "http://127.0.0.1:5500/admin.html";
                } else if (response.data.role === "Khách hàng") {
                    // Chuyển hướng đến trang khách hàng
                    window.location.href = "/user/index.html";
                } else {
                    $scope.errorMessage = "Không thể xác định quyền truy cập!";
                    console.error("Role không hợp lệ:", response.data.role);
                }
            } else {
                $scope.errorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
                console.error("Dữ liệu không hợp lệ:", response.data);
            }
        }).catch(function (error) {
            // Xử lý lỗi
            console.error("Đã xảy ra lỗi khi đăng nhập:", error);
            if (error.status === -1) {
                $scope.errorMessage = "Không thể kết nối tới máy chủ. Vui lòng kiểm tra lại!";
            } else {
                $scope.errorMessage = "Lỗi máy chủ. Vui lòng thử lại sau!";
            }
        });        
    };

     
    $scope.registerUser = function () {
        // Kiểm tra các trường bắt buộc
        if (!$scope.taikhoan || !$scope.matkhau || !$scope.hoten ) {
            alert("Vui lòng điền đầy đủ thông tin.");
            return;
        }
        
    
        // Kiểm tra đồng ý với các điều khoản
        if (!$scope.agree) {
            $scope.errorMessage = "Bạn cần đồng ý với Điều khoản và Chính sách Quyền riêng tư!";
            return;
        }
    
        var formData = new FormData();
        formData.append("TaiKhoan", $scope.taikhoan);
        formData.append("MatKhau", $scope.matkhau);
        formData.append("HoTen", $scope.hoten);
    
        // Gửi yêu cầu POST đến API
        $http({
            method: 'POST',
            url: current_url + '/api/User/create-user',
            headers: { 'Content-Type': undefined },
            data: formData
        }).then(function (response) {
            // Kiểm tra kết quả trả về từ API
            if (response.data.success) {
                alert("Đăng ký thành công! Bạn có thể đăng nhập");
            
                window.location.href = "/user/login.html";
                
            } else {
                $scope.errorMessage = "Đăng ký không thành công. Vui lòng thử lại!";
            }
        }).catch(function (error) {
            console.error("Đã xảy ra lỗi khi đăng ký:", error);
            $scope.errorMessage = "Lỗi máy chủ. Vui lòng thử lại sau!";
        });
    };
    
    
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
    var current_url = "https://localhost:44367"; 


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
        const taikhoan = localStorage.getItem('TaiKhoan'); 
        if (!taikhoan) {
            alert('Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng!');
            return;
        }
    
        // Lấy giỏ hàng hiện tại từ localStorage
        const cartKey = `${taikhoan}_cart`;
        let cart = JSON.parse(localStorage.getItem(cartKey)) || [];
    
        // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
        const existingProduct = cart.find(item => item.productId === $scope.productDetails.productId);
        if (existingProduct) {
            existingProduct.quantity += 1; // Nếu có thì tăng số lượng
        } else {
            // Thêm sản phẩm mới vào giỏ hàng
            cart.push({
                productId: $scope.productDetails.productId,
                name: $scope.productDetails.tenSp,
                price: $scope.productDetails.giaBan,
                quantity: 1,
                image: $scope.productDetails.hinhAnh
            });
        }
    
        // Lưu giỏ hàng vào localStorage
        localStorage.setItem(cartKey, JSON.stringify(cart));
        alert('Sản phẩm đã được thêm vào giỏ hàng!');
    };
    
    $scope.loadCart = function () {
        var taikhoan = localStorage.getItem('TaiKhoan'); 
       
    
        // Lấy giỏ hàng từ localStorage
        var cart = JSON.parse(localStorage.getItem(taikhoan + '_cart')) || [];
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
