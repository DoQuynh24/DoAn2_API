using System;
using System.Collections.Generic;
namespace WebTuiXachh_Gateway.Model
{
    public partial class ChiTietDonHangNhap
    {
        public int MaDHN { get; set; }
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public decimal GiaNhap { get; set; }
        public int SoLuong { get; set; }
        public virtual DonHangNhap DonHangNhap { get; set; } = null!;
    }
}
