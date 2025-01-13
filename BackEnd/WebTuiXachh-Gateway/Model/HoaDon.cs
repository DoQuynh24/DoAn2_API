using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class HoaDon
    {
        public int MaHD { get; set; }
        public int PerID { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public DateTime NgayDatHang { get; set; } = DateTime.Now;
        public string TrangThai { get; set; } = "Đang xử lý";
        public DateTime NgayNhanHang { get; set; }

        // Quan hệ 1-n: Một hóa đơn thuộc về một người dùng
        public virtual User User { get; set; } = null!;

        // Quan hệ 1-n: Một hóa đơn có nhiều chi tiết hóa đơn
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}
