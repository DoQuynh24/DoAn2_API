using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HoaDonModel
    {
        public int MaHD { get; set; }
        public int PerID { get; set; }
        public string HoTen { get; set; }
        public string  DiaChi { get; set; }
        public string SDT { get; set; }
        public DateTime NgayDatHang { get; set; } = DateTime.Now; 
        public string TrangThai { get; set; } = "Đang xử lý"; 
        public DateTime NgayNhanHang { get; set; } 
        public List<ChiTietHoaDonModel> ChiTietHoaDons { get; set; } 

    }
}

