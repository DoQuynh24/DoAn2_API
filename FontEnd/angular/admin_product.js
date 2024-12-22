app.controller('productCtrl', function($scope, $http,) {
    $scope.listProduct = []; // Khởi tạo mảng sản phẩm
    $scope.isEditMode = false; // Kiểm tra trạng thái sửa hay thêm
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
    var current_url = "https://localhost:44366"; 

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

//PRODUCT
    // Hàm tạo URL ảnh
    $scope.getImageUrl = function (fileName) {
        return current_url + '/api/TuiXach/get-img/' + fileName;
    };

  
    $scope.LoadProduct = function() {
        $http({
            method: 'GET',
            url: current_url + '/api/TuiXach/get-all',
        }).then(function (response){
            $scope.listProduct = response.data; 
            $scope.filteredProducts = $scope.listProduct; 
            $scope.totalProducts = $scope.listProduct.length; 
            console.log("Danh sách sản phẩm:", $scope.listProduct);
           
        })
        };
       
    $scope.addProduct = function() {
    formData.append('product', $scope.newProduct);  
    formData.append("MaDanhMuc", $scope.maDanhMuc);
    formData.append("TenSp", $scope.tenSp);
    formData.append("GiaBan", $scope.giaBan);
    formData.append("KhuyenMai", $scope.khuyenMai);
    formData.append("TonKho",$scope.tonKho);
    formData.append("MauSac",$scope.tenMau);
    formData.append("Size",$scope.maSize);
    formData.append("MoTa", $scope.moTa);

    if ($scope.newProduct.selectedFile) {
        formData.append("HinhAnh", $scope.newProduct.selectedFile);
    }

    $http({
        method: 'POST',
        url: current_url + '/api/TuiXach/create',
        headers: { 'Content-Type': undefined },
        data: formData
    }).then(function(response) {
        alert("Thêm sản phẩm thành công!");
        $scope.LoadProduct();
        $scope.clearFormProduct();
    }, function(error) {
        alert("Thêm sản phẩm thất bại!");
    });
    };


    $scope.onFileChange = function (element) {
        $scope.selectedFile = element.files[0]; // Lấy tệp hình ảnh đã chọn
    
        // Nếu có tệp mới, tạo URL mới cho hình ảnh
        if ($scope.selectedFile) {
            // Tạo URL hiển thị hình ảnh mới trong modal (trong trường hợp ảnh chưa được upload)
            $scope.newProduct.imageUrl = URL.createObjectURL($scope.selectedFile);
        }
    };
    

      // Mở modal và chuẩn bị dữ liệu
    $scope.editProduct = function (product) {
        $scope.isEditMode = true;
        $scope.newProduct = angular.copy(product); // Sao chép dữ liệu để sửa
        $scope.newProduct.imageUrl = $scope.getImageUrl(product.hinhAnh);
    };

    $scope.clearFormProduct = function () {
        $scope.isEditMode = false;
        $scope.newProduct = {}; // Reset form
    };

     // Thêm hoặc sửa sản phẩm
     $scope.saveProduct = function () {
        var formData = new FormData();
        // formData.append("MaDanhMuc", $scope.maDanhMuc);
        // formData.append("TenSp", $scope.tenSp);
        // formData.append("GiaBan", $scope.giaBan);
        // formData.append("KhuyenMai", $scope.khuyenMai);
        // formData.app("TonKho",$scope.tonKho);
        // formData.app("MauSac",$scope.tenMau);
        // formData.app("Size",$scope.maSize);
        // formData.append("MoTa", $scope.moTa);

        // if ($scope.newProduct.selectedFile) {
        //     formData.append("HinhAnh", $scope.newProduct.selectedFile);
        // }
        var url = $scope.isEditMode ? '/api/TuiXach/update'  + $scope.newProduct.maSp : '/api/TuiXach/create';
        var method = $scope.isEditMode ? 'PUT' : 'POST'; 
        $http({
            method: method,
            url: current_url + url,
            headers: { 'Content-Type': undefined },
            data: $scope.newProduct,
        }).then(function(response) {
            alert($scope.isEditMode ? "Cập nhật sản phẩm thành công!" : "Thêm sản phẩm mới thành công!");
            $scope.LoadUser(); 
            $scope.clearForm();
        }, function(error) {
            console.error("Lỗi cập nhật:", error.data);
            alert($scope.isEditMode ? "Cập nhật sản phẩm thất bại!" : "Thêm sản phẩm mớithất bại!");
        });
       
    };


    $scope.deleteProduct = function(productID) {
        var confirmation = confirm("Bạn có chắc chắn muốn xóa sản phẩm này không?");
        if (confirmation) {
            $http.delete(current_url + '/api/TuiXach/delete/' + productID).then(function(response) {
                alert("Xóa sản phẩm thành công!");
                $scope.LoadProduct(); // Reload danh sách sản phẩm sau khi xóa
            }, function(error) {
                alert("Xóa sản phẩm thất bại!");
            });
        } else {
            alert("Đã hủy thao tác xóa.");
        }
    };

    
    // phân trang
    $scope.currentPage = 1;
    $scope.itemsPerPage = 7;

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
    
    // Lấy bảng màu từ API
    $scope.loadColor = function () {
        $http({
            method: 'GET',
            url: current_url + '/api/MauSac/all',
        }).then(function (response){
                $scope.colors = response.data; 
                console.log("Mausac:", $scope.colors);
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
