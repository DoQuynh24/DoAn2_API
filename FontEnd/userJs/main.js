document.addEventListener("DOMContentLoaded", () => {
    const paragraphs = document.querySelectorAll("#header-center p");
    let currentIndex = 0; 
    function showNextParagraph() {   // Ẩn tất cả các thẻ <p>
        paragraphs.forEach(p => p.classList.remove("show"));
        paragraphs[currentIndex].classList.add("show"); // Hiển thị thẻ <p> hiện tại
        currentIndex = (currentIndex + 1) % paragraphs.length;// Chuyển sang thẻ <p> tiếp theo, nếu đến cuối thì quay lại thẻ đầu tiên
    }
    // Gọi hàm showNextParagraph mỗi 2 giây
    showNextParagraph(); // Hiển thị thẻ đầu tiên ngay lập tức
    setInterval(showNextParagraph, 2000); // Lặp lại mỗi 2 giây
});

document.addEventListener("DOMContentLoaded", function() {
    var userIcon = document.getElementById("user-icon");
    var loginForm = document.getElementById("login-form");
    var userDropdown = document.getElementById("user-dropdown");
    var logoutBtn = document.getElementById("logout-btn");



    // Mở dropdown khi nhấn vào user-icon
    userIcon.addEventListener('click', function(event) {
        // Ngừng sự kiện click để không truyền ra ngoài
        event.stopPropagation();

        // Toggle menu dropdown
        userDropdown.style.display = (userDropdown.style.display === 'block') ? 'none' : 'block';
    });

    // Đóng dropdown khi nhấn ra ngoài
    document.addEventListener('click', function(event) {
        if (!userIcon.contains(event.target) && !userDropdown.contains(event.target)) {
            userDropdown.style.display = 'none';
        }
    });

    // Xử lý đăng xuất
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function() {
            // Xóa thông tin người dùng khỏi localStorage
            localStorage.removeItem('hoten');
   
         
            // Redirect về trang đăng nhập
            window.location.href = '/user/login.html';
        });
    }

    // Nếu người dùng đã đăng nhập
    if (userName) {
        // Hiển thị menu dropdown khi trỏ chuột vào biểu tượng user
        if (userIcon) {
            userIcon.addEventListener("mouseover", function() {
                if (userDropdown) {
                    userDropdown.style.display = "block";
                }
            });
        }

        // Ẩn form đăng nhập nếu người dùng đã đăng nhập
        if (loginForm) {
            loginForm.style.display = "none";
        }
    } else {
        // Nếu chưa đăng nhập, khi trỏ chuột vào sẽ hiển thị form đăng nhập
        if (userIcon) {
            userIcon.addEventListener("mouseover", function() {
                if (loginForm) {
                    loginForm.style.display = "block";
                }
            });
        }

        // Ẩn menu dropdown nếu chưa đăng nhập
        if (userDropdown) {
            userDropdown.style.display = "none";
        }

        // Ẩn form đăng nhập khi chuột rời khỏi biểu tượng người dùng và form
        if (loginForm) {
            loginForm.addEventListener("mouseleave", function() {
                loginForm.style.display = "none";
            });
        }
    }



    // Thao tác hiển thị/ẩn mật khẩu khi nhấn vào icon
    var togglePassword = document.getElementById("toggle-password");
    if (togglePassword) {
        togglePassword.addEventListener("click", function() {
            var passwordInput = document.getElementById("password");
            var eyeIcon = document.getElementById("eye-icon");

            if (passwordInput && eyeIcon) {
                // Kiểm tra xem mật khẩu đang ẩn hay hiển thị
                if (passwordInput.type === "password") {
                    passwordInput.type = "text"; // Hiển thị mật khẩu
                    eyeIcon.src = "images/eye-slash.png"; // Đổi icon mắt thành mắt đã gạch
                } else {
                    passwordInput.type = "password"; // Ẩn mật khẩu
                    eyeIcon.src = "images/eye.png"; // Đổi icon trở lại mắt bình thường
                }
            }
        });
    }
});


// Hàm kiểm tra email hoặc số điện thoại
function validateEmailOrPhone(input) {
    // Kiểm tra xem input có phải là email hợp lệ
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (emailPattern.test(input)) {
        return true;
    }

    // Kiểm tra xem input có phải là số điện thoại hợp lệ (chỉ chứa số và độ dài 10-11 ký tự)
    var phonePattern = /^[0-9]{10,11}$/;
    if (phonePattern.test(input)) {
        return true;
    }

    return false;
}

// Hiển thị dropdown khi trỏ chuột vào "Sản phẩm"
document.getElementById("product-menu").addEventListener("mouseover", function() {
    document.getElementById("product-dropdown").style.display = "block";
    document.getElementById("collection-dropdown").style.display = "none"; // Ẩn bộ sưu tập khi trỏ vào sản phẩm
    document.getElementById("notifivstion-dropdown").style.display ="none"; //Ẩn thông báo khi trỏ vào sản phẩm
});

// Ẩn dropdown khi rê chuột ra ngoài mục "Sản phẩm"
document.getElementById("product-dropdown").addEventListener("mouseleave", function() {
    document.getElementById("product-dropdown").style.display = "none";
});

// Hiển thị dropdown khi trỏ chuột vào "Bộ sưu tập"
document.getElementById("collection-menu").addEventListener("mouseover", function() {
    document.getElementById("collection-dropdown").style.display = "block";
    document.getElementById("product-dropdown").style.display = "none"; // Ẩn sản phẩm khi trỏ vào bộ sưu tập
    document.getElementById("notifivstion-dropdown").style.display ="none"; // Ẩn thông báo khi trỏ vào bộ sưu tập
});

// Ẩn dropdown khi rê chuột ra ngoài mục "Bộ sưu tập"
document.getElementById("collection-dropdown").addEventListener("mouseleave", function() {
    document.getElementById("collection-dropdown").style.display = "none";
});

// Hiển thị dropdown khi trỏ chuột vào "Thông báo"
document.getElementById("notifivstion-menu").addEventListener("mouseover", function() {
    document.getElementById("notifivstion-dropdown").style.display = "block";
    document.getElementById("product-dropdown").style.display = "none"; // Ẩn sản phẩm khi trỏ vào thông báo
    document.getElementById("collection-dropdown").style.display = "none"; // Ẩn bộ sưu tập khi trỏ vào thông báo
});

// Ẩn dropdown khi rê chuột ra ngoài mục "Thông báo"
document.getElementById("notifivstion-dropdown").addEventListener("mouseleave", function() {
    document.getElementById("notifivstion-dropdown").style.display = "none";
});


document.addEventListener('DOMContentLoaded', function(){
    var nameLogo = document.getElementById('name');
    if (nameLogo) { 
        nameLogo.addEventListener('click', function(){
            window.location.href = '/user/index.html';
        });
    }
});



document.addEventListener('DOMContentLoaded', function(){
    var thongBao = document.getElementById('notifivstion-menu');
    if (thongBao) {
        thongBao.addEventListener('click', function(){
            window.location.href ='/user/news.html';
        });
    }
});


document.addEventListener('DOMContentLoaded', function(){
    var newButton = document.getElementById('new-arrivals');
    newButton.addEventListener('click', function(){
        window.location.href = '/user/NewBag.html';
    });
});
document.addEventListener('DOMContentLoaded', function(){
    var newButton = document.getElementById('new-arrivalss');
    newButton.addEventListener('click', function(){
        window.location.href = '/user/NewBag.html';
    });
});
// Thêm sự kiện click cho phần tử có id="cart"
document.addEventListener("DOMContentLoaded", function() {
    var cartIcon = document.getElementById("cart");
    cartIcon.addEventListener("click", function() {
        window.location.href = "/user/cart.html";
    });
});


const imageList = document.getElementById('image-list');
let isDown = false;
let startX;
let scrollLeft;

imageList.addEventListener('mousedown', (e) => {
    e.preventDefault(); // Ngăn hành vi kéo mặc định của trình duyệt
    isDown = true;
    startX = e.pageX - imageList.offsetLeft;
    scrollLeft = imageList.scrollLeft;
    imageList.style.cursor = 'grabbing'; // Đổi con trỏ khi đang kéo
});

imageList.addEventListener('mouseleave', () => {
    isDown = false;
    imageList.style.cursor = 'grab';
});

imageList.addEventListener('mouseup', () => {
    isDown = false;
    imageList.style.cursor = 'grab';
});

imageList.addEventListener('mousemove', (e) => {
    if (!isDown) return;
    e.preventDefault();
    const x = e.pageX - imageList.offsetLeft;
    const walk = (x - startX) * 1; // Điều chỉnh tốc độ cuộn
    imageList.scrollLeft = scrollLeft - walk;
});
 
 

    // Hàm thay đổi ảnh khi trỏ vào
    function changeImageOnHover(imgElement, newSrc) {
        imgElement.dataset.originalSrc = imgElement.src; // Lưu lại ảnh ban đầu
        imgElement.src = newSrc; // Thay thế bằng ảnh mới
    }

    // Hàm khôi phục lại ảnh ban đầu khi bỏ trỏ
    function restoreImage(imgElement) {
        imgElement.src = imgElement.dataset.originalSrc; // Khôi phục ảnh gốc
    }


// Hiển thị dropdown khi trỏ chuột vào "Color"
document.getElementById("color-menu").addEventListener("mouseover", function() {
    document.getElementById("color-dropdown").style.display = "block";
});

// Ẩn dropdown khi rê chuột ra ngoài mục "color"
document.getElementById("color-dropdown").addEventListener("mouseleave", function() {
    document.getElementById("color-dropdown").style.display = "none";
});

// Hiển thị dropdown khi trỏ chuột vào "category"
document.getElementById("category-menu").addEventListener("mouseover", function() {
    document.getElementById("category-dropdown").style.display = "block";
});

// Ẩn dropdown khi rê chuột ra ngoài mục "category"
document.getElementById("category-dropdown").addEventListener("mouseleave", function() {
    document.getElementById("category-dropdown").style.display = "none";
});

// Hiển thị dropdown khi trỏ chuột vào "material"
document.getElementById("material-menu").addEventListener("mouseover", function() {
    document.getElementById("material-dropdown").style.display = "block";
});

// Ẩn dropdown khi rê chuột ra ngoài mục "material"
document.getElementById("material-dropdown").addEventListener("mouseleave", function() {
    document.getElementById("material-dropdown").style.display = "none";
});


//NewBag
function showForm(tabId) {
    var newInContent = document.getElementById('new-in-content');
    var newCollectionContent = document.getElementById('new-collection-content');
    var newInTab = document.getElementById('in-tab');
    var newCollectionTab = document.getElementById('collection-tab');
    
    // Ẩn tất cả nội dung
    newInContent.style.display = 'none';
    newCollectionContent.style.display = 'none';
    
    // Bỏ lớp active khỏi tất cả các tab
    newInTab.classList.remove('active');
    newCollectionTab.classList.remove('active');
    
    // Hiển thị nội dung và thêm lớp active cho tab tương ứng
    if (tabId === 'in-form') {
        newInContent.style.display = 'block';
        newInTab.classList.add('active');
    } else if (tabId === 'collection-form') {
        newCollectionContent.style.display = 'block';
        newCollectionTab.classList.add('active');
    }
}

// Hiển thị mặc định tab New In khi tải trang
document.addEventListener('DOMContentLoaded', function() {
    showForm('in-form');
});



