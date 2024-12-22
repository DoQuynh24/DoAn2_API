using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface IDanhMucSanPhamBusiness
    {
      
        DanhMucSanPhamModel GetDanhMucById(string MaDanhMuc);
        List<DanhMucSanPhamModel> GetAllDanhMuc();
      
    }
}
