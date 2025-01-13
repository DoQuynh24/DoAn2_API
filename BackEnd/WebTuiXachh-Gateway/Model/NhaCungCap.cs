using System;
using System.Collections.Generic;
namespace WebTuiXachh_Gateway.Model
{
    public partial class NhaCungCap
    {
        public string TenNCC { get; set; }
        public string? DiaChi { get; set; } = "Chưa cập nhật";
        public string? LienHe { get; set; } = "Chưa cập nhật";
        // Quan hệ 1-n: Một nhà cung cấp có nhiều sản phẩm
        public virtual ICollection<TuiXach> TuiXachs { get; set; } = new List<TuiXach>();
    }
}
