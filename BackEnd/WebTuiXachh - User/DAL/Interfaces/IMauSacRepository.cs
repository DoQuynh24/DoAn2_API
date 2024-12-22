using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IMauSacRepository
    {
      
        //Lấy tất cả màu
        List<MauSacModel> GetDataAll();
        // Tìm kiếm danh mục sản phẩm theo các tiêu chí
        List<MauSacModel> Search(int pageIndex, int pageSize, out long total, string tenMau);
    }
}
