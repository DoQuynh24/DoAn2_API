app.controller('ordersCtrl', function($scope, $http,) { 
    $scope.listOrder = []; 
    
    $scope.newOrder = {};
    $scope.currentOrder = {}; 
    $scope.isEditMode = false;
    $scope.selectedOrder = {}; 
    $scope.listChiTiet = []; 
    $scope.newChiTiet = {}; // Chứa thông tin sản phẩm mới
    $scope.page = 1;
    $scope.itemsPerPage = 6;
    $scope.totalPages = 1;
    $scope.searchQuery = { tenncc: '' };

    // URL API
    var current_url = "https://localhost:44366"; 
    //xử lý giá
    
    $scope.parseCurrency = function (value) {
    if (!value) return 0; 
    value = value.toString().replace(/\./g, ''); // Loại bỏ dấu chấm
    let parsed = parseInt(value, 10);
    return isNaN(parsed) ? 0 : parsed; // Trả về 0 nếu không hợp lệ
};

    

    $scope.LoadOrder = function() {
        $http({
            method: 'GET',
            url: current_url + '/api/DonHangNhap/get-all',
            params: {
                page: $scope.page,
                itemsPerPage: $scope.itemsPerPage,
                tenNCC: $scope.searchQuery.tenncc, 
            }
        }).then(function(response) {
            $scope.listOrder = response.data;
            $scope.totalPages = Math.ceil(response.data.total / $scope.itemsPerPage);
            createPagination($scope.totalPages);  
            
        });
    };
    // Xem đơn hàng nhập (hàm khi nhấn nút see-btn)
    $scope.LoadlistCTDHN = function (donHangNhap) {
        $scope.isEditMode = true; // Chế độ chỉnh sửa
        $scope.selectedOrder = angular.copy(donHangNhap); 
        $scope.selectedOrder.ChiTietDonHangNhapsToDelete = []; // Khởi tạo danh sách xóa
         // Chuyển đổi ngayNhap thành kiểu Date
        if ($scope.selectedOrder.ngayNhap) {
            $scope.selectedOrder.ngayNhap = new Date($scope.selectedOrder.ngayNhap);
        }
        
        $http({
            method: 'GET',
            url: current_url + '/api/DonHangNhap/get/' + donHangNhap.maDHN,
        }).then(function (response) {
            if (response.data) {
                $scope.listChiTiet  = response.data.chiTietDonHangNhaps || [];
            } else {
                console.error("Dữ liệu chi tiết đơn hàng không hợp lệ:");
            }
            // Hiển thị modal
            var orderModal = new bootstrap.Modal(document.getElementById('orderModal'), {});
            orderModal.show();
        
        }).catch(function (error) {
            console.error('Lỗi khi tải chi tiết đơn hàng nhập:', error);
        });
    };

    $scope.addChiTiet = function () {
        // Kiểm tra dữ liệu
        if (!$scope.newChiTiet.tenSp || !$scope.newChiTiet.maSp || !$scope.newChiTiet.giaNhap || !$scope.newChiTiet.soLuong) {
            alert("Vui lòng nhập đầy đủ thông tin sản phẩm!");
            return;
        }
        $scope.newChiTiet.giaNhap = $scope.parseCurrency($scope.newChiTiet.giaNhap);
        $scope.listChiTiet.push(angular.copy($scope.newChiTiet));
        $scope.newChiTiet = {};
    };

   
    
    $scope.deleteChiTiet = function (ct) {
        if (!$scope.selectedOrder.ChiTietDonHangNhapsToDelete) {
            $scope.selectedOrder.ChiTietDonHangNhapsToDelete = [];
        }
        $scope.selectedOrder.ChiTietDonHangNhapsToDelete.push(ct.maSp);
    
        $scope.listChiTiet = $scope.listChiTiet.filter(function (item) {
            return item !== ct;
        });
    };
    
    
  

    $scope.saveOrder = function () {
        // Kiểm tra thông tin
        if (!$scope.selectedOrder.tenNCC || $scope.listChiTiet.length === 0) {
            alert("Thông tin không đầy đủ hoặc danh sách sản phẩm rỗng!");
            return;
        }
        // Định dạng ngày
        let ngayNhap = $scope.selectedOrder.ngayNhap ? new Date($scope.selectedOrder.ngayNhap).toISOString() : null;
    
        // Tạo dữ liệu payload
        let payload = {
            maDHN: $scope.selectedOrder.maDHN,
            ngayNhap: ngayNhap,
            tenNCC: $scope.selectedOrder.tenNCC,
            chiTietDonHangNhaps: $scope.listChiTiet.map(ct => ({
                maSp: ct.maSp,
                tenSp: ct.tenSp,
                giaNhap: $scope.parseCurrency(ct.giaNhap),
                soLuong: ct.soLuong
            })),
            ChiTietDonHangNhapsToDelete: $scope.selectedOrder.ChiTietDonHangNhapsToDelete || []
        };
    
        console.log("Payload:", payload);
    
        let url = $scope.isEditMode ? '/api/DonHangNhap/update' : '/api/DonHangNhap/create';
        let method = $scope.isEditMode ? 'PUT' : 'POST';
    
        $http({
            method: method,
            url: current_url + url,
            headers: { 'Content-Type': 'application/json' },
            data: payload
        }).then(function(response) {
            alert($scope.isEditMode ? "Cập nhật đơn hàng nhập thành công!" : "Tạo đơn hàng nhập thành công!");
            $scope.LoadOrder();
            var orderModal = bootstrap.Modal.getInstance(document.getElementById('orderModal'));
            if (orderModal) {
                orderModal.hide();
            }
        }).catch(function(error) {
            console.error("Lỗi cập nhật:", error);
            console.log(error.data.errors);
            alert($scope.isEditMode ? "Cập nhật đơn hàng nhập thất bại!" : "Tạo đơn hàng nhập thất bại!");
        });
    };
    
    $scope.LoadOrder();
    

     
    $scope.clearForm = function () {
        $scope.selectedOrder = {}; // Reset dữ liệu đơn hàng
        $scope.listChiTiet = [];  // Xóa danh sách chi tiết
        $scope.isEditMode = false; 
    
        // Đảm bảo xoá backdrop và reset body
        document.body.classList.remove('modal-open');
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) backdrop.remove();
        document.body.style.overflow = ''; // Reset overflow của body
    };
    // Lắng nghe sự kiện khi modal đóng
    document.getElementById('orderModal').addEventListener('hidden.bs.modal', function () {
        document.body.classList.remove('modal-open');
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) backdrop.remove();
        document.body.style.overflow = ''; // Reset overflow của body
    });

    
        // Hàm tạo phân trang (mũi tên)
    function createPagination(totalPages) {
        const pagination = document.querySelector('#pagination .pagination');
      
        pagination.innerHTML = ''; 

        // Nút "Previous" với mũi tên
        const prevItem = document.createElement('li');
        prevItem.className = `page-item ${$scope.page === 1 ? 'disabled' : ''}`;
        prevItem.innerHTML = `<a class="page-link" href="#" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a>`;
        prevItem.onclick = function(e) {
            e.preventDefault();
            if ($scope.page > 1) {
                $scope.page--;
                $scope.LoadOrder(); 
            }
        };
        pagination.appendChild(prevItem);

        // Các nút số trang
        for (let i = 1; i <= totalPages; i++) {
            const pageItem = document.createElement('li');
            pageItem.className = `page-item ${i === $scope.page ? 'active' : ''}`;
            pageItem.innerHTML = `<a class="page-link" href="#">${i}</a>`;
            pageItem.onclick = function(e) {
                e.preventDefault();
                $scope.page = i;
                $scope.LoadOrder();  // Gọi lại API khi chuyển trang
            };
            pagination.appendChild(pageItem);
        }

        // Nút "Next" với mũi tên
        const nextItem = document.createElement('li');
        nextItem.className = `page-item ${$scope.page === totalPages ? 'disabled' : ''}`;
        nextItem.innerHTML = `<a class="page-link" href="#" aria-label="Next"><span aria-hidden="true">&raquo;</span></a>`;
        nextItem.onclick = function(e) {
            e.preventDefault();
            if ($scope.page < totalPages) {
                $scope.page++;
                $scope.LoadOrder();  // Gọi lại API khi chuyển trang
            }
        };
        pagination.appendChild(nextItem);
    }
    // $scope.searchOrders = function(page) {
    //     $scope.page = page || 1; 
    //     var searchRequest = {
    //         tenncc: $scope.searchQuery.tenncc,   
    //         Page: $scope.page,  
    //         PageSize: 6  
    //     };
    
    //     // Gọi API với dữ liệu tìm kiếm
    //     $http({
    //         method: 'POST',
    //         url: current_url + '/api/DonHangNhap/search',  
    //         headers: { 'Content-Type': 'application/json' },  
    //         data: searchRequest  
    //     })
    //     .then(function(response) {
    //         if (response.data.success) { 
                
    //             $scope.listOrder = response.data.data;  
    //             $scope.totalRecords = response.data.totalRecords;  
    //             $scope.totalPages = response.data.totalPages;  
    //             $scope.currentPage = response.data.currentPage;  
    //             createPagination($scope.totalPages);  
    //         } else {
    //             console.error('Tìm kiếm thất bại:', response.data.message);  
    //         }
    //     }, function(error) {
    //         console.error('Lỗi khi gọi API:', error.data);  
    //     });
    // };
    
  
    // $scope.searchOrders($scope.page);


   
     // Hiển thị modal thêm/sửa
     $scope.clearForm = function() {
        $scope.isEditMode = false;
        $scope.selectedOrder = { ngayNhap: new Date(), tenNCC: '' };
        $scope.listChiTiet = [];
        var orderModal = new bootstrap.Modal(document.getElementById('orderModal'));
        orderModal.show();
    };
   
    
});

