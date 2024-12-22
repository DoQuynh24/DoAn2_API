window.onload = function () {
    const cartItemsContainer = document.getElementById("cart-items");
    const totalQuantityElement = document.getElementById("total-quantity");
    const totalPriceElement = document.getElementById("total-price");
    const lableElement = document.getElementById("lable");
    const checkoutBtn = document.getElementById("checkout-btn");

    const taikhoan = localStorage.getItem('TaiKhoan'); 
    if (!taikhoan) {
        cartItemsContainer.innerHTML = "<p>Vui lòng đăng nhập để xem giỏ hàng!</p>";
        return;
    }

    const cartKey = `${taikhoan}_cart`;
    let cart = JSON.parse(localStorage.getItem(cartKey)) || [];

    function updateCartDisplay() {
        cartItemsContainer.innerHTML = '';
        let totalQuantity = 0;
        let totalPrice = 0;

        if (cart.length === 0) {
            cartItemsContainer.innerHTML = "<p>Giỏ hàng trống!</p>";
            lableElement.style.display = 'none';
            checkoutBtn.style.display = 'none';
        } else {
            cart.forEach((product, index) => {
                const cleanPrice = parseInt(product.price.replace(/[^\d]/g, ''));
                totalQuantity += product.quantity || 1;
                totalPrice += cleanPrice * (product.quantity || 1);

                const itemDiv = document.createElement("div");
                itemDiv.classList.add("cart-item");
                itemDiv.innerHTML = `
                    <button class="btn-remove" data-index="${index}">✖︎</button>
                    <img src="${product.image}" alt="${product.name}" class="cart-item-image">
                    <div class="cart-item-details">
                        <h3 class="cart-item-name">${product.name}</h3>
                        <p class="cart-item-price">${cleanPrice.toLocaleString('vi-VN')} VNĐ</p>
                        <div class="button">
                            <button class="btn-decrease" data-index="${index}">-</button>
                            <span class="item-quantity">${product.quantity || 1}</span>
                            <button class="btn-increase" data-index="${index}">+</button>
                        </div>
                    </div>
                `;
                cartItemsContainer.appendChild(itemDiv);
            });

            totalQuantityElement.innerText = totalQuantity;
            totalPriceElement.innerText = totalPrice.toLocaleString('vi-VN');
            lableElement.style.display = 'block';
            checkoutBtn.style.display = 'block';
        }
    }

    updateCartDisplay();

    // Sự kiện thay đổi số lượng hoặc xóa sản phẩm
    cartItemsContainer.addEventListener('click', function (event) {
        const index = event.target.dataset.index;
        if (!index) return;

        if (event.target.classList.contains('btn-remove')) {
            cart.splice(index, 1);
        } else if (event.target.classList.contains('btn-increase')) {
            cart[index].quantity += 1;
        } else if (event.target.classList.contains('btn-decrease')) {
            if (cart[index].quantity > 1) {
                cart[index].quantity -= 1;
            } else {
                cart.splice(index, 1);
            }
        }

        // Cập nhật giỏ hàng và giao diện
        localStorage.setItem(cartKey, JSON.stringify(cart));
        updateCartDisplay();
    });


    // Nút thanh toán
    checkoutBtn.addEventListener("click", function () {
        localStorage.setItem("checkoutCart", JSON.stringify(cart)); // Lưu giỏ hàng vào LocalStorage
        localStorage.setItem("totalPrice", totalPriceElement.innerText); // Lưu tổng giá
        window.location.href = "/user/checkout.html"; // Chuyển hướng đến trang thanh toán
    });
};
