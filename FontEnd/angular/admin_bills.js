
app.controller('billsCtrl', function($scope, $http, $timeout) {
    $scope.listHoaDon = []; // Khởi tạo mảng hóa đơn
    $scope.filteredHoaDon = []; // Hóa đơn đã lọc theo trạng thái
    $scope.isEditMode = false; // Kiểm tra trạng thái sửa hay thêm
    $scope.selectedBill = {}; // Hóa đơn được chọn
    $scope.listChiTiet = []; 
    $scope.tongTienHang = 0; // Tổng tiền hàng
    $scope.phiVanChuyen = 25000; // Phí vận chuyển cố định
    $scope.thanhTien = 0; // Thành tiền
    $scope.selectedStatus = 'Tất cả'; 

    $scope.currentPage = 1; 
    $scope.itemsPerPage = 8; 

    // URL API
    var current_url = "https://localhost:44366"; 

    
    $scope.paginatedHoaDon = function () {
        let start = ($scope.currentPage - 1) * $scope.itemsPerPage;
        let end = start + $scope.itemsPerPage;
        return $scope.filteredHoaDon.slice(start, end);
    };
    
    $scope.totalPages = function () {
        return Math.ceil($scope.filteredHoaDon.length / $scope.itemsPerPage);
    };
    
    $scope.setPage = function (page) {
        if (page >= 1 && page <= $scope.totalPages()) {
            $scope.currentPage = page;
        }
    };
    



    // Hàm tính tổng tiền của các hóa đơn hoàn thành
    $scope.calculateTotalCompletedBills = function() {
        $scope.totalCompletedBills = 0; // Đặt lại giá trị ban đầu
        $scope.filteredHoaDon.forEach(function(hd) {
            if (hd.trangThai === 'Hoàn thành') {
                $scope.totalCompletedBills += hd.tongTienHang || 0; // Cộng tổng tiền hàng của hóa đơn (nếu tồn tại)
            }
        });
    };
   // Tải danh sách hóa đơn
   $scope.LoadHoaDon = function(trangThai) {
    $scope.selectedStatus = trangThai; // Cập nhật trạng thái
    let url = current_url + '/api/HoaDon/get-all';

    if (trangThai && trangThai !== 'Tất cả') {
        url += '?trangThai=' + trangThai;
    }

    $http.get(url).then(function(response) {
        $scope.listHoaDon = response.data;
        if (trangThai && trangThai !== 'Tất cả') {
            $scope.filteredHoaDon = $scope.listHoaDon.filter(hd => hd.trangThai === trangThai);
        } else {
            $scope.filteredHoaDon = $scope.listHoaDon;
        }
        $scope.currentPage = 1; // Đặt lại trang hiện tại
        $scope.calculateTotalCompletedBills(); // Tính tổng tiền
    }).catch(function(error) {
        console.error('Lỗi khi tải hóa đơn:', error);
    });
    };

    $scope.LoadHoaDon('Tất cả'); 


    // Xem hóa đơn (hàm khi nhấn nút see-btn)
    $scope.LoadlistCTHD = function (hoaDon) {
        $scope.selectedBill = angular.copy(hoaDon); 
        
        $http({
            method: 'GET',
            url: current_url + '/api/HoaDon/get/' + hoaDon.maHD,
        }).then(function (response) {
            if (response.data) {
                $scope.listChiTiet  = response.data.chiTietHoaDons || [];
                $scope.calculateTongTienHang();
            } else {
                console.error("Dữ liệu chi tiết đơn hàng không hợp lệ:");
            }
            // Hiển thị modal
            var billModal = new bootstrap.Modal(document.getElementById('billModal'), {});
            billModal.show();
        
        }).catch(function (error) {
            console.error('Lỗi khi tải chi tiết hóa đơn:', error);
        });
    };

    $scope.updateBill = function () {
        if ($scope.selectedBill && $scope.selectedBill.maHD && $scope.selectedBill.trangThai) {
            var requestData = $scope.selectedBill.trangThai; 
    
            $http({
                method: 'PUT',
                url: current_url + '/api/HoaDon/update-trang-thai/' + $scope.selectedBill.maHD,
                headers: {
                    'Content-Type': 'application/json'
                },
                data: JSON.stringify(requestData) 
            }).then(function (response) {
                alert('Cập nhật trạng thái thành công!');
                $scope.LoadHoaDon();
                var billModal = bootstrap.Modal.getInstance(document.getElementById('billModal'));
                if (billModal) {
                    billModal.hide(); 
                }     
            }).catch(function (error) {
                console.error('Lỗi khi cập nhật:', error);
                alert('Lỗi khi cập nhật trạng thái!');
            });
        }
    };

    $scope.LoadHoaDon();


   ;


    $scope.calculateTongTienHang = function () {
        $scope.tongTienHang = $scope.listChiTiet.reduce((total, item) => total + (item.giaBan * item.soLuong), 0);
        $scope.calculateThanhTien(); // Cập nhật thành tiền
    };
    $scope.calculateThanhTien = function () {
        $scope.thanhTien = $scope.tongTienHang + $scope.phiVanChuyen;
    };
});
