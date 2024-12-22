    
document.addEventListener('DOMContentLoaded', function(){
var nameLogo = document.getElementById('name');
if (nameLogo) { 
nameLogo.addEventListener('click', function(){
window.location.href = '/user/index.html';
    });
    }
});


function showForm(tabId) {
    var descriptionContent = document.getElementById('description-content');
    var sizeContent = document.getElementById('size-content');
    var preserveContent = document.getElementById('preserve-content');
    
    var descriptionTab = document.getElementById('description'); 
    var sizeTab = document.getElementById('size'); 
    var preserveTab = document.getElementById('preserve'); 
    
    // Ẩn tất cả nội dung của các tab
    descriptionContent.style.display = 'none';
    sizeContent.style.display = 'none';
    preserveContent.style.display = 'none';
    
    // Bỏ lớp 'active' khỏi tất cả các tab
    descriptionTab.classList.remove('active');
    sizeTab.classList.remove('active');
    preserveTab.classList.remove('active');
    
    // Hiển thị tab và nội dung tương ứng
    if (tabId === 'description-form') {
        descriptionContent.style.display = 'block';
        descriptionTab.classList.add('active');
    } else if (tabId === 'size-form') {
        sizeContent.style.display = 'block';
        sizeTab.classList.add('active');
    } else if (tabId === 'preserve-form') {
        preserveContent.style.display = 'block';
        preserveTab.classList.add('active');
    }
}

// Mặc định hiển thị tab "Mô tả sản phẩm" khi trang tải
document.addEventListener('DOMContentLoaded', function() {
    showForm('description-form');
});

function toggleContent(tabId) {
    var text = document.getElementById(tabId + '-text');
    var details = document.getElementById(tabId + '-details');
    var toggle = document.getElementById(tabId + '-toggle');
    
    // Kiểm tra xem nội dung đang hiển thị đầy đủ hay bị thu gọn
    if (details.style.display === 'none') {
        details.style.display = 'block';  // Mở rộng nội dung
        toggle.textContent = 'See less';  // Thay đổi nhãn thành "See less"
    } else {
        details.style.display = 'none';   // Thu gọn nội dung
        toggle.textContent = 'See more';  // Thay đổi nhãn thành "See more"
    }
}



function toggleDescription() {
    var details = document.getElementById('description-details');
    details.style.display = details.style.display === 'none' ? 'block' : 'none';
    document.getElementById('description-toggle').textContent = details.style.display === 'block' ? 'See less' : 'See more';
}

function togglePreserve() {
    var details = document.getElementById('preserve-details');
    details.style.display = details.style.display === 'none' ? 'block' : 'none';
    document.getElementById('preserve-toggle').textContent = details.style.display === 'block' ? 'See less' : 'See more';
}


document.addEventListener("DOMContentLoaded", function() {
    var cartIcon = document.getElementById("cart");
    cartIcon.addEventListener("click", function() {
        window.location.href = "/user/cart.html";
    });
});

