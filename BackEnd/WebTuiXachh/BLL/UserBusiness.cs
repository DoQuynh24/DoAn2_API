using DAL;
using Model;
using System.Collections.Generic;

namespace BLL
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Thêm người dùng
        public bool CreateUser(UserModel model)
            => _userRepository.Create(model);

        // Cập nhật thông tin người dùng
        public bool UpdateUser(UserModel model)
            => _userRepository.Update(model); 


        public bool DeleteUser(int perID) => _userRepository.Delete(perID);

        public UserModel GetUserById(int perID) => _userRepository.GetDatabyID(perID);
        public List<UserModel> GetDataAll() => _userRepository.GetDataAll();

        public List<UserModel> SearchUsers(int pageIndex, int pageSize, out long total, string taikhoan = "", string hoten = "", string diachi = "")
        {
            return _userRepository.Search(pageIndex, pageSize, out total , taikhoan, hoten, diachi);

        }


        // Cài đặt phương thức Authenticate
        public UserModel Authenticate(string taiKhoan, string matKhau)
        {
            // Kiểm tra tài khoản và mật khẩu
            return _userRepository.Authenticate(taiKhoan, matKhau);
        }
    }
}

