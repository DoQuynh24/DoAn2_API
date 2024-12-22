using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; // Thêm thư viện này để sử dụng IFormFile

namespace DAL
{
    public interface ITuiXachRepository
    {
       
        TuiXachModel GetTuiXachById(string maSp);
        List<TuiXachModel> GetDataAll();
        TuiXachModel GetByDanhMuc(string maDanhMuc);
        TuiXachModel GetByMauSac(string tenMau);
        TuiXachModel GetBySize(string maSize);
        List<TuiXachModel> Search(int pageIndex, int pageSize, out long total, string item_group_id, string item_name, string size, decimal? minPrice, decimal? maxPrice);
    }
}
