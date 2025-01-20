using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; // Thêm thư viện này để sử dụng IFormFile

namespace DAL
{
    public interface ITuiXachRepository
    {
        // Thêm túi xách
        bool Create(TuiXachModel model, IFormFile hinhAnhFile);  // Thêm tham số IFormFile để xử lý ảnh

        // Cập nhật túi xách
        bool Update(TuiXachModel model, IFormFile hinhAnhFile);  // Thêm tham số IFormFile để xử lý ảnh

        //Xóa túi xách
        bool Delete(string maSp);
        // Lấy túi xách theo mã sản phẩm
        TuiXachModel GetTuiXachById(string maSp);

        // Lấy tất cả túi xách
        List<TuiXachModel> GetDataAll();
        TuiXachModel GetByDanhMuc(string maDanhMuc);
        TuiXachModel GetByMauSac(string tenMau);
        TuiXachModel GetBySize(string maSize);

        // Tìm kiếm túi xách theo các tiêu chí (ví dụ: tên sản phẩm, màu sắc, kích thước, giá tiền)
        List<TuiXachModel> Search(int pageIndex,int pageSize,  out long total,string madanhmuc = "",string masp = "",string tensp = "",string tenmau = "",string masize = "",decimal? giabanMin = null,decimal? giabanMax = null);

    }
}
