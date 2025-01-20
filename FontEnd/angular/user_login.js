
var app = angular.module('AppLogin', []);
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
    
    formData.append("PerID", perId); 
    formData.append("TaiKhoan", $scope.taikhoan); 
    formData.append("MatKhau", $scope.matkhau);
    formData.append("HoTen", $scope.hoten);
    const localDate = new Date($scope.ngaysinh);
    localDate.setMinutes(localDate.getMinutes() - localDate.getTimezoneOffset());
    formData.append("NgaySinh", localDate.toISOString().split('T')[0]);
    formData.append("GioiTinh", $scope.gioitinh);
    formData.append("DiaChi", $scope.diachi);

    $http({
        method: 'PUT',
        url: `${current_url}/api/User/update-user`, 
        headers: {
            'Content-Type': undefined 
        },
        data: formData
    }).then(function (response) {
        if (response.data && response.data.success) {
            alert("Cập nhật thông tin thành công!");

            $scope.loadUserInfo();
        } else {
            alert("Không thể cập nhật thông tin. Vui lòng thử lại sau!");
        }
    }).catch(function (error) {
        console.error("Lỗi khi cập nhật thông tin:", error);
        alert("Đã xảy ra lỗi khi cập nhật thông tin. Vui lòng thử lại sau!");
    });
    };
    
    $scope.loadUserInfo();
    

    $scope.HoTen = localStorage.getItem("HoTen");
    $scope.TaiKhoan = localStorage.getItem("TaiKhoan");
    $scope.login = function () {
        // Lấy dữ liệu từ input
        var current_url = "https://localhost:44366"; 
        var username = document.getElementById('username').value;
        var password = document.getElementById('password').value;
      


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
       
            if (response.data && response.data.token && response.data.user_id && response.data.hoten) {
                alert("Đăng nhập thành công!");
            
                localStorage.setItem("token", response.data.token);
                localStorage.setItem("PerID", response.data.user_id);
                localStorage.setItem("HoTen", response.data.hoten);
                localStorage.setItem("TaiKhoan", response.data.taikhoan);

                $scope.HoTen = response.data.hoten;
                $scope.TaiKhoan = response.data.taikhoan;

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
            console.error("Đã xảy ra lỗi khi đăng nhập:", error);
            if (error.status === -1) {
                $scope.errorMessage = "Không thể kết nối tới máy chủ. Vui lòng kiểm tra lại!";
            } else {
                $scope.errorMessage = "Lỗi máy chủ. Vui lòng thử lại sau!";
            }
        });        
    };

     
    $scope.registerUser = function () {
      
        if (!$scope.taikhoan || !$scope.matkhau || !$scope.hoten ) {
            alert("Vui lòng điền đầy đủ thông tin.");
            return;
        }
        

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

    $scope.tongTienHang = 0;
    $scope.phiVanChuyen = 25000;
    $scope.selectedStatus = 'Tất cả';

    $scope.loadHoaDon = function (trangThai) {
        $scope.selectedStatus = trangThai; // Cập nhật trạng thái
        var perID = localStorage.getItem("PerID");
        var url = `https://localhost:44367/api/HoaDon/get-by-perid/${perID}`;

        // Thêm tham số lọc trạng thái vào URL nếu cần
        if (trangThai && trangThai !== 'Tất cả') {
            url += `?trangThai=${trangThai}`;
        }

        $http({
            method: 'GET',
            url: url,
            headers: { 'Content-Type': 'application/json' }
        }).then(function (response) {
            console.log(response.data);
            $scope.hoaDons = response.data; // Danh sách hóa đơn đầy đủ
            $scope.filteredHoaDons = $scope.hoaDons; // Lưu danh sách gốc để lọc

            // Tính tổng tiền hàng cho từng hóa đơn
            $scope.filteredHoaDons.forEach(hoaDon => {
                hoaDon.tongTienHang = hoaDon.chiTietHoaDons.reduce((total, item) => {
                    return total + (item.giaBan * item.soLuong);
                }, 0);
                $scope.calculateThanhTien(hoaDon);
            });
        }).catch(function (error) {
            console.error("Đã xảy ra lỗi khi tải hóa đơn:", error);
        });
    };
    $scope.viewOrderDetails = function (hoaDon) {
        $scope.selectedOrder = hoaDon; // Lưu hóa đơn được chọn vào $scope
    };
    
    // Tính tổng tiền thanh toán
    $scope.calculateThanhTien = function (hoaDon) {
        hoaDon.thanhTien = hoaDon.tongTienHang + $scope.phiVanChuyen;
    };

    // Lọc hóa đơn theo trạng thái
    $scope.filterHoaDonByStatus = function (status) {
        if (status === 'Tất cả') {
            $scope.filteredHoaDons = $scope.hoaDons;
        } else {
            $scope.filteredHoaDons = $scope.hoaDons.filter(hoaDon => hoaDon.trangThai === status);
        }
        $scope.selectedStatus = status; // Đánh dấu trạng thái đã chọn
    };

    // Tìm kiếm hóa đơn theo ID đơn hàng hoặc Tên sản phẩm
    $scope.searchHoaDon = function (query) {
        $scope.filteredHoaDons = $scope.hoaDons.filter(hoaDon => {
            return hoaDon.MaHD.toString().includes(query) || 
                hoaDon.ChiTietHoaDons.some(item => item.tenSp.toLowerCase().includes(query.toLowerCase()));
        });
    };

    $scope.loadHoaDon('Tất cả');
    // Hủy đơn hàng
    $scope.cancelOrder = function(hoaDon) {
        if (confirm("Bạn có chắc chắn muốn hủy đơn hàng này?")) {
            hoaDon.trangThai = 'Đã hủy'; // Cập nhật trạng thái đơn hàng trong giao diện
            $http({
                method: 'PUT',
                url: `https://localhost:44367/api/HoaDon/update-trang-thai/${hoaDon.maHD}`,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify('Đã hủy')
            }).then(function(response) {
                alert("Đơn hàng đã được hủy thành công!");
            }).catch(function(error) {
                console.error("Lỗi khi hủy đơn hàng:", error);
            });
        }
    };

    // Xác nhận đã nhận hàng
    $scope.confirmReceived = function(hoaDon) {
        if (confirm("Bạn có chắc chắn đã nhận được hàng?")) {
            hoaDon.trangThai = 'Hoàn thành'; // Cập nhật trạng thái đơn hàng trong giao diện
            $http({
                method: 'PUT',
                url: `https://localhost:44367/api/HoaDon/update-trang-thai/${hoaDon.maHD}`,
                headers: { 'Content-Type': 'application/json' },
                data: JSON.stringify('Hoàn thành')
            }).then(function(response) {
                alert("Đơn hàng đã được xác nhận là hoàn thành!");
            }).catch(function(error) {
                console.error("Lỗi khi xác nhận đã nhận hàng:", error);
            });
        }
    };

    // Đánh giá đơn hàng
    $scope.rateOrder = function (hoaDon) {
        $scope.selectedOrder = hoaDon; // Lưu hóa đơn được chọn để đánh giá
        $scope.ratingData = {
            perID: localStorage.getItem("PerID"),
            maSp: '',
            noiDung: ''
        };
    
        // Hiển thị modal đánh giá
        $('#rateOrderModal').modal('show');
    };
    
    // Gửi đánh giá sản phẩm
    $scope.submitRating = function () {
        if (!$scope.ratingData.maSp || !$scope.ratingData.noiDung) {
            alert("Vui lòng chọn sản phẩm và nhập nội dung đánh giá.");
            return;
        }
    
        // Tạo dữ liệu đánh giá
        const data = {
            perID: $scope.ratingData.perID,
            maSp: $scope.ratingData.maSp,
            noiDung: $scope.ratingData.noiDung,
            ngayBinhLuan: new Date().toISOString() 
        };
    
        $http({
            method: 'POST',
            url: 'https://localhost:44367/api/BinhLuan/create',
            headers: { 'Content-Type': 'application/json' },
            data: JSON.stringify(data)
        }).then(function (response) {
            alert("Đánh giá sản phẩm thành công!");
            $('#rateOrderModal').modal('hide'); // Đóng modal
            $scope.ratingData.noiDung = ''; // Xóa nội dung sau khi đánh giá
        }).catch(function (error) {
            console.error("Lỗi khi gửi đánh giá:", error);
            alert("Không thể gửi đánh giá. Vui lòng thử lại sau.");
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


    $scope.getImageUrl = function (fileName) {
        return current_url + '/api/TuiXach/get-img/' + fileName;
    };
    $scope.viewProductDetails = function (product) {
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
        const existingProduct = cart.find(item => item.maSp === $scope.productDetails.maSp);

        if (existingProduct) {
            existingProduct.quantity += 1; 
        } else {
           
            cart.push({
                maSp: $scope.productDetails.maSp, 
                tenSp: $scope.productDetails.tenSp,
                giaBan: $scope.productDetails.giaBan,
                quantity: 1,
                hinhAnh: $scope.productDetails.hinhAnh,
                tenMau: $scope.productDetails.tenMau,
                maSize: $scope.productDetails.maSize,
                khuyenMai: $scope.productDetails.khuyenMai
            });
        }

        // Lưu giỏ hàng vào localStorage
        localStorage.setItem(cartKey, JSON.stringify(cart));

        // Hiển thị thông báo
        alert('Sản phẩm đã được thêm vào giỏ hàng!');
    };

    // Hàm tải giỏ hàng từ localStorage
    $scope.loadCart = function () {
        const taikhoan = localStorage.getItem('TaiKhoan'); 
        const cartKey = `${taikhoan}_cart`;
        $scope.cart = JSON.parse(localStorage.getItem(cartKey)) || [];

        // Cập nhật tổng số lượng và tổng giá
        $scope.updateCartSummary = function () {
            let totalQuantity = 0;
            let totalPrice = 0;
        
            // Duyệt qua tất cả sản phẩm trong giỏ hàng
            $scope.cart.forEach(item => {
                totalQuantity += item.quantity; // Cộng tổng số lượng
                totalPrice += item.quantity * item.giaBan; // Cộng tổng tiền
            });
        
            // Hiển thị tổng số lượng và tổng tiền lên giao diện
            document.getElementById('total-quantity').textContent = totalQuantity;
            document.getElementById('total-price').textContent = totalPrice.toLocaleString(); // Tổng tiền chưa dùng
            document.getElementById('total').textContent = totalPrice.toLocaleString(); // Gán tổng tất cả dòng "tổng tiền"
        };
        
        $scope.updateCartSummary(); // Cập nhật khi load
        $scope.calculateTotal(); 
    };
    $scope.calculateTotal = function () {
        let totalAmount = 0;
    
        // Duyệt qua từng sản phẩm trong giỏ hàng để tính tổng tiền
        $scope.cart.forEach(item => {
            totalAmount += item.giaBan * item.quantity; // Tổng tiền của sản phẩm
        });
    
        return totalAmount; // Trả về tổng thành tiền
    };
    
    $scope.removeItem = function (item) {
        const index = $scope.cart.indexOf(item);
        if (index > -1) {
            $scope.cart.splice(index, 1);
        }
        $scope.updateCartSummary();
    };
    $scope.increaseQuantity = function (item) {
        item.quantity += 1;
        $scope.updateCartSummary(); // Cập nhật tổng số lượng và giá
    };
    
    $scope.decreaseQuantity = function (item) {
        if (item.quantity > 1) {
            item.quantity -= 1;
            $scope.updateCartSummary(); // Cập nhật tổng số lượng và giá
        }
    };
    $scope.goToCheckout = function () {
        const taikhoan = localStorage.getItem('TaiKhoan');
        if (!taikhoan) {
            alert('Vui lòng đăng nhập để tiếp tục!');
            return;
        }
        const cartKey = `${taikhoan}_cart`;
        let cart = JSON.parse(localStorage.getItem(cartKey)) || [];
        if (cart.length === 0) {
            alert('Giỏ hàng trống, vui lòng thêm sản phẩm!');
            return;
        }
        window.location.href = '/user/checkout.html'; // Chuyển đến trang checkout
    };
    
    $scope.calculateTotalQuantity = function () {
        const taikhoan = localStorage.getItem('TaiKhoan');
        const cartKey = `${taikhoan}_cart`;
        const cart = JSON.parse(localStorage.getItem(cartKey)) || [];
        return cart.reduce((total, item) => total + item.quantity, 0);
    };
    
    $scope.calculateTotalPrice = function () {
        const taikhoan = localStorage.getItem('TaiKhoan');
        const cartKey = `${taikhoan}_cart`;
        const cart = JSON.parse(localStorage.getItem(cartKey)) || [];
        return cart.reduce((total, item) => total + item.quantity * item.giaBan, 0);
    };
    
    $scope.placeOrder = function () {
        const taikhoan = localStorage.getItem('TaiKhoan');
        const perId = localStorage.getItem("PerID");
        const cartKey = `${taikhoan}_cart`;
        const cart = JSON.parse(localStorage.getItem(cartKey)) || [];
    
        if (!cart.length) {
            alert('Giỏ hàng trống!');
            return;
        }
         // Lấy thông tin từ các dropdown
        const provinceElement = document.getElementById('province');
        const districtElement = document.getElementById('district');
        const provinceName = provinceElement.options[provinceElement.selectedIndex]?.text || '';
        const districtName = districtElement.options[districtElement.selectedIndex]?.text || '';
        // Thông tin thanh toán
        const orderData = {
            PerID: perId, // Tài khoản người dùng
            HoTen: document.getElementById('first-name').value,
            DiaChi: `${document.getElementById('address').value}, ${districtName}, ${provinceName}`, // Kết hợp địa chỉ cụ thể
            SDT: document.getElementById('phone').value,
            TrangThai: 'Đang xử lý',
            ChiTietHoaDons: cart.map(item => ({
                MaSp: item.maSp,
                TenSp: item.tenSp,
                TenMau: item.tenMau,
                MaSize: item.maSize,
                SoLuong: item.quantity,
                GiaBan: item.giaBan,
                GhiChu: '',
                KhuyenMai: item.khuyenMai
            }))
        };
    
        // Gửi yêu cầu API
        $http.post(current_url + '/api/HoaDon/Create', orderData)
            .then(response => {
                if (response.data) {
                    alert('Đặt hàng thành công!');
                    localStorage.removeItem(cartKey); // Xóa giỏ hàng sau khi đặt hàng
                    window.location.href = '/user/cart.html';
                }
            })
            .catch(error => {
                console.error(error);
                alert('Đặt hàng thất bại, vui lòng thử lại!');
            });
    };

    $scope.isEditable = function (trangThai) {
        return trangThai === 'Đang xử lý';  // Kiểm tra xem trạng thái đơn hàng có cho phép chỉnh sửa không
    };
    
    $scope.isEditing = false;  // Biến để theo dõi trạng thái chỉnh sửa
    
    // Hàm để kích hoạt chế độ chỉnh sửa
    $scope.editOrder = function () {
        if ($scope.isEditable($scope.selectedOrder.trangThai)) {
            $scope.isEditing = true;
        } else {
            alert('Chỉ có thể chỉnh sửa khi đơn hàng ở trạng thái "Đang xử lý".');
        }
    };
    
    // Hàm lưu thay đổi sau khi chỉnh sửa
    $scope.saveOrder = function () {
        const updateData = {
            MaHD: $scope.selectedOrder.maHD,
            HoTen: $scope.selectedOrder.hoTen,
            DiaChi: $scope.selectedOrder.diaChi,
            SDT: $scope.selectedOrder.sdt
        };
    
        $http.put(current_url + '/api/HoaDon/Update', updateData)
            .then(response => {
                alert('Cập nhật thông tin hóa đơn thành công!');
                $scope.isEditing = false; 
            })
            .catch(error => {
                alert(error.data?.Message || 'Lỗi khi cập nhật thông tin đơn hàng!');
            });
    };
    
    // Hàm hủy bỏ chỉnh sửa
    $scope.cancelEdit = function () {
        $scope.isEditing = false;
    };
    

    
    $scope.LoadProduct = function() {
        $http({
            method: 'GET',
            url: 'https://localhost:44374/api/TuiXach/get-all',
        }).then(function(response) {
            $scope.listProduct = response.data;
            $scope.filteredProducts = $scope.listProduct; // Khởi tạo danh sách sản phẩm đã lọc
            console.log("Danh sách sản phẩm:", $scope.listProduct);
    
            // Chia nhỏ mảng sản phẩm theo hàng và trang
            $scope.preparePaginatedProducts();
        });
    };

    // Hàm lấy chi tiết sản phẩm theo mã sản phẩm
    $scope.ProductDetails = function () {
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
    $scope.preparePaginatedProducts = function() {
        let paginated = [];
        let totalItems = $scope.filteredProducts.length; // Sử dụng danh sách đã lọc
    
        for (let i = 0; i < totalItems; i += $scope.itemsPerPage) {
            let pageProducts = $scope.filteredProducts.slice(i, i + $scope.itemsPerPage);
    
            // Chia nhỏ mỗi trang thành các hàng
            let rows = [];
            for (let j = 0; j < pageProducts.length; j += $scope.productsPerRow) {
                let row = pageProducts.slice(j, j + $scope.productsPerRow);
    
                // Thêm placeholder nếu số lượng sản phẩm trong hàng ít hơn $scope.productsPerRow
                while (row.length < $scope.productsPerRow) {
                    row.push({ placeholder: true }); // Thêm đối tượng rỗng
                }
    
                rows.push(row);
            }
            paginated.push(rows);
        }
        $scope.paginatedProductsList = paginated;
    };
    
    $scope.paginatedProducts = function() {
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

    $scope.filterProducts = function() {
        if (!$scope.searchQuery || $scope.searchQuery.trim() === '') {
            $scope.filteredProducts = $scope.listProduct; // Nếu không có từ khóa tìm kiếm
        } else {
            const query = $scope.searchQuery.toLowerCase();
            $scope.filteredProducts = $scope.listProduct.filter(function(product) {
                return (
                    (product.tenSp && product.tenSp.toLowerCase().includes(query)) || // Tìm kiếm theo tên sản phẩm
                    (product.moTa && product.moTa.toLowerCase().includes(query))     // Tìm kiếm theo mô tả sản phẩm
                );
            });
        }
    
        // Làm mới danh sách phân trang
        $scope.preparePaginatedProducts();
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
