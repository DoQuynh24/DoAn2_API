using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IDanhMucSanPhamRepository
    {
        // Thêm danh mục sản phẩm
        bool Create(DanhMucSanPhamModel model);

        // Cập nhật thông tin danh mục sản phẩm
        bool Update(DanhMucSanPhamModel model);

        // Xóa danh mục sản phẩm
        bool Delete(string maDanhMuc);

        // Lấy thông tin danh mục sản phẩm theo mã danh mục
        DanhMucSanPhamModel GetDatabyID(string maDanhMuc);

        // Tìm kiếm danh mục sản phẩm theo các tiêu chí
        List<DanhMucSanPhamModel> Search(int pageIndex, int pageSize, out long total, string maDanhMuc, string tenDanhMuc);
        List<DanhMucSanPhamModel> GetDataAll();
    }
}
