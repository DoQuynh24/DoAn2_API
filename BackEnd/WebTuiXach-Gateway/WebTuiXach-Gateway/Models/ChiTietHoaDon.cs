using System;
using System.Collections.Generic;

namespace WebTuiXach_Gateway.Models
{
    public partial class ChiTietHoaDon
    {
        public int MaChiTietHD { get; set; }         
        public int MaHD { get; set; }               
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public string MauSac { get; set; }
        public string MaSize { get; set; }           
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal KhuyenMai { get; set; }
        public string GhiChu { get; set; }

        public virtual HoaDon HoaDon { get; set; }   // Liên kết với HoaDon
        public virtual TuiXach SanPham { get; set; } // Liên kết với TuiXach
    }
}
