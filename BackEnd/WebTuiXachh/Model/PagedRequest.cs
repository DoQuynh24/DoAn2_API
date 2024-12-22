using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PagedRequest
    {
        public int PageIndex { get; set; } = 1; // Trang mặc định
        public int PageSize { get; set; } = 6; // Kích thước trang mặc định
    }
}
