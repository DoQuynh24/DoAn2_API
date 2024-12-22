using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IMauSacRepository
    {
        // Thêm màu sác sản phẩm
        bool Create(MauSacModel model);

        // Cập nhật thông tin màu
        bool Update(MauSacModel model);

        // Xóa danh mục sản phẩm
        bool Delete(string tenMau);

        //Lấy tất cả màu
        List<MauSacModel> GetDataAll();
        // Tìm kiếm danh mục sản phẩm theo các tiêu chí
        List<MauSacModel> Search(int pageIndex, int pageSize, out long total, string tenMau);
    }
}
