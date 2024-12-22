
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
namespace Model
{
    public class UserModel
    {
        public int PerID { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; } 
        public string? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? Role { get; set; } = "Khách hàng";

    }
}