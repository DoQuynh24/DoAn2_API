using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class BinhLuan
    {
        public int MaBinhLuan { get; set; }
        public int PerID { get; set; }
        public string MaSp { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayBinhLuan { get; set; } = DateTime.Now;
        // Quan hệ n-1: Một bình luận thuộc về một sản phẩm
        public virtual TuiXach TuiXach { get; set; } = null!;

        // Quan hệ n-1: Một bình luận thuộc về một người dùng
        public virtual User User { get; set; } = null!;
    }
}
