using System;
using System.Collections.Generic;

namespace WebTuiXach_Gateway.Models
{
    public partial class TinTuc
    {
        public int MaTinTuc { get; set; }          
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public byte[] HinhAnh { get; set; }        
        public DateTime NgayDang { get; set; }     
        public string NguoiDang { get; set; }
    }
}
