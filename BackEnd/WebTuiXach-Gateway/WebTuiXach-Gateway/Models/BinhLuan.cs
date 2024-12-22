using System;
using System.Collections.Generic;

namespace WebTuiXach_Gateway.Models
{
    public partial class BinhLuan
    {
        public int MaBinhLuan { get; set; }
        public int PerID { get; set; }
        public string MaSp { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayBinhLuan { get; set; } = DateTime.Now;
        public virtual User Users { get; set; } = null!;
        public virtual TuiXach SanPham { get; set; } 
    }
}
