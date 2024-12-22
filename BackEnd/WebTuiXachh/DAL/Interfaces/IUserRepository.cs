using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IUserRepository
    {
        // Thêm người dùng
        bool Create(UserModel model);

        // Cập nhật người dùng
        bool Update(UserModel model);

        // Xóa người dùng
        bool Delete(int perID);

        // Lấy thông tin người dùng theo ID
        UserModel GetDatabyID(int id);
        List<UserModel> GetDataAll();

        // Tìm kiếm người dùng theo các tiêu chí (ví dụ: tên, tài khoản)
        List<UserModel> Search(int pageIndex, int pageSize, out long total, string taikhoan ="",string hoten = "", string diachi = "");

        // Xác thực thông tin tài khoản và mật khẩu 
        UserModel Authenticate(string taiKhoan, string matKhau);
    }
}
