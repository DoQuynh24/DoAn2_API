using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class DanhMucSanPham
    {
        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        // Quan hệ 1-n: Một danh mục có nhiều sản phẩm
        public virtual ICollection<TuiXach> TuiXachs { get; set; } = new List<TuiXach>();
    }
}
