using Model;
using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IHoaDonRepository
    {
        // Thêm hóa đơn
        int Create(HoaDonModel model);

        // Cập nhật thông tin hóa đơn
        bool Update(HoaDonModel model);
        bool UpdateTrangThai(HoaDonModel model);
        List<HoaDonModel> GetAll();
        HoaDonModel GetDatabyIDHD(int maHD);
        List<HoaDonModel> GetByTrangThai(string trangThai);
        //// Xóa hóa đơn
        //bool Delete(int maHD);

        //// Lấy thông tin hóa đơn theo mã hóa đơn
        //HoaDonModel GetDatabyID(int maHD);

    }
}
