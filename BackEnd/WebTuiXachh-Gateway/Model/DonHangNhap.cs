using System;
using System.Collections.Generic;
namespace WebTuiXachh_Gateway.Model
{
    public partial class DonHangNhap
    {
        public int MaDHN { get; set; }
        public string TenNCC { get; set; }
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        public virtual ICollection<ChiTietDonHangNhap> ChiTietDonHangNhaps { get; set; } = new List<ChiTietDonHangNhap>();

    }
}
