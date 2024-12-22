using System;
using System.Collections.Generic;

namespace WebTuiXach_Gateway.Models
{
    public partial class TuiXach
    {
        public string MaDanhMuc { get; set; }      
        public string MaSp { get; set; }            
        public string TenSp { get; set; }
        public decimal GiaSp { get; set; }
        public decimal? KhuyenMai { get; set; }      
        public int? TonKho { get; set; }            
        public string MauSac { get; set; }
        public string MaSize { get; set; }           
        public string MoTa { get; set; }
        public byte[] HinhAnh { get; set; }          
        public int SoLuotDanhGia { get; set; } = 0; 

        // Quan hệ với các bảng khác
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
        public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();
    }
}
