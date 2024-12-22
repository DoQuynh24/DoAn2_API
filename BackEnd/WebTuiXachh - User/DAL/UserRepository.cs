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
