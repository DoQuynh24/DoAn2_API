using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using DAL.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAL
{
    public partial class UserRepository : IUserRepository
    {
        private IDatabaseHelper _dbHelper;

        public UserRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool Create(UserModel model)
        {
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(
                    out string msgError,
                    "sp_user_create",
                    "@taikhoan", model.TaiKhoan,
                    "@matkhau", model.MatKhau,
                    "@hoten", model.HoTen,
                    "@ngaysinh", model.NgaySinh == DateTime.MinValue ? (object)DBNull.Value : model.NgaySinh,
                    "@gioitinh", string.IsNullOrEmpty(model.GioiTinh) ? "Chưa cập nhật" : model.GioiTinh,
                    "@diachi", string.IsNullOrEmpty(model.DiaChi) ?"Chưa cập nhật": model.DiaChi,
                    "@role", string.IsNullOrEmpty(model.Role) ? "Khách hàng" : model.Role
                );

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CreateUser: {ex.Message}", ex);
            }
        }


        public bool Update(UserModel model)
        {
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out string msgError, "sp_user_update",
                    "@per_id", model.PerID,
                    "@taikhoan", model.TaiKhoan,
                    "@matkhau", string.IsNullOrEmpty(model.MatKhau) ? (object)DBNull.Value : model.MatKhau,  
                    "@hoten", string.IsNullOrEmpty(model.HoTen) ? (object)DBNull.Value : model.HoTen,  
                    "@ngaysinh", model.NgaySinh == DateTime.MinValue ? (object)DBNull.Value : model.NgaySinh, 
                    "@gioitinh", string.IsNullOrEmpty(model.GioiTinh) ? (object)DBNull.Value : model.GioiTinh,  
                    "@diachi", string.IsNullOrEmpty(model.DiaChi) ? (object)DBNull.Value : model.DiaChi,  
                    "@role", string.IsNullOrEmpty(model.Role) ? (object)DBNull.Value : model.Role);  

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in UpdateUser: {ex.Message}", ex);
            }
        }





        // Phương thức xóa người dùng
        public bool Delete(int perID)
        {
            string msgError = "";
            try
            {
                var result = _dbHelper.ExecuteScalarSProcedureWithTransaction(out msgError, "sp_user_delete",
                    "@per_id", perID);

                if (!string.IsNullOrEmpty(msgError))
                {
                    throw new Exception(msgError);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Phương thức lấy dữ liệu người dùng theo ID
        public UserModel GetDatabyID(int perID)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_get_by_id",
                    "@per_id", perID);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<UserModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Phương thức lấy tất cả người dùng
        public List<UserModel> GetDataAll()
        {
            string msgError = "";
            try
            {
                // Thực hiện truy vấn tất cả người dùng từ cơ sở dữ liệu
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_get_all");

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                // Chuyển đổi kết quả thành danh sách UserModel
                return dt.ConvertTo<UserModel>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDataAll: {ex.Message}");
                throw;  // Ném lại lỗi để giữ nguyên thông tin
            }
        }


        // Phương thức tìm kiếm người dùng với phân trang

        public List<UserModel> Search(int pageIndex, int pageSize, out long total,string taikhoan, string hoten, string diachi)
        {
            string msgError = "";
            try
            {
                // Gọi stored procedure và lấy kết quả dưới dạng DataTable
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_search",
                    "@page_index", pageIndex,
                    "@page_size", pageSize,
                    "@taikhoan", taikhoan ?? string.Empty,
                    "@hoten", hoten ?? string.Empty,
                    "@diachi", diachi ?? string.Empty);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                if (dt == null || dt.Rows.Count == 0)
                {
                    total = 0;
                    return new List<UserModel>();
                }

                // Lấy tổng số bản ghi từ cột TotalRecords của dòng đầu tiên
                total = Convert.ToInt64(dt.Rows[0]["TotalRecords"]);

                // Chuyển đổi DataTable sang danh sách UserModel
                var users = dt.ConvertTo<UserModel>().ToList();
                return users;
            }
            catch (Exception ex)
            {
                total = 0;
                throw new Exception("Lỗi khi thực hiện tìm kiếm: " + ex.Message);
            }
        }



        // Phương thức xác thực người dùng
        public UserModel Authenticate(string username, string password)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_user_authenticate",
                    "@taikhoan", username,
                    "@matkhau", password);

                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);

                return dt.ConvertTo<UserModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
