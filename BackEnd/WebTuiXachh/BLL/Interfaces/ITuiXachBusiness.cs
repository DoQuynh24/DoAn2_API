using Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; // Thêm thư viện này để sử dụng IFormFile

namespace BLL
{
    public interface ITuiXachBusiness
    {
        // Thêm túi xách với hình ảnh
        bool CreateTuiXach(TuiXachModel model, IFormFile hinhAnhFile);

        // Cập nhật túi xách với hình ảnh
        bool UpdateTuiXach(TuiXachModel model, IFormFile hinhAnhFile);

        bool DeleteTuiXach(string maSp);
        TuiXachModel GetTuiXachById(string maSp);
        TuiXachModel GetByDanhMuc(string maDanhMuc);
        TuiXachModel GetByMauSac(string tenMau);
        TuiXachModel GetBySize(string maSize);
        List<TuiXachModel> GetDataAll();
        List<TuiXachModel> SearchTuiXachs(int pageIndex, int pageSize, out long total, string searchTerm = "", string color = "", string size = "", decimal? minPrice = null, decimal? maxPrice = null);
  
    }
}
