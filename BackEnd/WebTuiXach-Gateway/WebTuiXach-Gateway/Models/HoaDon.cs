using System;
using System.Collections.Generic;

namespace WebTuiXach_Gateway.Models
{
    public partial class HoaDon
    {
        public int MaHD { get; set; }               
        public int PerID { get; set; }             
        public DateTime NgayBan { get; set; }
        public string TrangThai { get; set; } = "Đang xử lý";

        public virtual User Users { get; set; } = null!;
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}
