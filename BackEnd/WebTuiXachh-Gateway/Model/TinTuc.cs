using System;
using System.Collections.Generic;

namespace WebTuiXachh_Gateway.Model
{
    public partial class TinTuc
    {
        public int MaTinTuc { get; set; }          
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string HinhAnh { get; set; }        
        public DateTime NgayDang { get; set; }     
        public string NguoiDang { get; set; }
    }
}
