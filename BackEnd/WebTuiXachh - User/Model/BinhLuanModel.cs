using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BinhLuanModel
    {
        public int MaBinhLuan { get; set; }
        public int PerID { get; set; }
        public string MaSp { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayBinhLuan { get; set; } = DateTime.Now;
    }
}

