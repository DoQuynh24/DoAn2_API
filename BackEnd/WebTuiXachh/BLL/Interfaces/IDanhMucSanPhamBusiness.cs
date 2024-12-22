using Model;
using System.Collections.Generic;

namespace BLL
{
    public interface IDanhMucSanPhamBusiness
    {
        bool CreateDanhMuc(DanhMucSanPhamModel model);
        bool UpdateDanhMuc(DanhMucSanPhamModel model);
        bool DeleteDanhMuc(string MaDanhMuc);
        DanhMucSanPhamModel GetDanhMucById(string MaDanhMuc);
        List<DanhMucSanPhamModel> GetAllDanhMuc();
        List<DanhMucSanPhamModel> SearchDanhMucs(int pageIndex, int pageSize, out long total, string maDanhMuc, string tenDanhMuc);
    }
}
