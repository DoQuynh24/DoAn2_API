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

        public bool UpdateUser(UserModel model)
            => _userRepository.Update(model); 


        public bool DeleteUser(int perID) => _userRepository.Delete(perID);

        public UserModel GetUserById(int perID) => _userRepository.GetDatabyID(perID);

       
        // Cài đặt phương thức Authenticate
        public UserModel Authenticate(string taiKhoan, string matKhau)
        {
            // Kiểm tra tài khoản và mật khẩu
            return _userRepository.Authenticate(taiKhoan, matKhau);
        }
    }
}

