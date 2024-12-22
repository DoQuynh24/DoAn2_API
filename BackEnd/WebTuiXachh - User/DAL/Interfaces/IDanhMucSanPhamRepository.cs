using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IDanhMucSanPhamRepository
    {
 

       
        DanhMucSanPhamModel GetDatabyID(string maDanhMuc);
        List<DanhMucSanPhamModel> GetDataAll();
    }
}
