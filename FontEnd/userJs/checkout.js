document.addEventListener('DOMContentLoaded', function () {
    var exitLogo = document.getElementById('exit');
    if (exitLogo) {
        exitLogo.addEventListener('click', function () {
            window.location.href = '/user/cart.html';
        });
    }
});


//  // Hiển thị popup thành công
//  const overlay = document.getElementById('overlay');
//  const paymentPopup = document.getElementById('payment-popup');
//  overlay.style.display = 'block';
//  paymentPopup.style.display = 'block';

document.addEventListener("DOMContentLoaded", function() {
    const paymentOptions = document.querySelectorAll(".payment-option");

    paymentOptions.forEach(option => {
        option.addEventListener("click", function() {
            paymentOptions.forEach(opt => opt.classList.remove("selected"));
            document.querySelectorAll(".payment-option .status").forEach(status => {
                status.textContent = "Chưa áp dụng";
            });
            option.classList.add("selected");
            option.querySelector(".status").textContent = "Đã chọn";
        });
    });
});


document.addEventListener("DOMContentLoaded", function () {
    const provinceSelect = document.getElementById("province");
    const districtSelect = document.getElementById("district");

    // Hàm tải danh sách tỉnh
    function loadProvinces() {
        fetch("https://provinces.open-api.vn/api/p/")
            .then(response => response.json())
            .then(data => {
                // Thêm các tỉnh vào dropdown
                data.forEach(province => {
                    const option = document.createElement("option");
                    option.value = province.code; // Mã tỉnh
                    option.textContent = province.name; // Tên tỉnh
                    provinceSelect.appendChild(option);
                });
            })
            .catch(error => console.error("Lỗi khi tải danh sách tỉnh:", error));
    }

    // Hàm tải danh sách huyện khi chọn tỉnh
    function loadDistricts(provinceCode) {
        districtSelect.innerHTML = '<option value="">-- Chọn Huyện --</option>'; // Reset danh sách huyện
        if (!provinceCode) return;

        fetch(`https://provinces.open-api.vn/api/p/${provinceCode}?depth=2`)
            .then(response => response.json())
            .then(data => {
                data.districts.forEach(district => {
                    const option = document.createElement("option");
                    option.value = district.code; // Mã huyện
                    option.textContent = district.name; // Tên huyện
                    districtSelect.appendChild(option);
                });
            })
            .catch(error => console.error("Lỗi khi tải danh sách huyện:", error));
    }

    // Gọi hàm tải tỉnh khi trang được load
    loadProvinces();

    // Lắng nghe sự kiện khi chọn tỉnh
    provinceSelect.addEventListener("change", function () {
        const provinceCode = this.value;
        loadDistricts(provinceCode);
    });
});
