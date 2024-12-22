using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface IUserBusiness
    {
        // Thêm người dùng
        bool CreateUser(UserModel model);

        // Cập nhật thông tin người dùng
        bool UpdateUser(UserModel model);

        // Xóa người dùng
        bool DeleteUser( int perID);

        // Xác thực thông tin tài khoản và mật khẩu
        UserModel Authenticate(string taiKhoan, string matKhau);

        // Lấy thông tin người dùng theo ID
        UserModel GetUserById(int id);
        //Lấy tất cả người dùng
        List<UserModel> GetDataAll();

        // Tìm kiếm người dùng với phân trang
        List<UserModel> SearchUsers(int pageIndex, int pageSize, out long total,string taikhoan = "" , string hoten = "",  string diachi = "");
    }
}

