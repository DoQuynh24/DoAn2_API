
    var cartIcon = document.getElementById("name");
    cartIcon.addEventListener("click", function() {
        window.location.href = "/user/index.html";
    });

// Lấy các phần tử cần thiết
var togglePassword = document.getElementById("toggle-password");
var passwordInput = document.getElementById("matkhau");
var eyeIcon = document.getElementById("eye-icon");

// Kiểm tra nếu nút và input tồn tại
if (togglePassword && passwordInput && eyeIcon) {
    togglePassword.addEventListener("click", function () {
        // Kiểm tra trạng thái hiện tại của input
        if (passwordInput.type === "password") {
            passwordInput.type = "text"; // Hiển thị mật khẩu
            eyeIcon.src = "images/eye-slash.png"; // Đổi icon thành mắt gạch
        } else {
            passwordInput.type = "password"; // Ẩn mật khẩu
            eyeIcon.src = "images/eye.png"; // Đổi icon thành mắt thường
        }
    });
}

document.addEventListener("DOMContentLoaded", function() {
    const buttons = document.querySelectorAll(".button");
    const tabs = document.querySelectorAll(".tab-content"); 
    const homeDiv = document.querySelector("#home"); 

    if (homeDiv) {
        homeDiv.style.display = "block"; 
    }

    buttons.forEach(button => {
        button.addEventListener("click", function() {
            const tabId = button.getAttribute("data-tab"); 

            tabs.forEach(tab => {
                tab.classList.remove("active");
                tab.style.display = "none"; 
            });

            const activeTab = document.getElementById(tabId); 
            if (activeTab) {
                activeTab.classList.add("active");
                activeTab.style.display = "block"; 
            }
        });
    });
});


