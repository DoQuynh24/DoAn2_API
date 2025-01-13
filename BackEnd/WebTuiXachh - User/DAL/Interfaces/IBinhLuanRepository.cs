using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IBinhLuanRepository
    {
        // Thêm bình luận
        bool Create(BinhLuanModel model);

        // Cập nhật bình luận
        bool Update(BinhLuanModel model);

        // Xóa bình luận
        bool Delete(int maBinhLuan);

        // Lấy thông tin bình luận theo mã bình luận
        BinhLuanModel GetDatabyID(int maBinhLuan);

        // Tìm kiếm bình luận theo các tiêu chí
        List<BinhLuanModel> Search(int pageIndex, int pageSize, out long total, string maSP, int perID); 

        // Thêm phương thức GetDataAll để lấy tất cả bình luận
        List<BinhLuanModel> GetDataAll();


    }
}
