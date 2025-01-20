app.controller('productCtrl', function($scope, $http,) {
    $scope.listProduct = []; // Khởi tạo mảng sản phẩm
    $scope.isEditProductMode = false; // Kiểm tra trạng thái sửa hay thêm
    $scope.newProduct = {};// Sản phẩm mới để thêm hoặc sửa
    $scope.currentProduct = {}; 

    $scope.categories = []; // Danh sách danh mục
    $scope.selectedCategory = ""; // Danh mục được chọn
    $scope.isEditCategoryMode = false; // Biến để kiểm tra chế độ thêm hay sửa
    $scope.categoryToEdit = {}; // Danh mục cần chỉnh sửa
    $scope.categoryNotFound = false; // Kiểm tra xem mã danh mục có tồn tại không

    $scope.sizes = []; // Danh sách size
    $scope.selectedSize="";

    $scope.colors = []; // Danh sách màu sắc
    $scope.selectedColor=""; //Màu sắc được chọn

    $scope.totalProducts = 0; // Tổng số sản phẩm

    // URL API
    var current_url = "https://localhost:44374"; 

   // Hàm kiểm tra mã danh mục có tồn tại không
   $scope.checkCategoryExist = function() {
    $scope.categoryNotFound = true; // Mặc định báo lỗi nếu chưa tìm thấy
    for (var i = 0; i < $scope.categories.length; i++) {
        if ($scope.categories[i].maDanhMuc === $scope.categoryToEdit.maDanhMuc) {
            $scope.categoryNotFound = false; // Nếu có tồn tại, bỏ báo lỗi
            $scope.categoryToEdit.tenDanhMuc = $scope.categories[i].tenDanhMuc; // Hiển thị tên danh mục
            break;
        }
    }
    };

    // Hàm mở modal và nạp thông tin danh mục vào modal
    $scope.editCategory = function() {
        $scope.categoryToEdit = angular.copy(Category);
        $scope.showEditModal = true; // Biến điều khiển mở modal
        var myModal = new bootstrap.Modal(document.getElementById('CategoryModal'));
        myModal.show(); // Show the modal
        
    };

    // Hàm lưu (thêm mới hoặc sửa)
    $scope.saveCategory = function() {

        var formData = new FormData();
        if ($scope.categoryNotFound) {
            alert("Mã danh mục không hợp lệ!");
            return;
        }
        formData.append("MaDanhMuc", $scope.categoryToEdit.maDanhMuc);
        formData.append("TenDanhMuc", $scope.categoryToEdit.tenDanhMuc);
        var url = $scope.isEditMode ? '/api/DanhMucSanPham/update' : '/api/DanhMucSanPham/create';
        var method = $scope.isEditMode ? 'PUT' : 'POST';
        $http({
            method: method,
            url: current_url + url,
            headers: { 'Content-Type': undefined },
            data: formData,
        }).then(function(response) {
            alert($scope.isEditMode ?"Chỉnh sửa danh mục thành công!": "Thêm danh mục thành công!");
            $scope.loadCategories(); 
            $('#editCategoryModal').modal('hide'); 
        }, function(error) {
            console.error($scope.isEditMode ?"Lỗi khi chỉnh sửa danh mục:" : "Lỗi thêm danh mục!", error);
            alert($scope.isEditMode ? "Cập nhật danh mục thất bại!" : "Thêm danh mục thất bại!");
        });
    };

    // Lấy danh sách danh mục từ API
    $scope.loadCategories = function () {
        $http({
            method: 'GET',
            url: current_url + '/api/DanhMucSanPham/all',
        }).then(function (response){
                $scope.categories = response.data; 
                console.log("Danh sách danh mục:", $scope.categories);
            });
    };
    // Hàm thêm danh mục mới
    $scope.addCategory = function () {
        var formData = new FormData();
        formData.append("MaDanhMuc", $scope.newCategory.maDanhMuc);
        formData.append("TenDanhMuc", $scope.newCategory.tenDanhMuc);
    
        $http({
            method: 'POST',
            url: current_url + '/api/DanhMucSanPham/create',
            data: formData,
            headers: { 'Content-Type': undefined } // Để trình duyệt tự thiết lập Content-Type
        }).then(function (response) {
            alert("Thêm danh mục mới thành công!");
            $scope.loadCategories(); // Gọi lại hàm load danh mục
            $scope.newCategory = {}; // Reset dữ liệu nhập
            $('#addCategoryModal').modal('hide'); // Đóng modal
        }, function (error) {
            console.error("Lỗi khi thêm danh mục:", error);
            alert("Thêm danh mục thất bại!");
        });
    };
    // Lọc sản phẩm theo danh mục
    $scope.filterProducts = function () {
        console.log("Selected Category:", $scope.selectedCategory);
        $http({
            method: 'GET',
            url: `${current_url}/api/TuiXach/get-by-danhmuc/BL`
        }).then(function (response) {
            if (response.data && response.data.length > 0) {
                $scope.listProduct = response.data;
                $scope.filteredProducts = $scope.listProduct; // Cập nhật danh sách hiển thị
                console.log("Danh sách sản phẩm theo danh mục:", $scope.filteredProducts);
            } else {
                $scope.listProduct = [];
                $scope.filteredProducts = []; // Xóa danh sách hiển thị 
            }
        }, function (error) {
            console.error("Lỗi gọi API:", error.data);
          
        });
    };
   
    
    // Hàm tra cứu tên danh mục từ ID
    $scope.category = function (categoryId) {
        var category = $scope.categories.find(function (cat) {
            return cat.maDanhMuc === categoryId;
        });
        return category ? category.tenDanhMuc : 'Chưa xác định';  // Trả về tên danh mục hoặc 'Chưa xác định' nếu không tìm thấy
    };
    

//PRODUCT
    // Hàm tạo URL ảnh
    $scope.getImageUrl = function (fileName) {
        return current_url + '/api/TuiXach/get-img/' + fileName;
    };

    
    $scope.LoadProduct = function() {
        $http({
            method: 'GET',
            url: 'https://localhost:44374/api/TuiXach/get-all',
        }).then(function (response){
            $scope.listProduct = response.data; 
            $scope.filteredProducts = $scope.listProduct; 
            $scope.totalProducts = $scope.listProduct.length; 
            console.log("Danh sách sản phẩm:", $scope.listProduct);
    
           
        })
    };
    
       
    $scope.addProduct = function () {
        var formData = new FormData();  
        var fileInput = document.getElementById('fileInput');
        
        formData.append("MaDanhMuc", $scope.newProduct.maDanhMuc);
        formData.append("MaSp", $scope.newProduct.maSp);
        formData.append("TenSp", $scope.newProduct.tenSp);
        formData.append("GiaBan", $scope.newProduct.giaBan);
        formData.append("KhuyenMai", $scope.newProduct.khuyenMai);
        formData.append("TonKho", $scope.newProduct.tonKho);
        formData.append("TenMau", $scope.newProduct.tenMau);  
        formData.append("MaSize", $scope.newProduct.maSize);   
        formData.append("MoTa", $scope.newProduct.moTa);
    
        // Kiểm tra nếu có tệp hình ảnh
        if (fileInput.files && fileInput.files[0]) {
            formData.append('hinhAnhFile', fileInput.files[0]); // Thêm tệp hình ảnh
            formData.append('HinhAnh', fileInput.files[0].name); // Thêm tên tệp
        } else {
            alert("Vui lòng chọn tệp hình ảnh!");
            return; // Dừng lại nếu chưa có tệp hình ảnh
        }


    
        // Gửi FormData tới API
        $http({
            method: 'POST',
            url: current_url + '/api/TuiXach/create',
            headers: { 'Content-Type': undefined }, // Để trình duyệt tự đặt `Content-Type` cho `FormData`
            data: formData
        }).then(function (response) {
            alert("Thêm sản phẩm thành công!");
            $scope.LoadProduct(); // Tải lại danh sách sản phẩm
            $scope.clearFormProduct(); // Xóa dữ liệu trong form
        }, function (error) {
            alert("Thêm sản phẩm thất bại!");
            console.error("Lỗi API:", error.data);
        });
    };
    

    // $scope.onFileChange = function (element) {
    //     $scope.selectedFile = element.files[0]; // Lấy tệp hình ảnh đã chọn
    
    //     // Nếu có tệp mới, tạo URL mới cho hình ảnh
    //     if ($scope.selectedFile) {
    //         // Tạo URL hiển thị hình ảnh mới trong modal (trong trường hợp ảnh chưa được upload)
    //         $scope.newProduct.imageUrl = URL.createObjectURL($scope.selectedFile);
    //     }
    // };
    
    $scope.onFileChange = function (element) {
        $scope.selectedFile = element.files[0]; // Lấy tệp hình ảnh đã chọn
    
        if ($scope.selectedFile) {
            // Hiển thị ảnh đã chọn trong giao diện
            $scope.newProduct.imageUrl = URL.createObjectURL($scope.selectedFile);
            $scope.$apply(); // Cập nhật AngularJS
        }
    };
    
      // Mở modal và chuẩn bị dữ liệu
      $scope.editProduct = function(product) {
        $scope.isEditProductMode = true; // Đặt chế độ sửa
        $scope.newProduct = angular.copy(product); // Sao chép dữ liệu sản phẩm
        $scope.newProduct.imageUrl = $scope.getImageUrl(product.hinhAnh); // Đường dẫn hình ảnh hiện tại
        $('#productModal').modal('show'); // Hiển thị modal
    };
    
    
    
    // $scope.onFileChange = function (element) {
    //     // Kiểm tra xem người dùng có chọn tệp không
    //     if (element.files && element.files[0]) {
    //         var reader = new FileReader();
    //         reader.onload = function (e) {
    //             // Lưu thông tin tệp vào đối tượng newProduct và hiển thị ảnh
    //             $scope.newProduct.selectedFile = element.files[0];
    //             $scope.newProduct.imageUrl = e.target.result;  // Lưu URL ảnh để hiển thị
    //             $scope.$apply();  // Cập nhật AngularJS
    //         };
    //         reader.readAsDataURL(element.files[0]);  // Đọc tệp dưới dạng URL để hiển thị
    //     }
    // };
    
    
    
    

    $scope.clearFormProduct = function () {
        $scope.isEditProductMode = false; // Đặt lại chế độ thêm
        $scope.newProduct = {}; // Xóa dữ liệu sản phẩm hiện tại
    };
    

    $scope.saveProduct = function () {
        var fileInput = document.getElementById('fileInput');
        // Tạo đối tượng FormData
        var formData = new FormData();
    
        formData.append("MaDanhMuc", $scope.newProduct.maDanhMuc);
        formData.append("MaSp", $scope.newProduct.maSp);
        formData.append("TenSp", $scope.newProduct.tenSp);
        formData.append("GiaBan", $scope.newProduct.giaBan);
        formData.append("KhuyenMai", $scope.newProduct.khuyenMai);
        formData.append("TonKho", $scope.newProduct.tonKho);
        formData.append("TenMau", $scope.newProduct.tenMau);  
        formData.append("MaSize", $scope.newProduct.maSize);   
        formData.append("MoTa", $scope.newProduct.moTa);
    
        // Kiểm tra nếu có tệp hình ảnh

        formData.append('HinhAnh', fileInput.files[0].name);  

        console.log("Dữ liệu FormData chuẩn bị gửi lên API:");
        for (var pair of formData.entries()) {
            console.log(pair[0] + ": " + pair[1]);
        }
    
        var url = $scope.isEditProductMode ? '/api/TuiXach/update' : '/api/TuiXach/create';
        var method = $scope.isEditProductMode ? 'PUT' : 'POST';
    
        $http({
            method: method,
            url: current_url + url,
            headers: { 'Content-Type': undefined },
            data: formData
        }).then(function (response) {
            alert($scope.isEditProductMode ? "Cập nhật sản phẩm thành công!" : "Thêm sản phẩm mới thành công!");
            $scope.newProduct.imageUrl = response.data.imageUrl;
            $scope.LoadProduct(); 
      
        }, function (error) {
            console.log($scope.newProduct.selectedFile);
            alert($scope.isEditProductMode ? "Cập nhật sản phẩm thất bại!" : "Thêm sản phẩm thất bại!");
            console.error("Lỗi:", error.data);
            
        });
    };
    

    $scope.deleteProduct = function(productID) {
        var confirmation = confirm("Bạn có chắc chắn muốn xóa sản phẩm này không?");
        if (confirmation) {
            $http.delete(current_url + '/api/TuiXach/delete/' + productID).then(function(response) {
                alert("Xóa sản phẩm thành công!");
                $scope.LoadProduct(); 
            }, function(error) {
                alert("Xóa sản phẩm thất bại!");
            });
        } else {
            alert("Đã hủy thao tác xóa.");
        }
    };

    
    // phân trang
    $scope.currentPage = 1;
    $scope.itemsPerPage = 5;

    $scope.paginatedProducts = function() {
        let startIndex = ($scope.currentPage - 1) * $scope.itemsPerPage;
        let endIndex = startIndex + $scope.itemsPerPage;
        return $scope.listProduct.slice(startIndex, endIndex);
    };

    $scope.totalPages = function() {
        return Math.ceil($scope.listProduct.length / $scope.itemsPerPage);
    };

    $scope.setPage = function(page) {
        if (page >= 1 && page <= $scope.totalPages()) {
            $scope.currentPage = page;
        }
    };
  
    $('#productModal').on('shown.bs.modal', function () {
        $(this).find('button').focus();  
    });

    $('#productModal').on('hidden.bs.modal', function () {
        $('#previousElement').focus(); 
    });

    // Lấy bảng màu từ API
    $scope.loadColor = function () {
        $http({
            method: 'GET',
            url: 'https://localhost:44374/api/MauSac/all',
        }).then(function (response){
                $scope.mauSacs = response.data; 
                console.log("Mausac:", $scope.mauSacs);
            });
    };
     // Hàm thêm màu sắc mới
     $scope.addColor = function () {
        var formData = new FormData();
        formData.append("MaMau", $scope.newColor.maMau);
        formData.append("TenMau", $scope.newColor.tenMau);
        $http({
            method: 'POST',
            url: current_url + '/api/MauSac/create', 
            data: formData,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            alert("Thêm màu sắc mới thành công!");
            $scope.loadColors(); // Gọi lại hàm load màu sắc
            $scope.newColor = {}; // Reset dữ liệu nhập
            $('#addColorModal').modal('hide'); // Đóng modal
        }, function (error) {
            console.error("Lỗi khi thêm màu sắc:", error);
            alert("Thêm màu sắc thất bại!");
        });
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


    
    //lọc sản phẩm
    $scope.filterProducts = function () {
        $scope.filteredProducts = $scope.listProduct.filter(function (product) {
            // Lọc theo danh mục
            const matchCategory = $scope.selectedCategory ? product.maDanhMuc === $scope.selectedCategory : true;
    
            // Lọc theo màu sắc
            const matchColor = $scope.selectedColor ? product.mauSac == $scope.selectedColor : true;
    
            // Lọc theo size
            const matchSize = $scope.selectedSize ? product.maSize === $scope.selectedSize : true;
    
            return matchCategory && matchColor && matchSize;
        });
    };
    
    // Gọi filterProducts() để áp dụng các bộ lọc mỗi khi dữ liệu thay đổi
    $scope.$watch('listProduct', function () {
        $scope.filterProducts();
    });

    // Tải dữ liệu ban đầu
    $scope.loadCategories(); // Gọi để tải danh mục ngay khi vào trang
    $scope.loadColor();
    $scope.loadSize();
    $scope.LoadProduct();
    $scope.filterProducts ();

    
});
