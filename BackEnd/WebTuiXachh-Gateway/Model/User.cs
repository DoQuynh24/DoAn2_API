using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class User
    {
        public int PerID { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? Role { get; set; } = "Khách hàng";
        public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>(); 
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>(); 
    }
}
