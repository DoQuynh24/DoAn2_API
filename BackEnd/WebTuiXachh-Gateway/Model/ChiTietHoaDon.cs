using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class ChiTietHoaDon
    {
        public int MaHD { get; set; }
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public string TenMau { get; set; }
        public string MaSize { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public string GhiChu { get; set; }
        public decimal KhuyenMai { get; set; }

        // Quan hệ n-1: Một chi tiết hóa đơn thuộc về một hóa đơn
        public virtual HoaDon HoaDon { get; set; } = null!;

        // Quan hệ n-1: Một chi tiết hóa đơn thuộc về một sản phẩm
        public virtual TuiXach TuiXach { get; set; } = null!;
    }
}
