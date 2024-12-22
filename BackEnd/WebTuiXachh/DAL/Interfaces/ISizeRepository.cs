using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ISizeRepository
    {
        // Thêm kích cỡ
        bool Create(SizeModel model);

        // Cập nhật kích cỡ
        bool Update(SizeModel model);

        // Lấy kích cỡ theo mã size
        SizeModel GetDatabyID(string maSize);
        // Xóa kích cỡ theo mã size
        bool Delete(string maSize);

        // Lấy tất cả kích cỡ
        List<SizeModel> GetDataAll();

        // Tìm kiếm kích cỡ theo các tiêu chí (ví dụ: mã size)
        List<SizeModel> Search(int pageIndex, int pageSize, out long total, string maSize);
    }
}
