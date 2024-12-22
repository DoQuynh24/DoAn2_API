using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DonHangNhapModel
    {
        public int MaDHN {  get; set; }
        public string TenNCC { get; set; }
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        public List<ChiTietDonHangNhapModel> ChiTietDonHangNhaps { get; set; }
        public List<int> ChiTietDonHangNhapsToDelete { get; set; }
    }
}
