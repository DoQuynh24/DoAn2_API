using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model
{
    public class TuiXachModel
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
        public string HinhAnh { get; set; }  // VARCHAR(MAX)
        public int SoLuotDanhGia { get; set; } = 0; // Số lượt đánh giá
      
    }
}

