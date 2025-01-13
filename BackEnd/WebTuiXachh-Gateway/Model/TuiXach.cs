using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class TuiXach
    {
        public string MaDanhMuc { get; set; }
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public string TenMau { get; set; }
        public string MaSize { get; set; }
        public decimal GiaBan { get; set; }
        public decimal KhuyenMai { get; set; }
        public int TonKho { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; } 
        public int SoLuotDanhGia { get; set; } = 0; // Số lượt đánh giá

        // Quan hệ 1-n: Một sản phẩm thuộc về một danh mục
        public virtual DanhMucSanPham  DanhMucSanPham { get; set; } = null!;

        // Quan hệ 1-n: Một sản phẩm có thể có nhiều chi tiết hóa đơn
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

        // Quan hệ 1-n: Một sản phẩm có thể có nhiều bình luận
        public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();
    }
}
