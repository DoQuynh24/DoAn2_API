$(document).ready(function () {
    $('#login-btn').on('click', login);
    $('#register-btn').on('click', register);

    // Hàm hiển thị form đăng nhập hoặc đăng ký
    function showForm(form) {
        if (form === 'login-form') {
            $('#login-form').show();
            $('#register-form').hide();
            $('#login-tab').addClass('active');
            $('#register-tab').removeClass('active');
        } else {
            $('#login-form').hide();
            $('#register-form').show();
            $('#register-tab').addClass('active');
            $('#login-tab').removeClass('active');
        }
    }

    // Hiển thị form đăng ký mặc định khi tải trang
    showForm('register-form');
});