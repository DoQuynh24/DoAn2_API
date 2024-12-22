app.controller('homeCtrl',function($scope, $http,)  {
    $scope.totalProducts = 0; // Tổng số sản phẩm
    $scope.listProduct = []; // Khởi tạo mảng sản phẩm

    $scope.listUser = []; 
    $scope.totalUser = 0; 

    $scope.listHoaDon = []; 
    $scope.filteredHoaDon = [];       // Danh sách hóa đơn theo trạng thái
    $scope.totalHoaDon = 0;           // Tổng tất cả hóa đơn
    $scope.totalDangXuLy = 0;         // Tổng hóa đơn đang xử lý 
    $scope.totalDangLayHang = 0;  
    $scope.totalChoGiaoHang = 0;
    $scope.totalHoanThanh = 0; 
    $scope.totalDaHuy = 0;   


    var current_url = "https://localhost:44366"; 

    $scope.LoadProduct = function() {
        $http({
            method: 'GET',
            url: current_url + '/api/TuiXach/get-all',
        }).then(function (response){
            $scope.listProduct = response.data; 
            $scope.filteredProducts = $scope.listProduct; 
            $scope.totalProducts = $scope.listProduct.length; 
         
           
        })
    };
    $scope.LoadProduct();

    $scope.LoadUser = function() {
        $http({
            method: 'GET',
            url: current_url + '/api/User/get-all-users',
           
        }).then(function(response) {
            $scope.listUser = response.data.data;
            $scope.totalUser = $scope.listUser.length;
            
        });
    };
    $scope.LoadUser();

 // Hàm load hóa đơn
 $scope.LoadHoaDon = function () {
    let url = current_url + '/api/HoaDon/get-all';

    $http({
        method: 'GET',
        url: url
    }).then(function (response) {
        $scope.listHoaDon = response.data;

        // Tính tổng số hóa đơn
        $scope.totalHoaDon = $scope.listHoaDon.length;

        // Tính tổng hóa đơn ở các trạng thái
        $scope.totalDangXuLy = $scope.listHoaDon.filter(hd => hd.trangThai === 'Đang xử lý').length;
        $scope.totalDangLayHang = $scope.listHoaDon.filter(hd => hd.trangThai === 'Đang lấy hàng').length;
        $scope.totalChoGiaoHang = $scope.listHoaDon.filter(hd => hd.trangThai === 'Chờ giao hàng').length;
        $scope.totalHoanThanh = $scope.listHoaDon.filter(hd => hd.trangThai === 'Hoàn thành').length;
        $scope.totalDaHuy = $scope.listHoaDon.filter(hd => hd.trangThai === 'Đã hủy').length;



        // Gọi hàm tạo biểu đồ
        createHoaDonChart();
    }).catch(function (error) {
        console.error('Lỗi khi tải hóa đơn:', error);
    });
};

// Gọi hàm LoadHoaDon
$scope.LoadHoaDon();

// Hàm tạo biểu đồ dạng miền dập dìu từ số liệu hóa đơn
function createHoaDonChart() {
    var ctx = document.getElementById('hoaDonChart').getContext('2d');

    var hoaDonChart = new Chart(ctx, {
        type: 'line', // Biểu đồ đường (miền)
        data: {
            labels: ['Đang xử lý','Đang lấy hàng', 'Chờ giao hàng','Hoàn thành','Đã hủy'], // Trạng thái hóa đơn
            datasets: [{
                label: 'Số lượng hóa đơn',
                data: [
                    $scope.totalDangXuLy, 
                    $scope.totalDangLayHang,
                    $scope.totalChoGiaoHang,
                    $scope.totalHoanThanh,
                    $scope.totalDaHuy
                ],
                borderColor: [
                    '#ff6384', // Đang xử lý - Đỏ
                    '#ffa500', // Đang lấy hàng - Xanh dương
                    '#008000', // Chờ giao hàng - Vàng
                    '#800080', // Hoàn thành - Xanh ngọc
                    '#ff0000'  // Đã hủy - Tím
                ],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',  // Đang xử lý
                    'rgba(54, 162, 235, 0.2)',  // Đang lấy hàng
                    'rgba(255, 206, 86, 0.2)',  // Chờ giao hàng
                    'rgba(75, 192, 192, 0.2)',  // Hoàn thành
                    'rgba(153, 102, 255, 0.2)'  // Đã hủy
                ],
                fill: true, // Tạo nền dưới đường
                borderWidth: 2, // Độ dày của đường
                pointBackgroundColor: [
                    '#ff6384',
                    '#ffa500',
                    '#008000',
                    '#800080',
                    '#ff0000'
                ], // Màu các điểm trên đường
                tension: 0.5, // Tăng độ cong cho đường để tạo hiệu ứng dập dìu
                pointRadius: 2, // Hiển thị điểm trên đường

            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true,
                    position: 'top',
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1, // Chỉ tăng theo số nguyên
                        callback: function(value) { 
                            return Number.isInteger(value) ? value : null; 
                        }
                    },
                    title: {
                        display: true,
                        text: 'Số lượng hóa đơn'
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: 'Trạng thái hóa đơn'
                    },
                    ticks: {
                        callback: function(value, index) {
                            // Danh sách màu cho các trạng thái
                            let colors = ['#ff6384', '#ffa500', '#008000', '#800080', '#ff0000'];
                            let labels = ['Đang xử lý', 'Đang lấy hàng', 'Chờ giao hàng', 'Hoàn thành', 'Đã hủy'];
        
                            // Tạo thẻ HTML với màu sắc cho từng trạng thái
                            return labels[index];
                        },
                        font: {
                            size: 14,
                            weight: 'bold'
                        },
                        color: function(context) {
                            // Danh sách màu cho trạng thái
                            const colors = ['#ff6384', '#ffa500', '#008000', '#800080', '#ff0000'];
                            return colors[context.index]; // Đổi màu chữ tương ứng với chỉ số
                        }
                    }
                }
            },
            elements: {
                line: {
                    tension: 1  // Độ cong của đường để tạo sự mềm mại
                }
            }
        }
    });
}



    

});
