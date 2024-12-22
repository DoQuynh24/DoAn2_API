document.addEventListener('DOMContentLoaded', function(){
    var exitLogo = document.getElementById('exit');
    if (exitLogo) { 
        exitLogo.addEventListener('click', function(){
            window.location.href = '/user/cart.html';
        });
    }
})
window.onload = function() {
    const totalPriceElement = document.getElementById("total-price"); // Vị trí hiển thị tổng tiền
    const totalPrice = localStorage.getItem("totalPrice"); // Lấy tổng tiền từ LocalStorage

    if (totalPrice) {
        totalPriceElement.innerText = totalPrice; // Cập nhật giá trị trên giao diện
    }
};
 
